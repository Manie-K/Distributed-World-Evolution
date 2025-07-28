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
        private Vector2 FrameSize;
        private int NumFrames;
        private int ColNumbers;
        private int StartRow;
        private int StartCol;

        public AnimationTexture(Texture2D texture, Vector2 FrameSize, int numFrames, int colNumbers, int startRow, int starCol)
        {
            this.texture = texture;
            this.FrameSize = FrameSize;
            NumFrames = numFrames;
            ColNumbers = colNumbers;
            StartRow = startRow;
            StartCol = starCol;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public Vector2 GetFrameSize()
        {
            return FrameSize;
        }

        public int GetNumFrames()
        {
            return NumFrames;
        }

        public int GetStartRow()
        {
            return StartRow;
        }

        public int GetStarCol()
        {
            return StartCol;
        }

        public int GetColNumbers()
        {
            return ColNumbers;
        }
    }
}
