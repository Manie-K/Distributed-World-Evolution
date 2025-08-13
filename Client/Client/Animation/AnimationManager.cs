using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Client
{
    public class AnimationManager
    {
        
        private readonly AnimationTexturesLoader animationtextureLoader;

        private AnimationTexture actualAnimationTexture;
        private int defaultAnimationIndex;
        private int actualAnimationPriority;

        private int actualAnimationIndex;
        private Direction actualAnimationDirectionIndex;
        private bool shouldResetToIdle = false;

        private int counter;
        private int activeFrame;
        private int interval = 7;

        public AnimationManager(ref AnimationTexturesLoader AnimationtextureLoader, int DefaultAnimationIndex)
        {
            animationtextureLoader = AnimationtextureLoader;
            defaultAnimationIndex = DefaultAnimationIndex;
            actualAnimationDirectionIndex = Direction.down;
            SetIdleAnimation();

        }

        public void SetAnimation(int animationIndex, Direction index, int animationPriority, bool resetToIdle = true)
        {
            if (!(actualAnimationIndex == animationIndex && actualAnimationDirectionIndex == index) && animationPriority >= actualAnimationPriority)
            {
                
                if (actualAnimationIndex != animationIndex)
                {
                    activeFrame = 0;
                    counter = 0;
                }

                actualAnimationTexture = animationtextureLoader.GetTexture(animationIndex, index);
                actualAnimationIndex = animationIndex;
                actualAnimationDirectionIndex = index;
                actualAnimationPriority = animationPriority;

                shouldResetToIdle = resetToIdle;
            }
        }

        public void SetAnimationWithDuration(int animationIndex, Direction index, int animationPriority, int totalDuration, bool resetToIdle = true)
        {
            if (!(actualAnimationIndex == animationIndex && actualAnimationDirectionIndex == index) && animationPriority >= actualAnimationPriority)
            {
                if (actualAnimationIndex != animationIndex)
                {
                    activeFrame = 0;
                    counter = 0;
                }

                actualAnimationTexture = animationtextureLoader.GetTexture(animationIndex, index);
                actualAnimationIndex = animationIndex;
                actualAnimationDirectionIndex = index;
                actualAnimationPriority = animationPriority;

                shouldResetToIdle = resetToIdle;

                int numFrames = actualAnimationTexture.GetNumFrames();
                interval = Math.Max(1, totalDuration / numFrames);
            }
        }


        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                activeFrame++;

                if (activeFrame >= actualAnimationTexture.GetNumFrames())
                {
                    if (shouldResetToIdle) SetIdleAnimation();                 
                    else activeFrame = 0;                  
                }

            }
        }

        private void SetIdleAnimation()
        {
            actualAnimationTexture = animationtextureLoader.GetTexture(defaultAnimationIndex, actualAnimationDirectionIndex);
            actualAnimationIndex = 0;
            actualAnimationPriority = 1;
            activeFrame = 0;
            counter = 0;
        }

        public int GetAcctualAnimationIndex()
        {
            return actualAnimationIndex;
        }

        public Rectangle GetFrame()
        {
            int globalFrameIndex = actualAnimationTexture.GetStarCol() + actualAnimationTexture.GetStartRow() * actualAnimationTexture.GetColNumbers() + activeFrame;

            int col = globalFrameIndex % actualAnimationTexture.GetColNumbers();
            int row = globalFrameIndex / actualAnimationTexture.GetColNumbers();

            return new Rectangle(
                col * (int)actualAnimationTexture.GetFrameSize().X,
                row * (int)actualAnimationTexture.GetFrameSize().Y,
                (int)actualAnimationTexture.GetFrameSize().X,
                (int)actualAnimationTexture.GetFrameSize().Y);
        }

        public Texture2D GetAcctualTexture()
        {
            return actualAnimationTexture.GetTexture();
        }
    }
}
