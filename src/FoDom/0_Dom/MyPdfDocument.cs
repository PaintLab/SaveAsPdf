//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
//MIT, 2014-2017, WinterDev

using System.Collections.Generic;
namespace PixelFarm.Drawing.Pdf
{
    public class MyPdfDocument
    {
        List<MyPdfPage> _pages = new List<MyPdfPage>();
        public MyPdfPage CreatePage()
        {
            return new MyPdfPage();
        }
        public List<MyPdfPage> Pages
        {
            get { return _pages; }
        }
    }
    public class MyPdfPage
    {
        MyPdfCanvas _canvas;
        public MyPdfPage()
        {
            _canvas = new MyPdfCanvas(this);
        }
        public MyPdfCanvas Canvas
        {
            get { return _canvas; }
        }
    }
    public class MyPdfCanvas
    {
        //TODO: implement canvas interface
        MyPdfPage _ownerPage;
        List<MyPdfTextBlock> _elems = new List<MyPdfTextBlock>();
        internal MyPdfCanvas(MyPdfPage owner)
        {
            this._ownerPage = owner;
        }
        public void DrawString(string str, float x, float y)
        {
            _elems.Add(
                new MyPdfTextBlock(str));
        }
        internal List<MyPdfTextBlock> TextElems { get { return _elems; } }

    }

    class MyPdfTextBlock
    {
        public MyPdfTextBlock(string str)
        {
            this.Text = str;
        }
        public string Text { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
