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

        public int GetMagic() => Util.ParseNum(this.Magic.data);
        public int GetMajorLinkerVersion() => Util.ParseNum(this.MajorLinkerVersion.data);
        public int GetMinorLinkerVersion() => Util.ParseNum(this.MinorLinkerVersion.data);
        public int GetSizeOfCode() => Util.ParseNum(this.SizeOfCode.data);
        public int GetSizeOfInitializedData() => Util.ParseNum(this.SizeOfInitializedData.data);
        public int GetSizeOfUninitializedData() => Util.ParseNum(this.SizeOfUninitializedData.data);
        public int GetAddressOfEntryPoint() => Util.ParseNum(this.AddressOfEntryPoint.data);
        public int GetSectionAlignment() => Util.ParseNum(this.SectionAlignment.data);
        public int GetFileAlignment() => Util.ParseNum(this.FileAlignment.data);
        public int GetMajorOperatingSystemVersion() => Util.ParseNum(this.MajorOperatingSystemVersion.data);
        public int GetMinorOperatingSystemVersion() => Util.ParseNum(this.MinorOperatingSystemVersion.data);
        public int GetMajorImageVersion() => Util.ParseNum(this.MajorImageVersion.data);
        public int GetMajorSubsystemVersion() => Util.ParseNum(this.MajorSubsystemVersion.data);
        public int GetMinorSubsystemVersion() => Util.ParseNum(this.MinorSubsystemVersion.data);
        public int GetMinorImageVersion() => Util.ParseNum(this.MinorImageVersion.data);
        public int GetWin32VersionValue() => Util.ParseNum(this.Win32VersionValue.data);
        public int GetSizeOfImage() => Util.ParseNum(this.SizeOfImage.data);
        public int GetCheckSum() => Util.ParseNum(this.CheckSum.data);
        public int GetSubsystem() => Util.ParseNum(this.Subsystem.data);
        public int GetDllCharacteristics() => Util.ParseNum(this.DllCharacteristics.data);
        public int GetSizeOfStackReserve() => Util.ParseNum(this.SizeOfStackReserve.data);
        public int GetSizeOfStackCommit() => Util.ParseNum(this.SizeOfStackCommit.data);
        public int GetSizeOfHeapReserve() => Util.ParseNum(this.SizeOfHeapReserve.data);
        public int GetSizeOfHeapCommit() => Util.ParseNum(this.SizeOfHeapCommit.data);
        public int GetLoaderFlags() => Util.ParseNum(this.LoaderFlags.data);
        public int GetNumberOfRvaAndSizes() => Util.ParseNum(this.NumberOfRvaAndSizes.data);
        public int GetBaseOfCode() => Util.ParseNum(this.BaseOfCode.data);

        public abstract int GetLast();
    }
}