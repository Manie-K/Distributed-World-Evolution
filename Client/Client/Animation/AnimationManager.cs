using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class AnimationManager
    {
        private Animation animation;

        private int counter;
        private int activeFrame;
        private int interval = 8;

        public AnimationManager(Animation animation)
        {
            SetAnimation(animation);
        }

        public void SetAnimation(Animation newAnimation)
        {
            animation = newAnimation;
            activeFrame = 0;
            counter = 0;
        }

        public void Update()
        {
            counter++;
            if (counter > interval)
            {
                counter = 0;
                activeFrame = (activeFrame + 1) % animation.NumFrames;
            }
        }

        public Rectangle GetFrame()
        {
            // Oblicz indeks absolutny w sprite sheet
            int globalFrameIndex = animation.StartCol + animation.StartRow * animation.SheetColumns + activeFrame;

            int col = globalFrameIndex % animation.SheetColumns;
            int row = globalFrameIndex / animation.SheetColumns;

            return new Rectangle(
                col * (int)animation.FrameSize.X,
                row * (int)animation.FrameSize.Y,
                (int)animation.FrameSize.X,
                (int)animation.FrameSize.Y);
        }
    }
}
