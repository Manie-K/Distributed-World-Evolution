using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logic
{
    public class AnimationManager
    {
        private int interval;
        private int framesAmount;

        public int ActiveFrame;
        private int counter;


        public AnimationManager(int framesAmount, int interval)
        {
            this.framesAmount = framesAmount;
            ActiveFrame = 0;
            this.interval = interval;
            counter = 0;
        }

        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                ActiveFrame++;

                if (ActiveFrame >= framesAmount)
                {
                    ActiveFrame = 0;
                }

            }
        }
    }
}
