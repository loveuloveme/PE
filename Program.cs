﻿using System;
using System.Collections.Generic;
using System.IO;

namespace PE{
    class Program{
        static void Main(string[] args) {
            //new PE("7z1900-x64.exe");
            var file = new PE("7z1900-x64.exe");
            var printFile = new PEprint(file);
            printFile.Print();
            //new PE("7z.dll");
        }
    }
}
