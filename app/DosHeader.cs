using System.IO;

namespace PE{
        class DosHeader{
        public Byte e_magic = new Byte(2, 0x00);
        public Byte e_lfanew = new Byte(4, 0x3c);

        public DosHeader(Stream fStream){
            e_magic.read(fStream, 0);
            e_lfanew.read(fStream, 0);
        }

        public string GetMagic() => Util.ParseString(e_magic.data);
        public int GetLF() => Util.ParseNum(e_lfanew.data);
    }
}
