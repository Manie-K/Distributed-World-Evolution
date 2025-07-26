using Microsoft.Xna.Framework;

namespace Client
{
    public class Animation
    {
        public int NumFrames { get; }
        public int SheetColumns { get; } 
        public Vector2 FrameSize { get; }

        public int StartRow { get; }
        public int StartCol { get; }

        public Animation(int numFrames, int sheetColumns, Vector2 frameSize, int startRow, int startCol)
        {
            NumFrames = numFrames;
            SheetColumns = sheetColumns;
            FrameSize = frameSize;

            StartRow = startRow;
            StartCol = startCol;
        }
    }
}