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

        public int BlockDelayCounter;
        public bool IsBlocked;
        public Animation(int framesAmount, int interval)
        {
            this.framesAmount = framesAmount;
            this.interval = interval;

            IsAnimationEnded = false;
            IsBlocked = false;
            ActiveFrame = 0;
            counter = 0;
            BlockDelayCounter = 0;
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

        public void UpdateBlock()
        {
            BlockDelayCounter++;
            if (BlockDelayCounter > 60) UnBlockAnimation();   
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
            BlockDelayCounter = 0;
        }

        public void UnBlockAnimation()
        {
            IsBlocked = false;
            BlockDelayCounter = 0;
        }
    }
}
