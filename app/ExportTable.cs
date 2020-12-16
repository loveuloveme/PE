using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace PE{
    class ExportTable{
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

        public List<string> names = new List<string>();

        private void GetNames(Stream fStream){
            var res = new List<string>();

            List<byte> byteName = new List<byte>();

            while(true){
                byteName.Add((byte)fStream.ReadByte());

                if((int)byteName[byteName.Count - 1] == 0){
                    res.Add(System.Text.Encoding.ASCII.GetString(byteName.ToArray()));
                    byteName = new List<byte>();
                }

                if(res.Count == this.GetNumberOfNames()) break;
            }

            names = res;
        }

        public ExportTable(Stream fStream, long offset){

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

            this.GetNames(fStream);
        }

        public int GetName() => Util.RVAToOffset(Util.ParseNum(Name.data));
        public int GetNumberOfFunctions() => Util.ParseNum(NumberOfFunctions.data);
        public int GetNumberOfNames() => Util.ParseNum(NumberOfNames.data);
        public int GetAddressOfFunctions() => Util.RVAToOffset(Util.ParseNum(AddressOfFunctions.data));
        public int GetAddressOfNames() => Util.RVAToOffset(Util.ParseNum(AddressOfNames.data));
        public int GetAddressOfNameOrdinals() => Util.RVAToOffset(Util.ParseNum(AddressOfNameOrdinals.data));
        public string GetTableName() => Util.ParseString(TableName.data);
    }
}
