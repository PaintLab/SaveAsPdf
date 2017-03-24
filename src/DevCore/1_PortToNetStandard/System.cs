//MIT, 2017, WinterDev
using System.Collections;
using System.Collections.Generic;
namespace System
{
    public class ApplicationException : Exception
    {
        public ApplicationException() { }
        public ApplicationException(string msg)
          : base(msg)
        {
        }
        public ApplicationException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }
    }
    public interface ICloneable { }
    public class SystemException : Exception
    {
        public SystemException(string msg) : base(msg) { }
    }
}
namespace System.IO
{
    public static class StreamIOExtensions
    {
        public static void Close(this System.IO.Stream strm)
        {
            //do nothing
            strm.Flush();
        }
        public static void Close(this System.IO.Compression.DeflateStream strm)
        {
            strm.Flush();
        }

    }
}
namespace System.Xml
{
    public static class XmlReaderExtensions
    {
        public static void Close(this System.Xml.XmlReader reader) { }
    }

}
