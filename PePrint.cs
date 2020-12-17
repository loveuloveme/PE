using System;
using System.Drawing;
using Console = Colorful.Console;

namespace PE {
    class PEprint{
        private PE pefile;

        private void PrintNewLine(){
            Console.WriteLine("");
        }

        private void PrintTitle(string title){
            Console.WriteLine(" "+title, Color.DarkRed);
        }

        private void PrintProp(string name, string data){
            Console.Write("     "+name+": ", Color.Gray);
            Console.WriteLine(data, Color.WhiteSmoke);
        }

        private void PrintProp(string name, int data, bool parse = true){
            Console.Write("     "+name+": ", Color.Gray);
            
            if(parse){
                Console.WriteLine("0x"+data.ToString("x"), Color.WhiteSmoke);
            }else{
                Console.WriteLine(data, Color.WhiteSmoke);
            }
        }

        private void PrintProp(string name, long data, bool parse = true){
            Console.Write("     "+name+": ", Color.Gray);
            
            if(parse){
                Console.WriteLine("0x"+data.ToString("x"), Color.WhiteSmoke);
            }else{
                Console.WriteLine(data, Color.WhiteSmoke);
            }
        }

        private void PrintProp(string name, DateTime time){
            Console.Write("     "+name+": ", Color.Gray);
            Console.WriteLine($"{time.Day.ToString().PadLeft(2, '0')}:{time.Month.ToString().PadLeft(2, '0')}:{time.Year}", Color.WhiteSmoke);
        }

        private void PrintDos(){
            PrintTitle("DOS header:");
            PrintProp("e_magic", pefile.dosheader.GetMagic());
            PrintProp("e_lfanew", pefile.dosheader.GetLF());
        }

        private void PrintFileHeader(){
            PrintTitle("File header:");
            
            if(pefile.peheader.fileHeader.GetMachine().ToString("x") == "8664"){
                PrintProp("Machine", "x64");
            }else if(pefile.peheader.fileHeader.GetMachine().ToString("x") == "14c"){
                PrintProp("Machine", "Intel 386");
            }else{
                PrintProp("Machine", "Other");
            }

            PrintProp("NumberOfSections", pefile.peheader.fileHeader.GetNumberOfSections());

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    
            PrintProp("TimeDateStamp", origin.AddSeconds(pefile.peheader.fileHeader.GetTimeDateStamp()));

            PrintProp("PointerToSymbolTable", pefile.peheader.fileHeader.GetPointerToSymbolTable());
            PrintProp("NumberOfSymbols", pefile.peheader.fileHeader.GetNumberOfSymbols(), false);
            PrintProp("SizeOfOptionalHeader", pefile.peheader.fileHeader.GetSizeOfOptionalHeader());
            PrintProp("Characteristics", string.Join(" ", pefile.peheader.fileHeader.ParseCharacteristics().ToArray()));
        }

