using System.IO;

namespace PE{
    class OptionalHeader:OpHeader{
        public Byte BaseOfData;

        public OptionalHeader(Stream fStream, long offset){
            offset_ = offset;
            
            Magic = new Byte(2, 0x0);
            MajorLinkerVersion = new Byte(1, 0x0 + 2);
            MinorLinkerVersion = new Byte(1, 0x0 + 3);
            SizeOfCode = new Byte(4, 0x0 + 4);
            SizeOfInitializedData = new Byte(4, 0x0 + 8);
            SizeOfUninitializedData = new Byte(4, 0x0 + 12);
            AddressOfEntryPoint = new Byte(4, 0x0 + 16);
            BaseOfCode = new Byte(4, 0x0 + 20);
            BaseOfData = new Byte(4, 0x0 + 24);
            ImageBase = new Byte(4, 0x0 + 28);
            SectionAlignment = new Byte(4, 0x0 + 32);
            FileAlignment = new Byte(4, 0x0 + 36);
            MajorOperatingSystemVersion = new Byte(2, 0x0 + 40);
            MinorOperatingSystemVersion = new Byte(2, 0x0 + 42);
            MajorImageVersion = new Byte(2, 0x0 + 44);
            MinorImageVersion = new Byte(2, 0x0 + 46);
            MajorSubsystemVersion = new Byte(2, 0x0 + 48);
            MinorSubsystemVersion = new Byte(2, 0x0 + 50);
            Win32VersionValue = new Byte(4, 0x0 + 52);
            SizeOfImage = new Byte(4, 0x0 + 56);
            SizeOfHeaders = new Byte(4, 0x0 + 60);
            CheckSum = new Byte(4, 0x0 + 64);
            Subsystem = new Byte(2, 0x0 + 68);
            DllCharacteristics = new Byte(2, 0x0 + 70);
            SizeOfStackReserve = new Byte(4, 0x0 + 72);
            SizeOfStackCommit = new Byte(4, 0x0 + 76);
            SizeOfHeapReserve = new Byte(4, 0x0 + 80);
            SizeOfHeapCommit = new Byte(4, 0x0 + 84);
            LoaderFlags = new Byte(4, 0x0 + 88);
            NumberOfRvaAndSizes = new Byte(4, 0x0 + 92);

            Magic.read(fStream, offset); 
            MajorLinkerVersion.read(fStream, offset);
            MinorLinkerVersion.read(fStream, offset);
            SizeOfCode.read(fStream, offset);
            SizeOfInitializedData.read(fStream, offset);
            SizeOfUninitializedData.read(fStream, offset);
            AddressOfEntryPoint.read(fStream, offset);
            BaseOfCode.read(fStream, offset);
            BaseOfData.read(fStream, offset);
            ImageBase.read(fStream, offset);
            SectionAlignment.read(fStream, offset);
            FileAlignment.read(fStream, offset);
            MajorOperatingSystemVersion.read(fStream, offset);
            MinorOperatingSystemVersion.read(fStream, offset);
            MajorImageVersion.read(fStream, offset);
            MinorImageVersion.read(fStream, offset);
            MajorSubsystemVersion.read(fStream, offset);
            MinorSubsystemVersion.read(fStream, offset);
            Win32VersionValue.read(fStream, offset);
            SizeOfImage.read(fStream, offset);
            SizeOfHeaders.read(fStream, offset);
            CheckSum.read(fStream, offset);
            Subsystem.read(fStream, offset);
            DllCharacteristics.read(fStream, offset);
            SizeOfStackReserve.read(fStream, offset);
            SizeOfStackCommit.read(fStream, offset);
            SizeOfHeapReserve.read(fStream, offset);
            SizeOfHeapCommit.read(fStream, offset);
            LoaderFlags.read(fStream, offset);
            NumberOfRvaAndSizes.read(fStream, offset);

            for(int i = 0; i < 16; i++){
                DataDirectory[i] = new ImageDataDir(fStream, offset+96 + 8*i);
            }
        }

        public int GetImageBase() => Util.ParseNum(this.ImageBase.data);
        public int GetSizeOfHeaders() => Util.ParseNum(this.SizeOfHeaders.data);

        public override int GetLast() => (int)this.offset_+96+128;
    }
}