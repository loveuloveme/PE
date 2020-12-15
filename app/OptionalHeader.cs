namespace PE{
    abstract class OpHeader{
        protected long offset_;

        public Byte Magic;
        public Byte MajorLinkerVersion;
        public Byte MinorLinkerVersion;
        public Byte SizeOfCode;
        public Byte SizeOfInitializedData;
        public Byte SizeOfUninitializedData;
        public Byte AddressOfEntryPoint;
        public Byte BaseOfCode;
        public Byte ImageBase;
        public Byte SectionAlignment;
        public Byte FileAlignment;
        public Byte MajorOperatingSystemVersion;
        public Byte MinorOperatingSystemVersion;
        public Byte MajorImageVersion;
        public Byte MinorImageVersion;
        public Byte MajorSubsystemVersion;
        public Byte MinorSubsystemVersion;
        public Byte Win32VersionValue;
        public Byte SizeOfImage;
        public Byte SizeOfHeaders;
        public Byte CheckSum;
        public Byte Subsystem;
        public Byte DllCharacteristics;
        public Byte SizeOfStackReserve;
        public Byte SizeOfStackCommit;
        public Byte SizeOfHeapReserve;
        public Byte SizeOfHeapCommit;
        public Byte LoaderFlags;
        public Byte NumberOfRvaAndSizes;

        public ImageDataDir[] DataDirectory = new ImageDataDir[16];

        public int GetImageBase() => Util.ParseNum(this.ImageBase.data);
        public int GetSizeOfHeaders() => Util.ParseNum(this.SizeOfHeaders.data);

        public abstract int GetLast();
    }
}