        private void PrintOptionalHeader(){
            PrintTitle("Optional header:");

            PrintProp("Magic", pefile.peheader.optionalHeader.GetMagic());
            PrintProp("MajorLinkerVersion", pefile.peheader.optionalHeader.GetMajorLinkerVersion());
            PrintProp("MinorLinkerVersion", pefile.peheader.optionalHeader.GetMinorLinkerVersion());
            PrintProp("SizeOfCode", pefile.peheader.optionalHeader.GetSizeOfCode());
            PrintProp("SizeOfInitializedData", pefile.peheader.optionalHeader.GetSizeOfInitializedData());
            PrintProp("SizeOfUninitializedData", pefile.peheader.optionalHeader.GetSizeOfUninitializedData());
            PrintProp("AddressOfEntryPoint", pefile.peheader.optionalHeader.GetAddressOfEntryPoint());
            PrintProp("BaseOfCode", pefile.peheader.optionalHeader.GetBaseOfCode());
            PrintProp("ImageBase", pefile.peheader.optionalHeader.GetImageBase());
            PrintProp("SectionAlignment", pefile.peheader.optionalHeader.GetSectionAlignment());
            PrintProp("FileAlignment", pefile.peheader.optionalHeader.GetFileAlignment());
            PrintProp("MajorOperatingSystemVersion", pefile.peheader.optionalHeader.GetMajorOperatingSystemVersion());
            PrintProp("MinorOperatingSystemVersion", pefile.peheader.optionalHeader.GetMinorOperatingSystemVersion());
            PrintProp("MajorImageVersion", pefile.peheader.optionalHeader.GetMajorImageVersion());
            PrintProp("MinorImageVersion", pefile.peheader.optionalHeader.GetMinorImageVersion());
            PrintProp("MajorSubsystemVersion", pefile.peheader.optionalHeader.GetMajorSubsystemVersion());
            PrintProp("MinorSubsystemVersion", pefile.peheader.optionalHeader.GetMinorSubsystemVersion());
            PrintProp("Win32VersionValue", pefile.peheader.optionalHeader.GetWin32VersionValue());
            PrintProp("SizeOfImage", pefile.peheader.optionalHeader.GetSizeOfImage());
            PrintProp("SizeOfHeaders", pefile.peheader.optionalHeader.GetSizeOfHeaders());
            PrintProp("CheckSum", pefile.peheader.optionalHeader.GetCheckSum());

            var subSys = pefile.peheader.optionalHeader.GetSubsystem();
            var sysStr = "";
            
            switch (pefile.peheader.optionalHeader.GetSubsystem()){
                case 0:
                    sysStr = "IMAGE_SUBSYSTEM_UNKNOWN";
                    break;
                case 1:
                    sysStr = "IMAGE_SUBSYSTEM_NATIVE";
                    break;
                case 2:
                    sysStr = "IMAGE_SUBSYSTEM_WINDOWS_GUI";
                    break;
                case 3:
                    sysStr = "IMAGE_SUBSYSTEM_WINDOWS_CUI";
                    break;
                case 5:
                    sysStr = "IMAGE_SUBSYSTEM_OS2_CUI";
                    break;
                case 7:
                    sysStr = "IMAGE_SUBSYSTEM_POSIX_CUI";
                    break;
                case 8:
                    sysStr = "IMAGE_SUBSYSTEM_NATIVE_WINDOWS";
                    break;
                case 9:
                    sysStr = "IMAGE_SUBSYSTEM_WINDOWS_CE_GUI";
                    break;
                case 10:
                    sysStr = "IMAGE_SUBSYSTEM_EFI_APPLICATION";
                    break;
                case 11:
                    sysStr = "IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER";
                    break;
                case 12:
                    sysStr = "IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER";
                    break;
                case 13:
                    sysStr = "IMAGE_SUBSYSTEM_EFI_ROM";
                    break;
                case 14:
                    sysStr = "IMAGE_SUBSYSTEM_XBOX";
                    break;
                case 16:
                    sysStr = "IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION";
                    break;
            }

            PrintProp("Subsystem", sysStr);

            PrintProp("DllCharacteristics", pefile.peheader.optionalHeader.GetDllCharacteristics());
            PrintProp("SizeOfStackReserve", pefile.peheader.optionalHeader.GetSizeOfStackReserve());
            PrintProp("SizeOfStackCommit", pefile.peheader.optionalHeader.GetSizeOfStackCommit());
            PrintProp("SizeOfHeapReserve", pefile.peheader.optionalHeader.GetSizeOfHeapReserve());
            PrintProp("SizeOfHeapCommit", pefile.peheader.optionalHeader.GetSizeOfHeapCommit());
            PrintProp("LoaderFlags", pefile.peheader.optionalHeader.GetLoaderFlags());
            PrintProp("NumberOfRvaAndSizes", pefile.peheader.optionalHeader.GetNumberOfRvaAndSizes());
        }

        private void PrintSections(){
            PrintTitle("Section Headers:");
            foreach (var item in pefile.sectionHeaders){
                PrintNewLine();
                PrintProp("Name", item.GetName());
                PrintProp("Virtual address", item.GetVirtualAddress());
                PrintProp("Virtual size", item.GetVirtualSize());
                PrintProp("Pointer to RAW data", item.GetPointerToRawData());
                PrintProp("Size of RAW data", item.GetSizeOfRawData());
                PrintProp("Characteristics", item.GetCharacteristics());
                PrintNewLine();
            }
        }

        private void PrintExportTable(){
            PrintTitle("Export Table:");
            if(pefile.exportTable == null) return;
            foreach (var item in pefile.exportTable.names){
                PrintProp("Export name", item);
            }
        }

        private void PrintImportTable(){
            PrintTitle("Import Table:");
            if(pefile.importTable == null) return;
            foreach (var item in pefile.importTable.descriptors){
                PrintProp("Import name", item.ImportName);
            }
        }

        private void PrintDirectoryTable(){
            PrintTitle("Directory Table:");
            var i = 0;
            
            foreach(var item in pefile.peheader.optionalHeader.DataDirectory){
                PrintProp("#", i, false);
                PrintProp("Virtual address", item.GetVirtualAddress());
                PrintProp("Size", item.GetSize());
                PrintNewLine();
                i++;
            }
        }

        public PEprint(PE pefile){
            this.pefile = pefile;
        }

        public void Print(){
            PrintDos();
            PrintNewLine();
            PrintFileHeader();
            PrintNewLine();
            PrintOptionalHeader();
            PrintNewLine();
            PrintDirectoryTable();
            PrintNewLine();
            PrintSections();
            PrintNewLine();
            PrintExportTable();
            PrintNewLine();
            PrintImportTable();
        }
    }
}
