using System.IO;

namespace PE{
    class FileHeader{
        long offset;

        public Byte Machine = new Byte(2, 0x0);
        public Byte NumberOfSections = new Byte(2, 0x0 + 2);
        public Byte TimeDateStamp = new Byte(4, 0x0 + 4);
        public Byte PointerToSymbolTable = new Byte(4, 0x0 + 8);
        public Byte NumberOfSymbols = new Byte(4, 0x0 + 12);
        public Byte SizeOfOptionalHeader = new Byte(2, 0x0 + 16);
        public Byte Characteristics = new Byte(2, 0x0 + 18);

        public FileHeader(Stream fStream, long offset){
            Machine.read(fStream, offset);
            NumberOfSections.read(fStream, offset);
            TimeDateStamp.read(fStream, offset);
            PointerToSymbolTable.read(fStream, offset);
            NumberOfSymbols.read(fStream, offset);
            SizeOfOptionalHeader.read(fStream, offset);
            Characteristics.read(fStream, offset);
        }

        public int GetMachine() => Util.ParseNum(Machine.data);
        public int GetNumberOfSections() => Util.ParseNum(NumberOfSections.data);
        public int GetTimeDateStamp() => Util.ParseNum(TimeDateStamp.data);
        public int GetPointerToSymbolTable() => Util.ParseNum(PointerToSymbolTable.data);
        public int GetNumberOfSymbols() => Util.ParseNum(NumberOfSymbols.data);
        public int GetSizeOfOptionalHeader() => Util.ParseNum(SizeOfOptionalHeader.data);
        public int GetCharacteristics() => Util.ParseNum(Characteristics.data);
    }
}
