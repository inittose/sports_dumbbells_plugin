using Kompas6API5;
using Kompas6Constants3D;
using System.Runtime.InteropServices;

namespace SportsDumbbellsPlugin.Wrapper
{
    //TODO: refactor

    /// <summary>
    /// Обёртка над KOMPAS-3D API5. Инкапсулирует подключение к КОМПАС,
    /// создание 3D-документа, работу с эскизами и базовыми 3D-операциями.
    /// </summary>
    public class Wrapper : IDisposable
    {
        /// <summary>
        /// Экземпляр приложения KOMPAS.
        /// </summary>
        private KompasObject? _kompas;

        /// <summary>
        /// Текущий 3D-документ KOMPAS.
        /// </summary>
        private ksDocument3D? _document3D;

        /// <summary>
        /// Верхняя деталь (Part) текущего 3D-документа.
        /// </summary>
        private ksPart? _topPart;

        /// <summary>
        /// Текущий 2D-документ (редактор эскиза), если эскиз находится в режиме редактирования.
        /// </summary>
        private ksDocument2D? _currentSketchDocument2D;

        /// <summary>
        /// Подключается к КОМПАС (создаёт экземпляр приложения) и активирует API контроллера.
        /// </summary>
        /// <param name="visible">Признак видимости окна КОМПАС.</param>
        public void AttachOrRunCad(bool visible = true)
        {
            if (_kompas != null)
            {
                return;
            }

            var kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
            if (kompasType == null)
            {
                throw new InvalidOperationException("Не найден ProgID KOMPAS.Application.5.");
            }

            _kompas = (KompasObject?)Activator.CreateInstance(kompasType);
            if (_kompas == null)
            {
                throw new InvalidOperationException("Не удалось создать KompasObject.");
            }

            _kompas.Visible = visible;
            _kompas.ActivateControllerAPI();
        }

        /// <summary>
        /// Создаёт новый 3D-документ и получает верхнюю деталь (pTop_Part).
        /// При наличии открытого документа закрывает его.
        /// </summary>
        /// <param name="invisible">Не используется, оставлено для совместимости сигнатуры.</param>
        public void CreateDocument3D(bool invisible = false)
        {
            EnsureKompas();

            CloseActiveDocument();

            _document3D = (ksDocument3D?)_kompas!.Document3D();
            if (_document3D == null)
            {
                throw new InvalidOperationException("Не удалось получить ksDocument3D.");
            }

            _document3D.Create();

            _topPart = (ksPart?)_document3D.GetPart((short)Part_Type.pTop_Part);
            if (_topPart == null)
            {
                throw new InvalidOperationException(
                    "Не удалось получить верхнюю деталь (pTop_Part).");
            }
        }

        /// <summary>
        /// Создаёт эскиз на переданной сущности плоскости и переводит его в режим редактирования.
        /// </summary>
        /// <param name="planeEntity">Сущность плоскости (например, базовая или смещённая).</param>
        /// <returns>Сущность эскиза.</returns>
        public ksEntity CreateSketchOnPlane(ksEntity planeEntity)
        {
            EnsureTopPart();

            var sketchEntity = (ksEntity?)_topPart!.NewEntity((short)Obj3dType.o3d_sketch);
            if (sketchEntity == null)
            {
                throw new InvalidOperationException(
                    "Не удалось создать сущность эскиза (o3d_sketch).");
            }

            var sketchDefinition = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDefinition.SetPlane(planeEntity);
            sketchEntity.Create();

            _currentSketchDocument2D = (ksDocument2D?)sketchDefinition.BeginEdit();
            if (_currentSketchDocument2D == null)
            {
                throw new InvalidOperationException(
                    "Не удалось войти в редактирование эскиза (BeginEdit).");
            }

            return sketchEntity;
        }

