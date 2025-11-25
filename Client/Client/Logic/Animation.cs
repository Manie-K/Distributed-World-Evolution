using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class Animation
    {
        private int interval;
        private int framesAmount;

        public int ActiveFrame;
        private int counter;

        public bool IsAnimationEnded;

        public float BlockDelayCounter;
        public float BlockDelay;
        public bool IsBlocked;
        public Animation(int framesAmount, int interval, float blockDelay = 1.0f)
        {
            this.framesAmount = framesAmount;
            this.interval = interval;

            IsAnimationEnded = false;
            IsBlocked = false;
            ActiveFrame = 0;
            counter = 0;
            BlockDelay = blockDelay;
            BlockDelayCounter = blockDelay;
        }

        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                ActiveFrame++;
                IsAnimationEnded = false;

                if (ActiveFrame >= framesAmount)
                {
                    ActiveFrame = 0;
                    IsAnimationEnded = true;
                }

            }
        }

        public void UpdateBlock(GameTime gameTime)
        {
            if (BlockDelayCounter > 0)
            {
                BlockDelayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (BlockDelayCounter <= 0) UnBlockAnimation();   
        }

        public void Reset()
        {
            counter = 0;
            ActiveFrame = 0;
            IsAnimationEnded = false;
        }

        public void BlockAnimation()
        {
            IsBlocked = true;
            BlockDelayCounter = BlockDelay;
        }

        public void UnBlockAnimation()
        {
            IsBlocked = false;
            BlockDelayCounter = BlockDelay;
        }
    }
}
