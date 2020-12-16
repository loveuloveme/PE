using System.IO;

namespace PE{
    class SectionHeader{
        //long offset;

        public Byte Name = new Byte(8, 0x0);
        
        //??
        public Byte PhysicalAddress = new Byte(4, 0x0 + 8);
        //??

        public Byte VirtualSize = new Byte(4, 0x0 + 8);
        public Byte VirtualAddress = new Byte(4, 0x0 + 12);
        public Byte SizeOfRawData = new Byte(4, 0x0 + 16);
        public Byte PointerToRawData = new Byte(4, 0x0 + 20);

        //??
        public Byte PointerToRelocations = new Byte(4, 0x0 + 28);
        public Byte PointerToLinenumbers = new Byte(4, 0x0 + 32);
        public Byte NumberOfRelocations = new Byte(2, 0x0 + 36);
        public Byte NumberOfLinenumbers = new Byte(2, 0x0 + 38);
        //??

        public Byte Characteristics = new Byte(4, 0x0 + 36);

        public SectionHeader(Stream fStream, long offset){
            Name.read(fStream, offset);
            PhysicalAddress.read(fStream, offset);
            VirtualSize.read(fStream, offset);
            VirtualAddress.read(fStream, offset);
            SizeOfRawData.read(fStream, offset);
            PointerToRawData.read(fStream, offset);
            PointerToRelocations.read(fStream, offset);
            PointerToLinenumbers.read(fStream, offset);
            NumberOfRelocations.read(fStream, offset);
            NumberOfLinenumbers.read(fStream, offset);
            Characteristics.read(fStream, offset);
        }

        public string GetName() => Util.ParseString(Name.data);
        public int GetVirtualAddress() => Util.ParseNum(this.VirtualAddress.data);
        public int GetVirtualSize() => Util.ParseNum(this.VirtualSize.data);
        public int GetPointerToRawData() => Util.ParseNum(PointerToRawData.data);
        public int GetSizeOfRawData() => Util.ParseNum(SizeOfRawData.data);
        public int GetCharacteristics() => Util.ParseNum(Characteristics.data);
    }
}
