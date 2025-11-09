using Microsoft.Xna.Framework;
using Client.Common;
using Client.Logic;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Client
{
    public enum Direction
    {
        down,
        up,
        left,
        right
    }

    public class Character : ColoredSprite
    {
        protected float speed;
        protected Direction CurrentDirection;
        public AnimationManager am;
       

        public Character(Vector2 position, Color color, int width, int height, float speed, int framesAmount, int interval)
            : base(null, position, width, height, color)
        {
            this.speed = speed;
            CurrentDirection = Direction.down;
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(width * am.GetActiveFrame(), height * (int)CurrentDirection, width, height);
        }

        public void SetAnimation(int type)
        {
            am.SetAnimation(type);
        }

        public virtual void Update(GameTime gameTime, InputManager inputManager, EntityStateDTO state) 
        {
            SetAnimation(0);

            if (state.Health <= 0)
            {
                SetAnimation(2);
            }
            else if (state.LastInteractionName.Equals("attack"))
            {
                SetAnimation(1);
            }

            am.Update(gameTime);
        }
    }
}
