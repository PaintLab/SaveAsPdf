using System.Runtime.InteropServices;

namespace Fonet.Pdf.Gdi
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct KerningPair
    {
        public ushort wFirst;
        public ushort wSecond;
        public int iKernAmount;
    }
}