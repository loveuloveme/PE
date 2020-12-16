using System.IO;
using System.Collections.Generic;

namespace PE{
    class ImportTable{

        // typedef struct _IMAGE_IMPORT_DESCRIPTOR {
        //     DWORD   OriginalFirstThunk;
        //     DWORD   TimeDateStamp;
        //     DWORD   ForwarderChain;
        //     DWORD   Name;
        //     DWORD   FirstThunk;
        // } IMAGE_IMPORT_DESCRIPTOR, *PIMAGE_IMPORT_DESCRIPTOR;
        
        // typedef struct _IMAGE_THUNK_DATA32 {
        //     union {
        //         DWORD ForwarderString;
        //         DWORD Function;
        //         DWORD Ordinal;
        //         DWORD AddressOfData;
        //     } u1;
        // } IMAGE_THUNK_DATA32,*PIMAGE_THUNK_DATA32;

        // typedef struct _IMAGE_IMPORT_BY_NAME {
        //     WORD    Hint;
        //     BYTE    Name[1];
        // } IMAGE_IMPORT_BY_NAME, *PIMAGE_IMPORT_BY_NAME;

        public class ImportByName{
            public Byte Hint;
        }

        public abstract class ImageThunk{
            public Byte AddressOfData;

            //public int GetAddressOfData() => Util.ParseNum(AddressOfData.data);
            public int GetAddressOfData(){
                var str = System.Convert.ToString(Util.ParseNum(AddressOfData.data), 2);//(System.Convert.ToString(AddressOfData.data[0], 2)+System.Convert.ToString(AddressOfData.data[1], 2)+System.Convert.ToString(AddressOfData.data[2], 2)+System.Convert.ToString(AddressOfData.data[3], 2)).PadLeft(32, '0').Substring(1);
                if(isNum()){
                    return System.Convert.ToInt32(str, 2);    
                }else{
                    return Util.RVAToOffset(System.Convert.ToInt32(str, 2));
                }
            }

            public bool isNum(){
                if(System.Convert.ToString(AddressOfData.data[0], 2).PadLeft(8, '0')[0] == '1'){
                    return true;
                }else{
                    return false;
                }
            }
        }

        public class ImageThunk32:ImageThunk{
            public ImageThunk32(Stream fStream, long offset){
                AddressOfData = new Byte(4, 0x0);

                AddressOfData.read(fStream, offset);
            }
        }

        public class ImageThunk64:ImageThunk{
            public ImageThunk64(Stream fStream, long offset){
                AddressOfData = new Byte(8, 0x0);

                AddressOfData.read(fStream, offset);
            }
        }

        public class ImportDescriptor{
            public Byte OriginalFirstThunk = new Byte(4, 0x0); 
            public Byte TimeDateStamp = new Byte(4, 0x0 + 4); 
            public Byte ForwarderChain = new Byte(4, 0x0 + 8); 
            public Byte Name = new Byte(4, 0x0 + 12); 
            public Byte FirstThunk = new Byte(4, 0x0 + 16); 

            public ImageThunk OriginalThunk;
            public ImageThunk Thunk;

            public string ImportName;

            public ImportDescriptor(Stream fStream, long offset){
                OriginalFirstThunk.read(fStream, offset);
                TimeDateStamp.read(fStream, offset);
                ForwarderChain.read(fStream, offset);

                Name.read(fStream, offset);

                FirstThunk.read(fStream, offset);

                if(GetName() == -1) return;

                var nameOffset = GetName();
                fStream.Seek(nameOffset, SeekOrigin.Begin);
                
                List<byte> byteName = new List<byte>();

                while(true){
                    byteName.Add((byte)fStream.ReadByte());
                    if((int)byteName[byteName.Count - 1] == 0) break;
                }

                ImportName = System.Text.Encoding.ASCII.GetString(byteName.ToArray());
                
                if(GetOriginalFirstThunk() != -1){
                    OriginalThunk = new ImageThunk32(fStream, GetOriginalFirstThunk());
                }


                if(GetFirstThunk() != -1){
                    Thunk = new ImageThunk32(fStream, GetFirstThunk());
                }

                //System.Console.WriteLine(Thunk.GetAddressOfData());
            }

            public bool isNull(){
                if(OriginalFirstThunk.data[0] == 0 && OriginalFirstThunk.data[1] == 0 && OriginalFirstThunk.data[2] == 0 && OriginalFirstThunk.data[3] == 0) return true;

                return false;
            }

            public int GetOriginalFirstThunk() => Util.RVAToOffset(Util.ParseNum(OriginalFirstThunk.data));
            public int GetFirstThunk() => Util.RVAToOffset(Util.ParseNum(FirstThunk.data));
            public int GetName() => Util.RVAToOffset(Util.ParseNum(Name.data));
        }

        public List<ImportDescriptor> descriptors = new List<ImportDescriptor>();

        public ImportTable(Stream fStream, long offset){
            while(true){
                descriptors.Add(new ImportDescriptor(fStream, offset + 20 * descriptors.Count));

                if(descriptors[descriptors.Count - 1].isNull()){
                    descriptors.RemoveAt(descriptors.Count - 1);
                    break;
                }
            }
        }
    }
}   