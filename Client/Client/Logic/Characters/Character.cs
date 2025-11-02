using Microsoft.Xna.Framework;
using Client.Common;
using Client.Logic;

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
            SpriteDrawingOffset = new Vector2((width / 2 - 16) * -1, (height / 2 - 16) * -1);
            CurrentDirection = Direction.down;
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(width * am.GetActiveFrame(), height * (int)CurrentDirection, width, height);
        }

        public void SetNewAnimation(int type)
        {
            am.SetNewAnimation(type);
        }

        public virtual void Update(GameTime gameTime, InputManager inputManager) { }
    }
}
