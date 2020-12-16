using System.IO;

namespace PE{
    class ImageDataDir{
        public Byte VirtualAddress = new Byte(4, 0x0);
        public Byte Size = new Byte(4, 0x0 + 4);

        public ImageDataDir(Stream fStream, long offset){
            VirtualAddress.read(fStream, offset); 
            Size.read(fStream, offset);
        }

        public int GetVirtualAddress() => Util.ParseNum(this.VirtualAddress.data);
        public int GetSize() => Util.ParseNum(this.Size.data);
    }
}
