//MIT, 2014-2017, WinterDev

namespace PixelFarm.Drawing.Playback
{
    public enum PlaybackCommandKind
    {
        Unknown,
        //------------------
        //1. init

        //2. coordinates
        //------------------
        SetCanvasOrigin,
        //3. drawing

        //4. text and fonts



    }
    public class Command
    {
        PlaybackCommandKind _kind;
    }



}