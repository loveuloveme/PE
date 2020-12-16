using System.IO;

namespace PE{
    class PEHeader{
        //long offset;

        public Byte signature = new Byte(4, 0x0);
        public FileHeader fileHeader;
        public OpHeader optionalHeader;

        public PEHeader(Stream fStream, long offset){
            signature.read(fStream, offset);
            fileHeader = new FileHeader(fStream, offset + 4);

            if(fileHeader.GetMachine() == 34404){
                optionalHeader = new OptionalHeader64(fStream, offset + 24);
            }else{
                optionalHeader = new OptionalHeader(fStream, offset + 24);
            }
        }
    } 
}
