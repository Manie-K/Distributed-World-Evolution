using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class AnimationTexture
    {
        private Texture2D texture;
        private Vector2 frameSize;
        private int numFrames;
        private int colNumbers;
        private int startRow;
        private int startCol;

        public AnimationTexture(Texture2D Texture, Vector2 FrameSize, int NumFrames, int ColNumbers, int StartRow, int StarCol)
        {
            texture = Texture;
            frameSize = FrameSize;
            numFrames = NumFrames;
            colNumbers = ColNumbers;
            startRow = StartRow;
            startCol = StarCol;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Vector2 GetFrameSize()
        {
            return frameSize;
        }

        public int GetNumFrames()
        {
            return numFrames;
        }

        public int GetStartRow()
        {
            return startRow;
        }

        public int GetStarCol()
        {
            return startCol;
        }

        public int GetColNumbers()
        {
            return colNumbers;
        }
    }
}
