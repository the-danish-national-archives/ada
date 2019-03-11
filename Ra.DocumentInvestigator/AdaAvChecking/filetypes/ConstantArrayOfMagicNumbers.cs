namespace Ra.DocumentInvestigator.AdaAvChecking.filetypes
{
    public struct ConstantArrayOfMagicNumbers
    {
        public static readonly byte[] TiffBigEndian = {0x4d, 0x4d, 0x00, 0x2a};
        public static readonly byte[] TiffLittleEndian = {0x49, 0x49, 0x2a, 0x00};
        public static readonly byte[] JP2 = {0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A, 0x87, 0x0A}; // see ISO JPEG 2000: Core I.5.1
        public static readonly byte[] Mp3WithID3V2Container = {0x49, 0x44, 0x33};
        public static readonly byte[] Mp3WithoutID3 = {0xFF, 0xFB};
        public static readonly byte[] WAVRiff = {0x52, 0x49, 0x46, 0x46};
        public static readonly byte[] WAVWave = {0x57, 0x41, 0x56, 0x45};
        public static readonly byte[] XMLFourBytes = {0x3c, 0x3f, 0x78, 0x6d};
        public static readonly byte[] XMLTwoBytes = {0x6c, 0x20};
    }
}