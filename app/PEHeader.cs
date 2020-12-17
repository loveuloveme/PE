using System.IO;

namespace PE{
    class PEHeader{
        //long offset;

        public Byte signature = new Byte(4, 0x0);
        public FileHeader fileHeader;
        public OpHeader optionalHeader;
        public bool is32plus = false;

        public PEHeader(Stream fStream, long offset){
            signature.read(fStream, offset);
            fileHeader = new FileHeader(fStream, offset + 4);

            
            Byte Magic = new Byte(2, (uint)(offset + 24));
            Magic.read(fStream);

            if(Util.ParseNum(Magic.data) == 523){
                is32plus = true;
                optionalHeader = new OptionalHeader64(fStream, offset + 24);
            }else{
                optionalHeader = new OptionalHeader(fStream, offset + 24);
            }
        }
    } 
}
