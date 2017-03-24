namespace Fonet.Image
{
 
    using Fonet.DataTypes;
    using Fonet.Pdf.Filter;


    //this is fonet image's abstraction

    /// <summary>
    /// A bitmap image that will be referenced by fo:external-graphic.
    /// </summary>
    public class FonetImage
    {
        public const int DEFAULT_BITPLANES = 8;
        
        //// Image URL
        private string m_href = null;

        //// image width
        private int width = 0;

        //// image height
        private int height = 0;

        //// Image color space 
        private ColorSpace m_colorSpace = null;

        //// Bits per pixel
        private int m_bitsPerPixel = 0;

        // Image data 
        private byte[] m_bitmaps = null;


        /// <summary>
        ///     Constructs a new FonetImage using the supplied bitmap.
        /// </summary>
        /// <remarks>
        ///     Does not hold a reference to the passed bitmap.  Instead the
        ///     image data is extracted from <b>bitmap</b> on construction.
        /// </remarks>
        /// <param name="href">The location of <i>bitmap</i></param>
        /// <param name="imageData">The image data</param>
        public FonetImage(string href,
            int width,
            int height,
            int bitPlans,
            byte[] imageData)
        {
            this.m_href = href;
            this.m_bitmaps = imageData;
            this.width = width;
            this.height = height;



            //if (bitmap.RawFormat.Equals(ImageFormat.Jpeg))
            //    //{
            //    //    JpegParser parser = new JpegParser(m_bitmaps);
            //    //    JpegInfo info = parser.Parse();

            //    //    m_bitsPerPixel = info.BitsPerSample;
            //    //    m_colorSpace = new ColorSpace(info.ColourSpace);
            //    //    width = info.Width;
            //    //    height = info.Height;

            //    //    // A "no-op" filter since the JPEG data is already compressed
            //    //    filter = new DctFilter();
            //    //}
            //    //else
            //    //{
            //    //    ExtractOtherImageBits(bitmap);

            //    //    // Performs zip compression
            //    //    filter = new FlateFilter();
            //    //}
            //--------------------------------------------------- 
            m_colorSpace = new ColorSpace(ColorSpace.DeviceRgb); //***
            m_bitsPerPixel = bitPlans;// DEFAULT_BITPLANES; // 8 
        }

        /// <summary>
        ///     Return the image URL.
        /// </summary>
        /// <returns>the image URL (as a string)</returns>
        public string Uri
        {
            get
            {
                return m_href;
            }
        }

        /// <summary>
        ///     Return the image width. 
        /// </summary>
        /// <returns>the image width</returns>
        public int Width
        {
            get
            {

                return this.width;
            }
        }

        /// <summary>
        ///     Return the image height. 
        /// </summary>
        /// <returns>the image height</returns>
        public int Height
        {
            get
            {

                return this.height;
            }
        }

        /// <summary>
        ///     Return the number of bits per pixel. 
        /// </summary>
        /// <returns>number of bits per pixel</returns>
        public int BitsPerPixel
        {
            get
            {
                return m_bitsPerPixel;
            }
        }

        /// <summary>
        ///     Return the image data size
        /// </summary>
        /// <returns>The image data size</returns>
        public int BitmapsSize
        {
            get
            {
                return (m_bitmaps != null) ? m_bitmaps.Length : 0;
            }
        }

        /// <summary>
        ///     Return the image data (uncompressed). 
        /// </summary>
        /// <returns>the image data</returns>
        public byte[] Bitmaps
        {
            get
            {
                return m_bitmaps;
            }
        }

        /// <summary>
        ///     Return the image color space. 
        /// </summary>
        /// <returns>the image color space (Fonet.Datatypes.ColorSpace)</returns>
        public ColorSpace ColorSpace
        {
            get
            {

                return m_colorSpace;
            }
        }
        IFilter filter;
        /// <summary>
        ///     Returns the filter that should be applied to the bitmap data.
        /// </summary>
        public IFilter Filter
        {
            get
            {
                //eg. jpeg=>  filter = new DctFilter();
                //eg. png=> filter ? 

                return filter;
            }
        }
    }

    //public struct PixelData
    //{
    //    public byte blue;
    //    public byte green;
    //    public byte red;
    //}
}