        /// <summary>
        /// Завершает редактирование эскиза.
        /// </summary>
        /// <param name="sketchEntity">Сущность эскиза.</param>
        public void FinishSketch(ksEntity sketchEntity)
        {
            var sketchDefinition = (ksSketchDefinition)sketchEntity.GetDefinition();
            sketchDefinition.EndEdit();

            _currentSketchDocument2D = null;
        }

        /// <summary>
        /// Рисует окружность в текущем 2D-эскизе.
        /// </summary>
        /// <param name="centerX">X координата центра.</param>
        /// <param name="centerY">Y координата центра.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <param name="style">Стиль линии (по умолчанию 1).</param>
        public void DrawCircle(double centerX, double centerY, double radius, int style = 1)
        {
            if (_currentSketchDocument2D == null)
            {
                throw new InvalidOperationException(
                    "Нет активного 2D-эскиза. Сначала вызови CreateSketchOnPlane().");
            }

            _currentSketchDocument2D.ksCircle(centerX, centerY, radius, style);
        }

        /// <summary>
        /// Создаёт смещённую плоскость относительно базовой плоскости YOZ.
        /// Используется для построения геометрии со смещением вдоль оси X.
        /// </summary>
        /// <param name="offsetX">Смещение вдоль оси X.</param>
        /// <returns>Сущность смещённой плоскости.</returns>
        public ksEntity CreateOffsetPlaneYOZ(double offsetX)
        {
            EnsureTopPart();

            var basePlaneEntity =
                (ksEntity?)_topPart!.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);

            if (basePlaneEntity == null)
            {
                throw new InvalidOperationException("Не удалось получить базовую плоскость YOZ.");
            }

            var planeEntity = (ksEntity?)_topPart.NewEntity((short)Obj3dType.o3d_planeOffset);
            if (planeEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_planeOffset.");
            }

            var planeDefinition = (ksPlaneOffsetDefinition)planeEntity.GetDefinition();
            planeDefinition.SetPlane(basePlaneEntity);
            planeDefinition.offset = offsetX;

            planeEntity.Create();
            return planeEntity;
        }

        /// <summary>
        /// Строит диск на плоскости YOZ с заданным смещением по оси X.
        /// Диск создаётся как выдавливание кольцевого профиля (две окружности в одном эскизе).
        /// </summary>
        /// <param name="outerRadius">Внешний радиус диска.</param>
        /// <param name="holeRadius">Радиус отверстия диска.</param>
        /// <param name="thickness">Толщина диска.</param>
        /// <param name="offsetX">Смещение плоскости построения по оси X.</param>
        /// <param name="direction">Направление выдавливания.</param>
        public void BuildDiskAtX(
            double outerRadius,
            double holeRadius,
            double thickness,
            double offsetX = 0,
            Direction_Type direction = Direction_Type.dtNormal)
        {
            var planeEntity = CreateOffsetPlaneYOZ(offsetX);
            var sketchEntity = CreateSketchOnPlane(planeEntity);

            DrawCircle(0, 0, outerRadius);
            DrawCircle(0, 0, holeRadius);

            FinishSketch(sketchEntity);

            BossExtrusion(sketchEntity, thickness, direction);
        }

        /// <summary>
        /// Строит цилиндр с осью вдоль оси X, используя плоскость YOZ со смещением.
        /// Выдавливание выполняется симметрично относительно плоскости (dtMiddlePlane).
        /// </summary>
        /// <param name="radius">Радиус цилиндра.</param>
        /// <param name="height">Длина цилиндра вдоль оси X.</param>
        /// <param name="offsetX">Смещение плоскости YOZ по оси X.</param>
        public void BuildCylinderAtX(double radius, double height, double offsetX = 0)
        {
            var planeEntity = CreateOffsetPlaneYOZ(offsetX);
            var sketchEntity = CreateSketchOnPlane(planeEntity);

            DrawCircle(0, 0, radius);
            FinishSketch(sketchEntity);

            BossExtrusion(sketchEntity, height, Direction_Type.dtMiddlePlane);
        }

