using Microsoft.Xna.Framework;
using Client.Common;
using Client.Logic;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;
using Client.UI;

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
        public CharacterHealthBar HealthBar;
        public int MaxHealth;

        public Character(Vector2 position, Color color, int width, int height, float speed, int maxHealth)
            : base(null, position, width, height, color)
        {
            this.speed = speed;
            MaxHealth = maxHealth;
            CurrentDirection = Direction.down;
            HealthBar = new CharacterHealthBar();
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

        public void SetCurrentDirection(Position2D currentPosition, Position2D newPosition)
        {
            if (newPosition.X > currentPosition.X)
            {
                CurrentDirection = Direction.right;
            }
            else if (newPosition.X < currentPosition.X)
            { 
                CurrentDirection = Direction.left;
            }
            else if (newPosition.Y < currentPosition.Y)
            {
                CurrentDirection = Direction.up;
            }
            else if (newPosition.Y > currentPosition.Y)
            {
                CurrentDirection = Direction.down;
            }
        }
    }
}
