using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;
using System.Runtime.InteropServices;

namespace SportsDumbbellsPlugin.Wrapper
{
    public sealed class Wrapper : IDisposable
    {
        private KompasObject? _kompas;
        private ksDocument3D? _doc3D;
        private ksPart? _topPart;
        private ksDocument2D? _current2dDoc;

        public void AttachOrRunCad(bool visible = true)
        {
            if (_kompas != null)
            {
                return;
            }

            var t = Type.GetTypeFromProgID("KOMPAS.Application.5");
            if (t == null)
            {
                throw new InvalidOperationException("Не найден ProgID KOMPAS.Application.5.");
            }

            _kompas = (KompasObject?)Activator.CreateInstance(t);
            if (_kompas == null)
            {
                throw new InvalidOperationException("Не удалось создать KompasObject.");
            }

            _kompas.Visible = visible;
            _kompas.ActivateControllerAPI();
        }

        public void CreateDocument3D(bool invisible = false)
        {
            EnsureKompas();

            CloseActiveDocument();

            _doc3D = (ksDocument3D?)_kompas!.Document3D();
            if (_doc3D == null)
            {
                throw new InvalidOperationException("Не удалось получить ksDocument3D.");
            }

            // В API5 есть перегрузки Create(), иногда Create(invisible, typeDoc).
            // В большинстве примеров достаточно Create().
            _doc3D.Create();

            _topPart = (ksPart?)_doc3D.GetPart((short)Part_Type.pTop_Part);
            if (_topPart == null)
            {
                throw new InvalidOperationException("Не удалось получить верхнюю деталь (pTop_Part).");
            }
        }

        /// <summary>
        /// Создаёт эскиз на базовой плоскости (XOY/XOZ/YOZ) и входит в режим редактирования.
        /// Возвращает сущность эскиза.
        /// </summary>
        public ksEntity CreateSketchOnPlane(string plane)
        {
            EnsureTopPart();

            short planeType = plane?.ToUpperInvariant() switch
            {
                "XOY" or "XY" => (short)Obj3dType.o3d_planeXOY,
                "XOZ" or "XZ" => (short)Obj3dType.o3d_planeXOZ,
                "YOZ" or "YZ" => (short)Obj3dType.o3d_planeYOZ,
                _ => (short)Obj3dType.o3d_planeXOY
            };

            var basePlane = (ksEntity?)_topPart!.GetDefaultEntity(planeType);
            if (basePlane == null)
            {
                throw new InvalidOperationException("Не удалось получить базовую плоскость для эскиза.");
            }

            var sketchEntity = (ksEntity?)_topPart.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketchEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать сущность эскиза (o3d_sketch).");
            }

            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.SetPlane(basePlane);
            sketchEntity.Create();

            _current2dDoc = (ksDocument2D?)sketchDef.BeginEdit();
            if (_current2dDoc == null)
            {
                throw new InvalidOperationException("Не удалось войти в редактирование эскиза (BeginEdit).");
            }

            return sketchEntity;
        }

        public void FinishSketch(ksEntity sketchEntity)
        {
            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.EndEdit();
            _current2dDoc = null;
        }

        // -------- 2D примитивы --------

        public void DrawCircle(double xc, double yc, double radius, int style = 1)
        {
            if (_current2dDoc == null)
            {
                throw new InvalidOperationException("Нет активного 2D-эскиза. Сначала вызови CreateSketchOnPlane().");
            }

            _current2dDoc.ksCircle(xc, yc, radius, style);
        }

        public ksEntity BuildCylinderOnXOY(double x, double y, double radius, double height)
        {
            var sketch = CreateSketchOnPlane("XOY");
            DrawCircle(x, y, radius);
            FinishSketch(sketch);

            return BossExtrusion(sketch, height, Direction_Type.dtNormal);
        }

