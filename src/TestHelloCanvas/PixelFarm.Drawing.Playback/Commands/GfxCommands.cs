//MIT, 2014-2017, WinterDev

using System;

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
        PathCommand,
        SetStroke_Color,
        //4. text and fonts

    }
    public abstract class Command
    {
        public abstract PlaybackCommandKind Kind { get; }
    }
    public class CmdSetCanvasOrigin : Command
    {
        public override PlaybackCommandKind Kind { get { return PlaybackCommandKind.SetCanvasOrigin; } }
        public float X { get; set; }
        public float Y { get; set; }
    }
    public class CmdPath : Command
    {
        public override PlaybackCommandKind Kind { get { return PlaybackCommandKind.PathCommand; } }
    }

    public class CmdSetStrokeColor : Command
    {
        public override PlaybackCommandKind Kind { get { return PlaybackCommandKind.SetStroke_Color; } }
        public Color Color { get; set; }
    }


}