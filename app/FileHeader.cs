using System.IO;
using System.Collections.Generic;

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

        public string byteName = "";

        public FileHeader(Stream fStream, long offset){
            Machine.read(fStream, offset);
            NumberOfSections.read(fStream, offset);
            TimeDateStamp.read(fStream, offset);
            PointerToSymbolTable.read(fStream, offset);
            NumberOfSymbols.read(fStream, offset);
            SizeOfOptionalHeader.read(fStream, offset);
            Characteristics.read(fStream, offset);

            for(int i = 0; i < TimeDateStamp.byteCount; i++){    
                byteName += Util.Reverse(System.Convert.ToString(fStream.ReadByte(), 2).PadLeft(8, '0'));
            }

            //byteName += "1";

            //byteName = Util.Reverse(byteName);

            System.Console.WriteLine(byteName);
        }

        public int GetMachine() => Util.ParseNum(Machine.data);
        public int GetNumberOfSections() => Util.ParseNum(NumberOfSections.data);

        public long GetTimeDateStamp(){
            return System.Convert.ToUInt32(byteName, 2);
        }

        public List<string> ParseCharacteristics(){
            var chars = new List<string>();

            var byteMap = Util.Reverse(System.Convert.ToString(GetCharacteristics(), 2).PadLeft(16, '0'));

            if(byteMap[0] == '1') chars.Add("IMAGE_FILE_RELOCS_STRIPPED");
            if(byteMap[1] == '1') chars.Add("IMAGE_FILE_EXECUTABLE_IMAGE");
            if(byteMap[2] == '1') chars.Add("IMAGE_FILE_LINE_NUMS_STRIPPED");
            if(byteMap[3] == '1') chars.Add("IMAGE_FILE_LOCAL_SYMS_STRIPPED");
            if(byteMap[4] == '1') chars.Add("IMAGE_FILE_AGGRESSIVE_WS_TRIM");
            if(byteMap[5] == '1') chars.Add("IMAGE_FILE_LARGE_ADDRESS_AWARE");
            if(byteMap[7] == '1') chars.Add("IMAGE_FILE_BYTES_REVERSED_LO");
            if(byteMap[8] == '1') chars.Add("IMAGE_FILE_32BIT_MACHINE");
            if(byteMap[9] == '1') chars.Add("IMAGE_FILE_DEBUG_STRIPPED");
            if(byteMap[10] == '1') chars.Add("IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP");
            if(byteMap[11] == '1') chars.Add("IMAGE_FILE_NET_RUN_FROM_SWAP");
            if(byteMap[12] == '1') chars.Add("IMAGE_FILE_SYSTEM");
            if(byteMap[13] == '1') chars.Add("IMAGE_FILE_DLL");
            if(byteMap[14] == '1') chars.Add("IMAGE_FILE_UP_SYSTEM_ONLY");
            if(byteMap[15] == '1') chars.Add("IMAGE_FILE_BYTES_REVERSED_HI");

            return chars;
        }

        public int GetPointerToSymbolTable() => Util.ParseNum(PointerToSymbolTable.data);
        public int GetNumberOfSymbols() => Util.ParseNum(NumberOfSymbols.data);
        public int GetSizeOfOptionalHeader() => Util.ParseNum(SizeOfOptionalHeader.data);
        public int GetCharacteristics() => Util.ParseNum(Characteristics.data);
    }
}