        public void BuildDiskWithHoleOnXOY(double x, double y, double outerRadius, double holeRadius, double thickness)
        {
            // Тело диска
            var sketchOuter = CreateSketchOnPlane("XOY");
            DrawCircle(x, y, outerRadius);
            FinishSketch(sketchOuter);
            BossExtrusion(sketchOuter, thickness, Direction_Type.dtNormal);

            // Вырез отверстия
            var sketchHole = CreateSketchOnPlane("XOY");
            DrawCircle(x, y, holeRadius);
            FinishSketch(sketchHole);
            CutExtrusion(sketchHole, thickness + 1.0, forward: true);
        }

        public ksEntity CreateOffsetPlaneXOY(double offsetZ)
        {
            EnsureTopPart();

            var basePlane = (ksEntity?)_topPart!.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            if (basePlane == null)
            {
                throw new InvalidOperationException("Не удалось получить базовую плоскость XOY.");
            }

            var planeEntity = (ksEntity?)_topPart.NewEntity((short)Obj3dType.o3d_planeOffset);
            if (planeEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_planeOffset.");
            }

            var planeDef = (ksPlaneOffsetDefinition)planeEntity.GetDefinition();
            planeDef.SetPlane(basePlane);
            planeDef.offset = offsetZ;

            planeEntity.Create();
            return planeEntity;
        }

        public ksEntity CreateOffsetPlaneYOZ(double offsetX)
        {
            EnsureTopPart();

            var basePlane = (ksEntity?)_topPart!.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);
            if (basePlane == null)
            {
                throw new InvalidOperationException("Не удалось получить базовую плоскость YOZ.");
            }

            var planeEntity = (ksEntity?)_topPart.NewEntity((short)Obj3dType.o3d_planeOffset);
            if (planeEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_planeOffset.");
            }

            var planeDef = (ksPlaneOffsetDefinition)planeEntity.GetDefinition();
            planeDef.SetPlane(basePlane);
            planeDef.offset = offsetX;

