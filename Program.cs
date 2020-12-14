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

    class PE{
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
            public Byte e_lfanew = new Byte(1, 0x3c);

            public DosHeader(Stream fStream){
                e_magic.read(fStream, 0);
                e_lfanew.read(fStream, 0);
            }
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

        class OptionalHeader{
            // IMAGE_DATA_DIRECTORY DataDirectory[IMAGE_NUMBEROF_DIRECTORY_ENTRIES];

            public Byte Magic = new Byte(2, 0x0);
            public Byte MajorLinkerVersion = new Byte(1, 0x0 + 2);
            public Byte MinorLinkerVersion = new Byte(1, 0x0 + 3);
            public Byte SizeOfCode = new Byte(4, 0x0 + 4);
            public Byte SizeOfInitializedData = new Byte(4, 0x0 + 8);
            public Byte SizeOfUninitializedData = new Byte(4, 0x0 + 12);
            public Byte AddressOfEntryPoint = new Byte(4, 0x0 + 16);
            public Byte BaseOfCode = new Byte(4, 0x0 + 20);
            public Byte BaseOfData = new Byte(4, 0x0 + 24);
            public Byte ImageBase = new Byte(4, 0x0 + 28);
            public Byte SectionAlignment = new Byte(4, 0x0 + 32);
            public Byte FileAlignment = new Byte(4, 0x0 + 36);
            public Byte MajorOperatingSystemVersion = new Byte(2, 0x0 + 40);
            public Byte MinorOperatingSystemVersion = new Byte(2, 0x0 + 42);
            public Byte MajorImageVersion = new Byte(2, 0x0 + 44);
            public Byte MinorImageVersion = new Byte(2, 0x0 + 46);
            public Byte MajorSubsystemVersion = new Byte(2, 0x0 + 48);
            public Byte MinorSubsystemVersion = new Byte(2, 0x0 + 50);
            public Byte Win32VersionValue = new Byte(4, 0x0 + 52);
            public Byte SizeOfImage = new Byte(4, 0x0 + 56);
            public Byte SizeOfHeaders = new Byte(4, 0x0 + 60);
            public Byte CheckSum = new Byte(4, 0x0 + 64);
            public Byte Subsystem = new Byte(2, 0x0 + 68);
            public Byte DllCharacteristics = new Byte(2, 0x0 + 70);
            public Byte SizeOfStackReserve = new Byte(4, 0x0 + 72);
            public Byte SizeOfStackCommit = new Byte(4, 0x0 + 76);
            public Byte SizeOfHeapReserve = new Byte(4, 0x0 + 80);
            public Byte SizeOfHeapCommit = new Byte(4, 0x0 + 84);
            public Byte LoaderFlags = new Byte(4, 0x0 + 88);
            public Byte NumberOfRvaAndSizes = new Byte(4, 0x0 + 92);

            public ImageDataDir[] DataDirectory = new ImageDataDir[16];

            public OptionalHeader(Stream fStream, long offset){
                Magic.read(fStream, offset); 
                MajorLinkerVersion.read(fStream, offset);
                MinorLinkerVersion.read(fStream, offset);
                SizeOfCode.read(fStream, offset);
                SizeOfInitializedData.read(fStream, offset);
                SizeOfUninitializedData.read(fStream, offset);
                AddressOfEntryPoint.read(fStream, offset);
                BaseOfCode.read(fStream, offset);
                BaseOfData.read(fStream, offset);
                ImageBase.read(fStream, offset);
                SectionAlignment.read(fStream, offset);
                FileAlignment.read(fStream, offset);
                MajorOperatingSystemVersion.read(fStream, offset);
                MinorOperatingSystemVersion.read(fStream, offset);
                MajorImageVersion.read(fStream, offset);
                MinorImageVersion.read(fStream, offset);
                MajorSubsystemVersion.read(fStream, offset);
                MinorSubsystemVersion.read(fStream, offset);
                Win32VersionValue.read(fStream, offset);
                SizeOfImage.read(fStream, offset);
                SizeOfHeaders.read(fStream, offset);
                CheckSum.read(fStream, offset);
                Subsystem.read(fStream, offset);
                DllCharacteristics.read(fStream, offset);
                SizeOfStackReserve.read(fStream, offset);
                SizeOfStackCommit.read(fStream, offset);
                SizeOfHeapReserve.read(fStream, offset);
                SizeOfHeapCommit.read(fStream, offset);
                LoaderFlags.read(fStream, offset);
                NumberOfRvaAndSizes.read(fStream, offset);

                for(int i = 0; i < 16; i++){
                    DataDirectory[i] = new ImageDataDir(fStream, offset+96 + 8*i);
                }
            }

            public int GetImageBase() => Util.ParseNum(this.ImageBase.data);
        }

        class SectionHeader{
            //long offset;

            public Byte Name = new Byte(8, 0x0);
            public Byte PhysicalAddress = new Byte(4, 0x0 + 8);
            public Byte VirtualSize = new Byte(4, 0x0 + 12);
            public Byte VirtualAddress = new Byte(4, 0x0 + 16);
            public Byte SizeOfRawData = new Byte(4, 0x0 + 20);
            public Byte PointerToRawData = new Byte(4, 0x0 + 24);
            public Byte PointerToRelocations = new Byte(4, 0x0 + 28);
            public Byte PointerToLinenumbers = new Byte(4, 0x0 + 32);
            public Byte NumberOfRelocations = new Byte(2, 0x0 + 36);
            public Byte NumberOfLinenumbers = new Byte(2, 0x0 + 38);
            public Byte Characteristics = new Byte(4, 0x0 + 40);

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
        }
        
        class PEHeader{
            //long offset;

            public Byte signature = new Byte(4, 0x0);
            public FileHeader fileHeader;
            public OptionalHeader optionalHeader;

            public PEHeader(Stream fStream, long offset){
                signature.read(fStream, offset);
                fileHeader = new FileHeader(fStream, offset + 4);
                optionalHeader = new OptionalHeader(fStream, offset + 24);
            }
        }

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
            }

            public string GetName() => Util.ParseString(Name.data);
            public int GetAddressOfFunctions() => Util.ParseNum(AddressOfFunctions.data);
        }

        DosHeader dosheader;
        PEHeader peheader;
        List<SectionHeader> sectionHeaders = new List<SectionHeader>();
        ExportTable exportTable;

        public PE(string fileName){
            Stream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            dosheader = new DosHeader(file);
            peheader = new PEHeader(file, dosheader.e_lfanew.data[0]);
            
            for(int i = 0; i < peheader.fileHeader.GetNumberOfSections(); i++){
                sectionHeaders.Add(new SectionHeader(file, 488+40*i));
            }

            exportTable = new ExportTable(file, peheader.optionalHeader.DataDirectory[0].GetVirtualAddress() - 0xC00);
            Console.WriteLine(exportTable.GetAddressOfFunctions());
            
            // Console.WriteLine(peheader.optionalHeader.DataDirectory[0].VirtualAddress.data[0].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.DataDirectory[0].VirtualAddress.data[1].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.DataDirectory[0].VirtualAddress.data[2].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.DataDirectory[0].VirtualAddress.data[3].ToString("x"));
            
            //Console.WriteLine(BitConverter.ToUInt16(peheader.optionalHeader.DataDirectory[0].VirtualAddress.data)); 

            // Console.WriteLine(peheader.optionalHeader.ImageBase.data[0].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.ImageBase.data[1].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.ImageBase.data[2].ToString("x"));
            // Console.WriteLine(peheader.optionalHeader.ImageBase.data[3].ToString("x"));
            
            //Console.WriteLine(BitConverter.ToInt16(new System.Byte[]{peheader.optionalHeader.ImageBase.data[2], peheader.optionalHeader.ImageBase.data[3]}));
        }
    }


    class Program{
        static void Main(string[] args) {
            //new PE("7z1900-x64.exe");
            new PE("twain_32.dll");
        }
    }
}
