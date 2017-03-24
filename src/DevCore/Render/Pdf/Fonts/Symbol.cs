﻿//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
namespace Fonet.Render.Pdf.Fonts
{
    internal class Symbol : Base14Font
    {
        private static readonly int[] CodePointWidths;

        private static readonly CodePointMapping DefaultMapping
            = CodePointMapping.GetMapping("SymbolEncoding");

        public Symbol()
            : base("Symbol", null, 1010, 1010, -293, 32, 255, CodePointWidths, DefaultMapping) { }

        static Symbol()
        {
            CodePointWidths = new int[256];
            CodePointWidths[0x0020] = 250;
            CodePointWidths[0x0021] = 333;
            CodePointWidths[0x22] = 713;
            CodePointWidths[0x0023] = 500;
            CodePointWidths[0x24] = 549;
            CodePointWidths[0x0025] = 833;
            CodePointWidths[0x0026] = 778;
            CodePointWidths[0x27] = 439;
            CodePointWidths[0x0028] = 333;
            CodePointWidths[0x0029] = 333;
            CodePointWidths[0x2A] = 500;
            CodePointWidths[0x002B] = 549;
            CodePointWidths[0x002C] = 250;
            CodePointWidths[0x2D] = 549;
            CodePointWidths[0x002E] = 250;
            CodePointWidths[0x002F] = 278;
            CodePointWidths[0x0030] = 500;
            CodePointWidths[0x0031] = 500;
            CodePointWidths[0x0032] = 500;
            CodePointWidths[0x0033] = 500;
            CodePointWidths[0x0034] = 500;
            CodePointWidths[0x0035] = 500;
            CodePointWidths[0x0036] = 500;
            CodePointWidths[0x0037] = 500;
            CodePointWidths[0x0038] = 500;
            CodePointWidths[0x0039] = 500;
            CodePointWidths[0x003A] = 278;
            CodePointWidths[0x003B] = 278;
            CodePointWidths[0x003C] = 549;
            CodePointWidths[0x003D] = 549;
            CodePointWidths[0x003E] = 549;
            CodePointWidths[0x003F] = 444;
            CodePointWidths[0x40] = 549;
            CodePointWidths[0x41] = 722;
            CodePointWidths[0x42] = 667;
            CodePointWidths[0x43] = 722;
            CodePointWidths[0x44] = 612;
            CodePointWidths[0x45] = 611;
            CodePointWidths[0x46] = 763;
            CodePointWidths[0x47] = 603;
            CodePointWidths[0x48] = 722;
            CodePointWidths[0x49] = 333;
            CodePointWidths[0x4A] = 631;
            CodePointWidths[0x4B] = 722;
            CodePointWidths[0x4C] = 686;
            CodePointWidths[0x4D] = 889;
            CodePointWidths[0x4E] = 722;
            CodePointWidths[0x4F] = 722;
            CodePointWidths[0x50] = 768;
            CodePointWidths[0x51] = 741;
            CodePointWidths[0x52] = 556;
            CodePointWidths[0x53] = 592;
            CodePointWidths[0x54] = 611;
            CodePointWidths[0x55] = 690;
            CodePointWidths[0x56] = 439;
            CodePointWidths[0x57] = 768;
            CodePointWidths[0x58] = 645;
            CodePointWidths[0x59] = 795;
            CodePointWidths[0x5A] = 611;
            CodePointWidths[0x005B] = 333;
            CodePointWidths[0x5C] = 863;
            CodePointWidths[0x005D] = 333;
            CodePointWidths[0x5E] = 658;
            CodePointWidths[0x005F] = 500;
            CodePointWidths[0x60] = 500;
            CodePointWidths[0x61] = 631;
            CodePointWidths[0x62] = 549;
            CodePointWidths[0x63] = 549;
            CodePointWidths[0x64] = 494;
            CodePointWidths[0x65] = 439;
            CodePointWidths[0x66] = 521;
            CodePointWidths[0x67] = 411;
            CodePointWidths[0x68] = 603;
            CodePointWidths[0x69] = 329;
            CodePointWidths[0x6A] = 603;
            CodePointWidths[0x6B] = 549;
            CodePointWidths[0x6C] = 549;
            CodePointWidths[0x006D] = 576;
            CodePointWidths[0x00B5] = 576;
            CodePointWidths[0x6E] = 521;
            CodePointWidths[0x6F] = 549;
            CodePointWidths[0x70] = 549;
            CodePointWidths[0x71] = 521;
            CodePointWidths[0x72] = 549;
            CodePointWidths[0x73] = 603;
            CodePointWidths[0x74] = 439;
            CodePointWidths[0x75] = 576;
            CodePointWidths[0x76] = 713;
            CodePointWidths[0x77] = 686;
            CodePointWidths[0x78] = 493;
            CodePointWidths[0x79] = 686;
            CodePointWidths[0x7A] = 494;
            CodePointWidths[0x007B] = 480;
            CodePointWidths[0x007C] = 200;
            CodePointWidths[0x007D] = 480;
            CodePointWidths[0x7E] = 549;
            CodePointWidths[0xA1] = 620;
            CodePointWidths[0xA2] = 247;
            CodePointWidths[0xA3] = 549;
            CodePointWidths[0xA4] = 167;
            CodePointWidths[0xA5] = 713;
            CodePointWidths[0x0083] = 500;
            CodePointWidths[0xA7] = 753;
            CodePointWidths[0xA8] = 753;
            CodePointWidths[0xA9] = 753;
            CodePointWidths[0xAA] = 753;
            CodePointWidths[0xAB] = 1042;
            CodePointWidths[0xAC] = 987;
            CodePointWidths[0xAD] = 603;
            CodePointWidths[0xAE] = 987;
            CodePointWidths[0xAF] = 603;
            CodePointWidths[0x00B0] = 400;
            CodePointWidths[0x00B1] = 549;
            CodePointWidths[0xB2] = 411;
            CodePointWidths[0xB3] = 549;
            CodePointWidths[0x00D7] = 549;
            CodePointWidths[0xB5] = 713;
            CodePointWidths[0xB6] = 494;
            CodePointWidths[0x0095] = 460;
            CodePointWidths[0x00F7] = 549;
            CodePointWidths[0xB9] = 549;
            CodePointWidths[0xBA] = 549;
            CodePointWidths[0xBB] = 549;
            CodePointWidths[0x0085] = 1000;
            CodePointWidths[0xBD] = 603;
            CodePointWidths[0xBE] = 1000;
            CodePointWidths[0xBF] = 658;
            CodePointWidths[0xC0] = 823;
            CodePointWidths[0xC1] = 686;
            CodePointWidths[0xC2] = 795;
            CodePointWidths[0xC3] = 987;
            CodePointWidths[0xC4] = 768;
            CodePointWidths[0xC5] = 768;
            CodePointWidths[0xC6] = 823;
            CodePointWidths[0xC7] = 768;
            CodePointWidths[0xC8] = 768;
            CodePointWidths[0xC9] = 713;
            CodePointWidths[0xCA] = 713;
            CodePointWidths[0xCB] = 713;
            CodePointWidths[0xCC] = 713;
            CodePointWidths[0xCD] = 713;
            CodePointWidths[0xCE] = 713;
            CodePointWidths[0xCF] = 713;
            CodePointWidths[0xD0] = 768;
            CodePointWidths[0xD1] = 713;
            CodePointWidths[0xD2] = 790;
            CodePointWidths[0xD3] = 790;
            CodePointWidths[0xD4] = 890;
            CodePointWidths[0xD5] = 823;
            CodePointWidths[0xD6] = 549;
            CodePointWidths[0xD7] = 250;
            CodePointWidths[0x00AC] = 713;
            CodePointWidths[0xD9] = 603;
            CodePointWidths[0xDA] = 603;
            CodePointWidths[0xDB] = 1042;
            CodePointWidths[0xDC] = 987;
            CodePointWidths[0xDD] = 603;
            CodePointWidths[0xDE] = 987;
            CodePointWidths[0xDF] = 603;
            CodePointWidths[0xE0] = 494;
            CodePointWidths[0xE1] = 329;
            CodePointWidths[0xE2] = 790;
            CodePointWidths[0xE3] = 790;
            CodePointWidths[0xE4] = 786;
            CodePointWidths[0xE5] = 713;
            CodePointWidths[0xE6] = 384;
            CodePointWidths[0xE7] = 384;
            CodePointWidths[0xE8] = 384;
            CodePointWidths[0xE9] = 384;
            CodePointWidths[0xEA] = 384;
            CodePointWidths[0xEB] = 384;
            CodePointWidths[0xEC] = 494;
            CodePointWidths[0xED] = 494;
            CodePointWidths[0xEE] = 494;
            CodePointWidths[0xEF] = 494;
            CodePointWidths[0xF1] = 329;
            CodePointWidths[0xF2] = 274;
            CodePointWidths[0xF3] = 686;
            CodePointWidths[0xF4] = 686;
            CodePointWidths[0xF5] = 686;
            CodePointWidths[0xF6] = 384;
            CodePointWidths[0xF7] = 384;
            CodePointWidths[0xF8] = 384;
            CodePointWidths[0xF9] = 384;
            CodePointWidths[0xFA] = 384;
            CodePointWidths[0xFB] = 384;
            CodePointWidths[0xFC] = 494;
            CodePointWidths[0xFD] = 494;
            CodePointWidths[0xFE] = 494;
        }
    }
}