            planeEntity.Create();
            return planeEntity;
        }

        public ksEntity CreateSketchOnPlane(ksEntity planeEntity)
        {
            EnsureTopPart();

            var sketchEntity = (ksEntity?)_topPart!.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketchEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать сущность эскиза (o3d_sketch).");
            }

            var sketchDef = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDef.SetPlane(planeEntity);
            sketchEntity.Create();

            _current2dDoc = (ksDocument2D?)sketchDef.BeginEdit();
            if (_current2dDoc == null)
            {
                throw new InvalidOperationException("Не удалось войти в редактирование эскиза (BeginEdit).");
            }

            return sketchEntity;
        }

        public void BuildDiskAtZ(double outerRadius, double holeRadius, double thickness, double z0)
        {
            var planeOuter = CreateOffsetPlaneXOY(z0);
            var sketchOuter = CreateSketchOnPlane(planeOuter);
            DrawCircle(0, 0, outerRadius);
            FinishSketch(sketchOuter);
            BossExtrusion(sketchOuter, thickness, Direction_Type.dtNormal);

            var planeHole = CreateOffsetPlaneXOY(z0);
            var sketchHole = CreateSketchOnPlane(planeHole);
            DrawCircle(0, 0, holeRadius);
            FinishSketch(sketchHole);
            CutExtrusion(sketchHole, thickness + 1.0, forward: true);
        }

        public void BuildDiskAtX(
            double outerRadius,
            double holeRadius,
            double thickness,
            double x0 = 0,
            Direction_Type direction = Direction_Type.dtNormal)
        {
            var planeOuter = CreateOffsetPlaneYOZ(x0);
            var sketchOuter = CreateSketchOnPlane(planeOuter);
            DrawCircle(0, 0, outerRadius);
            DrawCircle(0, 0, holeRadius);
            FinishSketch(sketchOuter);
            BossExtrusion(sketchOuter, thickness, direction);
        }

        public void BuildCylinderAtZ(double radius, double height, double z0)
        {
            var plane = CreateOffsetPlaneXOY(z0);
            var sketch = CreateSketchOnPlane(plane);

            DrawCircle(0, 0, radius);
            FinishSketch(sketch);

            BossExtrusion(sketch, height, Direction_Type.dtNormal);
        }

        public void BuildCylinderAtX(double radius, double height, double x0 = 0)
        {
            var plane = CreateOffsetPlaneYOZ(x0);
            var sketch = CreateSketchOnPlane(plane);

            DrawCircle(0, 0, radius);
            FinishSketch(sketch);

            BossExtrusion(sketch, height, Direction_Type.dtMiddlePlane);
        }



        // -------- 3D операции --------

        /// <summary>
        /// Добавление материала: выдавливание по эскизу (обычно окружность/профиль).
        /// </summary>
        public ksEntity BossExtrusion(
            ksEntity sketchEntity,
            double depth,
            Direction_Type direction = Direction_Type.dtNormal)
        {
            EnsureTopPart();

            var bossEntity = (ksEntity?)_topPart!.NewEntity((short)Obj3dType.o3d_bossExtrusion);
            if (bossEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_bossExtrusion.");
            }

            var bossDef = (ksBossExtrusionDefinition)bossEntity.GetDefinition();
            var extParam = (ksExtrusionParam)bossDef.ExtrusionParam();

            bossDef.SetSketch(sketchEntity);

            extParam.direction = (short)direction;
            extParam.typeNormal = (short)End_Type.etBlind;
            extParam.depthNormal = Math.Abs(depth);

            bossEntity.Create();
            return bossEntity;
        }

        /// <summary>
        /// Вырез: выдавливание по эскизу на заданную глубину.
        /// </summary>
        public ksEntity CutExtrusion(ksEntity sketchEntity, double depth, bool forward = true)
        {
            EnsureTopPart();

            var cutEntity = (ksEntity?)_topPart!.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            if (cutEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_cutExtrusion.");
            }

            var cutDef = (ksCutExtrusionDefinition)cutEntity.GetDefinition();

            cutDef.cut = true;
            cutDef.directionType = (short)(forward ? Direction_Type.dtNormal : Direction_Type.dtReverse);

            bool side1 = forward;

            cutDef.SetSideParam(
                side1,
                (short)End_Type.etBlind,
                Math.Abs(depth),
                0.0,
                false);

            cutDef.SetSketch(sketchEntity);

            cutEntity.Create();
            return cutEntity;
        }

        public void UpdateModel()
        {
            _doc3D?.RebuildDocument();
        }

        public void SaveAs(string filePath)
        {
            if (_doc3D == null)
            {
                throw new InvalidOperationException("Нет активного 3D-документа для сохранения.");
            }

            _doc3D.SaveAs(filePath);
        }

        public void CloseActiveDocument()
        {
            if (_doc3D == null)
            {
                return;
            }

            try
            {
                _doc3D.close();
            }
            catch
            {
                // Игнорируем: COM может упасть, если документ уже закрыт
            }
            finally
            {
                ReleaseCom(_current2dDoc);
                ReleaseCom(_topPart);
                ReleaseCom(_doc3D);

                _current2dDoc = null;
                _topPart = null;
                _doc3D = null;
            }
        }

        public void Dispose()
        {
            CloseActiveDocument();
            ReleaseCom(_kompas);
            _kompas = null;
        }

        private void EnsureKompas()
        {
            if (_kompas == null)
            {
                throw new InvalidOperationException("KOMPAS не инициализирован. Вызови AttachOrRunCad().");
            }
        }

        private void EnsureTopPart()
        {
            if (_topPart == null)
            {
                throw new InvalidOperationException("Документ/деталь не инициализированы. Вызови CreateDocument3D().");
            }
        }

        private static void ReleaseCom(object? com)
        {
            if (com == null)
            {
                return;
            }

            try
            {
                if (Marshal.IsComObject(com))
                {
                    Marshal.FinalReleaseComObject(com);
                }
            }
            catch
            {
                // игнор
            }
        }
    }
}
