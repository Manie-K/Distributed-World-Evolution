using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Client
{
    public class AnimationManager
    {
        
        private readonly AnimationTexturesLoader _AnimationtextureLoader;

        private AnimationTexture AcctualAnimationTexture;
        private int DefaultAnimationIndex;
        private int AcctualAnimationPriorytet;

        private int AcctualAnimationIndex;
        private int AcctualAnimationDirectionIndex;
        private bool shouldResetToIdle = false;

        private int counter;
        private int activeFrame;
        private int interval = 7;

        public AnimationManager(ref AnimationTexturesLoader AnimationtextureLoader, int DefaultAnimationIndex)
        {
            _AnimationtextureLoader = AnimationtextureLoader;
            this.DefaultAnimationIndex = DefaultAnimationIndex;
            AcctualAnimationDirectionIndex = 2;
            SetIdleAnimation();

        }

        public void SetAnimation(int animationIndex, int index, int animationPriorytet, bool resetToIdle = true)
        {
            if (!(AcctualAnimationIndex == animationIndex && AcctualAnimationDirectionIndex == index) && animationPriorytet >= AcctualAnimationPriorytet)
            {
                
                if (AcctualAnimationIndex != animationIndex)
                {
                    activeFrame = 0;
                    counter = 0;
                }

                AcctualAnimationTexture = _AnimationtextureLoader.GetTexture(animationIndex, index);
                AcctualAnimationIndex = animationIndex;
                AcctualAnimationDirectionIndex = index;
                AcctualAnimationPriorytet = animationPriorytet;

                shouldResetToIdle = resetToIdle;
            }
        }

        public void SetAnimationWithDuration(int animationIndex, int index, int animationPriorytet, int totalDuration, bool resetToIdle = true)
        {
            if (!(AcctualAnimationIndex == animationIndex && AcctualAnimationDirectionIndex == index) && animationPriorytet >= AcctualAnimationPriorytet)
            {
                if (AcctualAnimationIndex != animationIndex)
                {
                    activeFrame = 0;
                    counter = 0;
                }

                AcctualAnimationTexture = _AnimationtextureLoader.GetTexture(animationIndex, index);
                AcctualAnimationIndex = animationIndex;
                AcctualAnimationDirectionIndex = index;
                AcctualAnimationPriorytet = animationPriorytet;

                shouldResetToIdle = resetToIdle;

                int numFrames = AcctualAnimationTexture.GetNumFrames();
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

                if (activeFrame >= AcctualAnimationTexture.GetNumFrames())
                {
                    if (shouldResetToIdle) SetIdleAnimation();                 
                    else activeFrame = 0;                  
                }

            }
        }

        private void SetIdleAnimation()
        {
            AcctualAnimationTexture = _AnimationtextureLoader.GetTexture(DefaultAnimationIndex, AcctualAnimationDirectionIndex);
            AcctualAnimationIndex = 0;
            AcctualAnimationPriorytet = 1;
            activeFrame = 0;
            counter = 0;
        }

        public int GetAcctualAnimationIndex()
        {
            return AcctualAnimationIndex;
        }

        public Rectangle GetFrame()
        {
            int globalFrameIndex = AcctualAnimationTexture.GetStarCol() + AcctualAnimationTexture.GetStartRow() * AcctualAnimationTexture.GetColNumbers() + activeFrame;

            int col = globalFrameIndex % AcctualAnimationTexture.GetColNumbers();
            int row = globalFrameIndex / AcctualAnimationTexture.GetColNumbers();

            return new Rectangle(
                col * (int)AcctualAnimationTexture.GetFrameSize().X,
                row * (int)AcctualAnimationTexture.GetFrameSize().Y,
                (int)AcctualAnimationTexture.GetFrameSize().X,
                (int)AcctualAnimationTexture.GetFrameSize().Y);
        }

        public Texture2D GetAcctualTexture()
        {
            return AcctualAnimationTexture.GetTexture();
        }
    }
}
