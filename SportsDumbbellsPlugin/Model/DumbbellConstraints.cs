namespace SportsDumbbellsPlugin.Model
{
    public static class DumbbellConstraints
    {
        // --- Стержень ---
        public const double CenterLengthMin = 100;
        public const double CenterLengthMax = 200;

        public const double SeatLengthMin = 70;
        public const double SeatLengthMax = 150;

        public const double HandleDiameterMin = 24;
        public const double HandleDiameterMax = 34;

        public const double SeatDiameterMin = 24;
        public const double SeatDiameterMax = 34;

        public const int DisksPerSideMin = 0;
        public const int DisksPerSideMax = 8;

        // --- Диски ---
        public const double DiskOuterDiameterMin = 120;
        public const double DiskOuterDiameterMax = 260;

        public const double DiskHoleDiameterMin = 26;
        public const double DiskHoleDiameterMax = 34;

        public const double DiskThicknessMin = 10;
        public const double DiskThicknessMax = 40;

        // --- Связи ---
        // d отверстия диска = d2 + [0.5; 1.5]
        public const double HoleDiameterOffsetMin = 0.5;
        public const double HoleDiameterOffsetMax = 1.5;

        // Значения по умолчанию.
        public const double DefaultCenterLength = 150;
        public const double DefaultSeatLength = 100;
        public const double DefaultHandleDiameter = 30;
        public const double DefaultSeatDiameter = 28;

        public const double DefaultDiskOuterDiameter = 200;
        public const double DefaultDiskHoleDiameter = 29;
        public const double DefaultDiskThickness = 20;

        public const int DefaultDisksPerSide = 1;
    }
}
