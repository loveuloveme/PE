using System.IO;
using System.Collections.Generic;

namespace PE{
    class PE{ 
        public DosHeader dosheader;
        public PEHeader peheader;
        public List<SectionHeader> sectionHeaders = new List<SectionHeader>();
        
        public ExportTable exportTable;
        public ImportTable importTable;

        public PE(string fileName){
            Stream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            dosheader = new DosHeader(file);

            peheader = new PEHeader(file, dosheader.GetLF());

            for(int i = 0; i < peheader.fileHeader.GetNumberOfSections(); i++){
                sectionHeaders.Add(new SectionHeader(file, peheader.optionalHeader.GetLast()+40*i));
            }

            Util.SetData(sectionHeaders, peheader);

            if(Util.RVAToOffset(peheader.optionalHeader.DataDirectory[0].GetVirtualAddress()) != -1){
                exportTable = new ExportTable(file, Util.RVAToOffset(peheader.optionalHeader.DataDirectory[0].GetVirtualAddress()));
            }

            importTable = new ImportTable(file, Util.RVAToOffset(peheader.optionalHeader.DataDirectory[1].GetVirtualAddress()));
        }
    }
}
