using System;
using System.IO;
using System.Collections.Generic;

namespace PE{
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
    
    static class Util{
        static PEHeader peheader;
        static List<SectionHeader> sectionHeaders;

        public static string Reverse(string s){
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int ParseNum(System.Byte[] bytes){
            List<string> bytesStr = new List<string>();

            foreach(var byteItem in bytes){
                bytesStr.Add(byteItem.ToString("x") == "0" ? "00" : byteItem.ToString("x").PadLeft(2, '0'));
            }

            bytesStr.Reverse();

            string result = "";
            
            foreach(var byteItem in bytesStr){
                result += byteItem;
            }

            return int.Parse(result, System.Globalization.NumberStyles.HexNumber);
        }

        public static long ParseNum64(System.Byte[] bytes){
            List<string> bytesStr = new List<string>();

            foreach(var byteItem in bytes){
                bytesStr.Add(byteItem.ToString("x") == "0" ? "00" : byteItem.ToString("x").PadLeft(2, '0'));
            }

            bytesStr.Reverse();

            string result = "";
            
            foreach(var byteItem in bytesStr){
                result += byteItem;
            }

            return Int64.Parse(result, System.Globalization.NumberStyles.HexNumber);
        }

        public static string ParseString(System.Byte[] data){
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public static int RVAToOffset(int rva){
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

        public static void SetData(List<SectionHeader> sectionHeaders_, PEHeader peheader_){
            sectionHeaders = sectionHeaders_;
            peheader = peheader_;
        }
    }
}
