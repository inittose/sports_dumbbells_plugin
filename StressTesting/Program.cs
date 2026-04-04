using SportsDumbbellsPlugin.Wrapper;
using SportsDumbbellsPluginCore.Model;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.VisualBasic.Devices;

namespace StressTesting
{
    internal static class Program
    {
        /// <summary>
        /// Точка входа в приложение нагрузочного тестирования.
        /// Выполняет многократное построение модели гантели и
        /// логирует время построения, использование ОЗУ,
        /// загрузку CPU процесса и Working Set процесса.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        [STAThread]
        private static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;

            const int iterationsCount = 2000;
            const double bytesToGigabytes = 1.0 / 1073741824.0;
            const double bytesToMegabytes = 1.0 / (1024.0 * 1024.0);

            var parameters = CreateTestParameters();
            using var wrapper = new Wrapper();
            var builder = new Builder(wrapper);

            var stopwatch = new Stopwatch();
            var computerInfo = new ComputerInfo();
            var currentProcess = Process.GetCurrentProcess();

            var logPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "log.txt");

            using var streamWriter = new StreamWriter(
                logPath,
                append: false,
                encoding: Encoding.UTF8)
            {
                AutoFlush = true
            };

            streamWriter.WriteLine(
                "Iteration\tBuildTimeMs\tUsedRamGb\tCpuProcessPercent\t"
                + "ProcessWorkingSetMb");

            var cancelled = false;

            Console.CancelKeyPress += (_, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cancelled = true;
            };

            try
            {
                for (var iteration = 1;
                    iteration <= iterationsCount && !cancelled;
                    iteration++)
                {
                    currentProcess.Refresh();
                    var cpuStart = currentProcess.TotalProcessorTime;

                    stopwatch.Restart();

                    builder.Build(parameters);

                    // Чтобы каждая итерация была независимой и не
                    // накапливала открытые документы КОМПАС-3D.
                    wrapper.CloseActiveDocument();

                    stopwatch.Stop();

                    currentProcess.Refresh();
                    var cpuEnd = currentProcess.TotalProcessorTime;

                    var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                    if (elapsedMilliseconds <= 0)
                    {
                        elapsedMilliseconds = 1;
                    }

                    var cpuDelta = cpuEnd - cpuStart;

                    var cpuPercent =
                        cpuDelta.TotalMilliseconds
                        / (elapsedMilliseconds * Environment.ProcessorCount)
                        * 100.0;

                    var usedRamGigabytes =
                        (computerInfo.TotalPhysicalMemory
                         - computerInfo.AvailablePhysicalMemory)
                        * bytesToGigabytes;

                    var processWorkingSetMegabytes =
                        currentProcess.WorkingSet64 * bytesToMegabytes;

                    streamWriter.WriteLine(
                        $"{iteration}\t" +
                        $"{elapsedMilliseconds}\t" +
                        $"{usedRamGigabytes:F6}\t" +
                        $"{cpuPercent:F2}\t" +
                        $"{processWorkingSetMegabytes:F2}");

                    Console.WriteLine(
                        $"[{iteration}/{iterationsCount}] "
                        + $"Build={elapsedMilliseconds} ms, "
                        + $"RAM={usedRamGigabytes:F3} GB, "
                        + $"CPU={cpuPercent:F2} %, "
                        + $"WS={processWorkingSetMegabytes:F2} MB");
                }
            }
            catch (Exception exception)
            {
                streamWriter.WriteLine();
                streamWriter.WriteLine("ERROR");
                streamWriter.WriteLine(exception.ToString());

                Console.WriteLine();
                Console.WriteLine(
                    "Во время нагрузочного тестирования произошла ошибка:");
                Console.WriteLine(exception);
            }
            finally
            {
                wrapper.CloseActiveDocument();
            }
        }

        /// <summary>
        /// Создаёт набор корректных параметров средней сложности
        /// для нагрузочного тестирования.
        /// </summary>
        /// <returns>Параметры гантели.</returns>
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
}