        /// <summary>
        /// Создаёт операцию выдавливания (добавление материала) по эскизу.
        /// </summary>
        /// <param name="sketchEntity">Сущность эскиза.</param>
        /// <param name="depth">Глубина выдавливания.</param>
        /// <param name="direction">Направление выдавливания.</param>
        /// <returns>Сущность операции выдавливания.</returns>
        public ksEntity BossExtrusion(
            ksEntity sketchEntity,
            double depth,
            Direction_Type direction = Direction_Type.dtNormal)
        {
            EnsureTopPart();

            var extrusionEntity = (ksEntity?)_topPart!.NewEntity(
                (short)Obj3dType.o3d_bossExtrusion);

            if (extrusionEntity == null)
            {
                throw new InvalidOperationException("Не удалось создать o3d_bossExtrusion.");
            }

            var extrusionDefinition = (ksBossExtrusionDefinition)extrusionEntity.GetDefinition();
            var extrusionParameters = (ksExtrusionParam)extrusionDefinition.ExtrusionParam();

            extrusionDefinition.SetSketch(sketchEntity);

            extrusionParameters.direction = (short)direction;
            extrusionParameters.typeNormal = (short)End_Type.etBlind;
            extrusionParameters.depthNormal = Math.Abs(depth);

            extrusionEntity.Create();
            return extrusionEntity;
        }

        /// <summary>
        /// Перестраивает 3D-документ.
        /// </summary>
        public void UpdateModel()
        {
            _document3D?.RebuildDocument();
        }

        /// <summary>
        /// Сохраняет 3D-документ в файл.
        /// </summary>
        /// <param name="filePath">Путь сохранения файла.</param>
        public void SaveAs(string filePath)
        {
            if (_document3D == null)
            {
                throw new InvalidOperationException("Нет активного 3D-документа для сохранения.");
            }

            _document3D.SaveAs(filePath);
        }

        /// <summary>
        /// Закрывает активный 3D-документ и освобождает связанные COM-объекты.
        /// </summary>
        public void CloseActiveDocument()
        {
            if (_document3D == null)
            {
                return;
            }

            try
            {
                _document3D.close();
            }
            catch
            {
                // Игнорируем исключения: COM может выбросить ошибку, если документ уже закрыт.
            }
            finally
            {
                ReleaseCom(_currentSketchDocument2D);
                ReleaseCom(_topPart);
                ReleaseCom(_document3D);

                _currentSketchDocument2D = null;
                _topPart = null;
                _document3D = null;
            }
        }

        /// <summary>
        /// Освобождает ресурсы: закрывает документ и освобождает COM-объект КОМПАС.
        /// </summary>
        public void Dispose()
        {
            CloseActiveDocument();

            ReleaseCom(_kompas);
            _kompas = null;
        }

        /// <summary>
        /// Проверяет, что объект КОМПАС инициализирован.
        /// </summary>
        private void EnsureKompas()
        {
            if (_kompas == null)
            {
                throw new InvalidOperationException(
                    "KOMPAS не инициализирован. Вызови AttachOrRunCad().");
            }
        }

        /// <summary>
        /// Проверяет, что верхняя деталь 3D-документа инициализирована.
        /// </summary>
        private void EnsureTopPart()
        {
            if (_topPart == null)
            {
                throw new InvalidOperationException(
                    "Документ/деталь не инициализированы. Вызови CreateDocument3D().");
            }
        }

        /// <summary>
        /// Освобождает COM-объект, если он является COM-объектом.
        /// </summary>
        /// <param name="comObject">Освобождаемый объект.</param>
        private static void ReleaseCom(object? comObject)
        {
            if (comObject == null)
            {
                return;
            }

            try
            {
                if (Marshal.IsComObject(comObject))
                {
                    Marshal.FinalReleaseComObject(comObject);
                }
            }
            catch
            {
                //TODO: ??
                // Игнорируем исключения при освобождении COM.
            }
        }
    }
}
