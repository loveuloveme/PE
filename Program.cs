using System;
using System.Collections.Generic;
using System.IO;

namespace PE{
    static class Util{
        private static string Reverse(string s){
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int ParseNum(System.Byte[] bytes){
            List<string> bytesStr = new List<string>();

            foreach(var byteItem in bytes){
                bytesStr.Add(byteItem.ToString("x") == "0" ? "00" : byteItem.ToString("x"));
            }

            bytesStr.Reverse();

            string result = "";
            
            foreach(var byteItem in bytesStr){
                result += byteItem;
            }

            return int.Parse(result, System.Globalization.NumberStyles.HexNumber);
        }

        public static string ParseString(System.Byte[] data){
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }

    public class Byte{
        public uint offset;
        public byte[] data;
        public int byteCount;

        public Byte(int byteCount, uint offset){
            this.offset = offset;
            this.byteCount = byteCount;
            this.data = new byte[this.byteCount];
        }

        public void read(Stream fStream, long baseOffset = 0){
            fStream.Seek((int)(baseOffset+offset), SeekOrigin.Begin);

            for(var i = 0; i < this.byteCount; i++){
                data[i] = (byte)fStream.ReadByte();
            }
            //fStream.Read(this.data, (int)(baseOffset+offset), this.byteCount);
        }
    }


    class DosHeader{
        public Byte e_magic = new Byte(1, 0x00);
        public Byte e_lfanew = new Byte(4, 0x3c);

        public DosHeader(Stream fStream){
            e_magic.read(fStream, 0);
            e_lfanew.read(fStream, 0);
        }

        public int GetLF() => Util.ParseNum(e_lfanew.data);
    }

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
    }

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
    }
    
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

    class PE{ 
        DosHeader dosheader;
        PEHeader peheader;
        List<SectionHeader> sectionHeaders = new List<SectionHeader>();
        ExportTable exportTable;

        private int RVAToOffset(int rva){
            
            int curSect = 0;

            for(int i = 0; i < peheader.fileHeader.GetNumberOfSections(); i++){
                if(sectionHeaders[curSect].GetVirtualAddress() <= rva){
                    if((sectionHeaders[curSect].GetVirtualAddress() + sectionHeaders[curSect].GetVirtualSize()) > rva){
                        rva -= sectionHeaders[curSect].GetVirtualAddress();
                        rva += sectionHeaders[curSect].GetPointerToRawData();

                        return rva;
                    }
                }

                curSect += 1;
            }

            return -1;
        }

        public PE(string fileName){
            Stream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            dosheader = new DosHeader(file);
            peheader = new PEHeader(file, dosheader.GetLF());

            for(int i = 0; i < peheader.fileHeader.GetNumberOfSections(); i++){
                sectionHeaders.Add(new SectionHeader(file, peheader.optionalHeader.GetLast()+40*i));
            }

            Console.WriteLine(RVAToOffset(peheader.optionalHeader.DataDirectory[1].GetVirtualAddress()).ToString("x"));
        }
    }


    class Program{
        static void Main(string[] args) {
            //new PE("7z1900-x64.exe");
            //new PE("twain_32.dll");
            new PE("7z.dll");
        }
    }
}
