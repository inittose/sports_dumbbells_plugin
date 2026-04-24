using Microsoft.VisualBasic.Devices;
using Microsoft.Win32.SafeHandles;
using SportsDumbbellsPlugin.Wrapper;
using SportsDumbbellsPluginCore.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace StressTesting
{
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            var config = StressTestConfig.Parse(args);

            Directory.CreateDirectory(config.OutputDirectory);

            var metricsPath = Path.Combine(config.OutputDirectory, "metrics.csv");
            var infoPath = Path.Combine(config.OutputDirectory, "run-info.txt");

            using var metricsWriter = new StreamWriter(
                metricsPath,
                append: false,
                encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: true))
            {
                AutoFlush = true
            };

            using var infoWriter = new StreamWriter(
                infoPath,
                append: false,
                encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: true))
            {
                AutoFlush = true
            };

            WriteRunInfo(infoWriter, config);
            WriteMetricsHeader(metricsWriter);

            SafeJobHandle? jobHandle = null;

            try
            {
                if (config.ProcessMemoryLimitMb.HasValue)
                {
                    jobHandle = JobMemoryLimiter.ApplyProcessMemoryLimit(
                        config.ProcessMemoryLimitMb.Value);
                }

                var parameters = CreateTestParameters();
                using var wrapper = new Wrapper();
                var builder = new Builder(wrapper);

                var stopwatch = new Stopwatch();
                var computerInfo = new ComputerInfo();
                var currentProcess = Process.GetCurrentProcess();

                var ballast = new List<byte[]>();

                var cancelled = false;
                Console.CancelKeyPress += (_, eventArgs) =>
                {
                    eventArgs.Cancel = true;
                    cancelled = true;
                };

                var iteration = 0;

                while (!cancelled && iteration < config.MaxIterations)
                {
                    iteration++;

                    var status = "OK";
                    var exceptionText = string.Empty;

                    stopwatch.Restart();

                    try
                    {
                        builder.Build(parameters);

                        if (config.BallastMbPerIteration > 0)
                        {
                            ballast.Add(CreateBallastChunk(
                                config.BallastMbPerIteration));
                        }
                    }
                    catch (Exception exception)
                    {
                        status = "FAIL";
                        exceptionText =
                            $"{exception.GetType().Name}: {exception.Message}";
                    }
                    finally
                    {
                        stopwatch.Stop();

                        try
                        {
                            wrapper.CloseActiveDocument();
                        }
                        catch
                        {
                            // Не мешаем логированию метрик итерации.
                        }
                    }

                    currentProcess.Refresh();

                    var buildTimeMs = Math.Max(1, stopwatch.ElapsedMilliseconds);

                    var usedRamGb =
                        (computerInfo.TotalPhysicalMemory
                         - computerInfo.AvailablePhysicalMemory)
                        / 1073741824.0;

                    var processWorkingSetMb =
                        currentProcess.WorkingSet64 / (1024.0 * 1024.0);

                    metricsWriter.WriteLine(string.Join(";",
                        iteration,
                        buildTimeMs,
                        usedRamGb.ToString("F6", CultureInfo.InvariantCulture),
                        processWorkingSetMb.ToString("F2", CultureInfo.InvariantCulture),
                        status,
                        EscapeCsv(exceptionText)));

                    Console.WriteLine(
                        $"[{iteration}] " +
                        $"Build={buildTimeMs} ms, " +
                        $"RAM={usedRamGb:F3} GB, " +
                        $"WS={processWorkingSetMb:F2} MB, " +
                        $"Status={status}");

                    if (status == "FAIL")
                    {
                        infoWriter.WriteLine();
                        infoWriter.WriteLine(
                            $"Завершение по ошибке на итерации: {iteration}");
                        infoWriter.WriteLine(
                            $"RAM всей системы: {usedRamGb:F6} GB");
                        infoWriter.WriteLine(
                            $"RAM процесса (Working Set): " +
                            $"{processWorkingSetMb:F2} MB");
                        infoWriter.WriteLine(
                            $"Исключение: {exceptionText}");

                        return 1;
                    }
                }

                infoWriter.WriteLine();
                infoWriter.WriteLine(
                    "Тест завершён без отказа в пределах заданного числа итераций.");

                return 0;
            }
            catch (Exception exception)
            {
                infoWriter.WriteLine();
                infoWriter.WriteLine("Фатальная ошибка вне основного цикла.");
                infoWriter.WriteLine(exception.ToString());

                return 2;
            }
            finally
            {
                jobHandle?.Dispose();
            }
        }

        private static void WriteRunInfo(
            StreamWriter writer,
            StressTestConfig config)
        {
            writer.WriteLine("Stress testing run info");
            writer.WriteLine($"UTC start time: {DateTime.UtcNow:O}");
            writer.WriteLine($"Machine: {Environment.MachineName}");
            writer.WriteLine($"OS: {RuntimeInformation.OSDescription}");
            writer.WriteLine($".NET: {RuntimeInformation.FrameworkDescription}");
            writer.WriteLine(
                $"Process memory limit, MB: " +
                $"{(config.ProcessMemoryLimitMb?.ToString() ?? "not set")}");
            writer.WriteLine(
                $"Ballast MB per iteration: {config.BallastMbPerIteration}");
            writer.WriteLine($"Max iterations: {config.MaxIterations}");
            writer.WriteLine($"Output directory: {config.OutputDirectory}");
        }

        private static void WriteMetricsHeader(StreamWriter writer)
        {
            writer.WriteLine(
                "Iteration;" +
                "BuildTimeMs;" +
                "UsedRamGb;" +
                "ProcessWorkingSetMb;" +
                "Status;" +
                "Exception");
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>
        /// Создаёт дополнительную нагрузку на память процесса.
        /// Нужна только для теста до отказа.
        /// </summary>
        private static byte[] CreateBallastChunk(int sizeMb)
        {
            var bytes = new byte[sizeMb * 1024 * 1024];

            // Трогаем память по страницам, чтобы она реально выделилась.
            for (var i = 0; i < bytes.Length; i += 4096)
            {
                bytes[i] = 1;
            }

            return bytes;
        }

        /// <summary>
        /// Создаёт набор корректных параметров средней сложности
        /// для нагрузочного тестирования.
        /// </summary>
        private static DumbbellParameters CreateTestParameters()
        {
            var parameters = new DumbbellParameters
            {
                Rod = new RodParameters
                {
                    HandleLength = 140.0,
                    SeatLength = 100.0,
                    HandleDiameter = 30.0,
                    SeatDiameter = 28.0,
                    GrooveCount = 5,
                    GrooveDepth = 0.5,
                },
                DisksPerSide = 3,
            };

            parameters.Disks.Add(new DiskParameters
            {
                OuterDiameter = 180.0,
                HoleDiameter = 29.0,
                Thickness = 20.0,
                FilletDiameter = 4.0,
            });

            parameters.Disks.Add(new DiskParameters
            {
                OuterDiameter = 160.0,
                HoleDiameter = 29.0,
                Thickness = 15.0,
                FilletDiameter = 4.0,
            });

            parameters.Disks.Add(new DiskParameters
            {
                OuterDiameter = 140.0,
                HoleDiameter = 29.0,
                Thickness = 10.0,
                FilletDiameter = 4.0,
            });

            return parameters;
        }
    }

    internal sealed class StressTestConfig
    {
        public int MaxIterations { get; private set; } = int.MaxValue;

        public int? ProcessMemoryLimitMb { get; private set; }

        public int BallastMbPerIteration { get; private set; } = 0;

        public string OutputDirectory { get; private set; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stress-test-output");

        public static StressTestConfig Parse(string[] args)
        {
            var config = new StressTestConfig();

            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--max-iterations":
                        config.MaxIterations = int.Parse(
                            args[++i],
                            CultureInfo.InvariantCulture);
                        break;

                    case "--memory-limit-mb":
                        config.ProcessMemoryLimitMb = int.Parse(
                            args[++i],
                            CultureInfo.InvariantCulture);
                        break;

                    case "--ballast-mb-per-iteration":
                        config.BallastMbPerIteration = int.Parse(
                            args[++i],
                            CultureInfo.InvariantCulture);
                        break;

                    case "--output":
                        config.OutputDirectory = args[++i];
                        break;
                }
            }

            return config;
        }
    }

    internal static class JobMemoryLimiter
    {
        private const int JobObjectExtendedLimitInformation = 9;
        private const uint JOB_OBJECT_LIMIT_PROCESS_MEMORY = 0x00000100;
        private const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000;

        public static SafeJobHandle ApplyProcessMemoryLimit(int memoryLimitMb)
        {
            if (memoryLimitMb <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(memoryLimitMb));
            }

            var jobHandle = CreateJobObject(IntPtr.Zero, null);
            if (jobHandle.IsInvalid)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var limitInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = new JOBOBJECT_BASIC_LIMIT_INFORMATION
                {
                    LimitFlags =
                        JOB_OBJECT_LIMIT_PROCESS_MEMORY
                        | JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
                },
                ProcessMemoryLimit =
                    (UIntPtr)((ulong)memoryLimitMb * 1024UL * 1024UL),
            };

            var size = Marshal.SizeOf<JOBOBJECT_EXTENDED_LIMIT_INFORMATION>();

            if (!SetInformationJobObject(
                    jobHandle,
                    JobObjectExtendedLimitInformation,
                    ref limitInfo,
                    (uint)size))
            {
                jobHandle.Dispose();
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            if (!AssignProcessToJobObject(jobHandle, GetCurrentProcess()))
            {
                jobHandle.Dispose();
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return jobHandle;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern SafeJobHandle CreateJobObject(
            IntPtr lpJobAttributes,
            string? lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetInformationJobObject(
            SafeJobHandle hJob,
            int jobObjectInfoClass,
            ref JOBOBJECT_EXTENDED_LIMIT_INFORMATION lpJobObjectInfo,
            uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AssignProcessToJobObject(
            SafeJobHandle job,
            IntPtr process);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [StructLayout(LayoutKind.Sequential)]
        private struct IO_COUNTERS
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            public long PerProcessUserTimeLimit;
            public long PerJobUserTimeLimit;
            public uint LimitFlags;
            public UIntPtr MinimumWorkingSetSize;
            public UIntPtr MaximumWorkingSetSize;
            public uint ActiveProcessLimit;
            public nuint Affinity;
            public uint PriorityClass;
            public uint SchedulingClass;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
            public IO_COUNTERS IoInfo;
            public UIntPtr ProcessMemoryLimit;
            public UIntPtr JobMemoryLimit;
            public UIntPtr PeakProcessMemoryUsed;
            public UIntPtr PeakJobMemoryUsed;
        }
    }

    internal sealed class SafeJobHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafeJobHandle() : base(ownsHandle: true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return CloseHandle(handle);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
    }
}