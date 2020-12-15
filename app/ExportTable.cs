using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PE{
    class ExportTable{
        int _offset;

        public Byte Characteristics = new Byte(4, 0x0);
        public Byte TimeDateStamp = new Byte(4, 0x0 + 4);
        public Byte MajorVersion = new Byte(2, 0x0 + 8);
        public Byte MinorVersion = new Byte(2, 0x0 + 10);
        public Byte Name = new Byte(4, 0x0 + 12);
        public Byte Base = new Byte(4, 0x0 + 16);
        public Byte NumberOfFunctions = new Byte(4, 0x0 + 20);
        public Byte NumberOfNames = new Byte(4, 0x0 + 24);
        public Byte AddressOfFunctions = new Byte(4, 0x0 + 28);
        public Byte AddressOfNames = new Byte(4, 0x0 + 32);
        public Byte AddressOfNameOrdinals = new Byte(4, 0x0 + 36);

        public Byte TableName;

        public ExportTable(Stream fStream, long offset, int sectOffset){
            _offset = sectOffset;

            offset -= sectOffset;

            Characteristics.read(fStream, offset);
            TimeDateStamp.read(fStream, offset);
            MajorVersion.read(fStream, offset);
            MinorVersion.read(fStream, offset);
            Name.read(fStream, offset);
            Base.read(fStream, offset);
            NumberOfFunctions.read(fStream, offset);
            NumberOfNames.read(fStream, offset);
            AddressOfFunctions.read(fStream, offset); 
            AddressOfNames.read(fStream, offset); 
            AddressOfNameOrdinals.read(fStream, offset);
            
            TableName = new Byte(2, (uint)(this.GetName()));
            TableName.read(fStream, 0);
        }

        public int GetName() => Util.ParseNum(Name.data) - _offset;
        public int GetNumberOfFunctions() => Util.ParseNum(NumberOfFunctions.data);
        public int GetNumberOfNames() => Util.ParseNum(NumberOfNames.data);
        public int GetAddressOfFunctions() => Util.ParseNum(AddressOfFunctions.data) - _offset;
        public int GetAddressOfNames() => Util.ParseNum(AddressOfNames.data) - _offset;
        public string GetTableName() => Util.ParseString(TableName.data);
    }
}
