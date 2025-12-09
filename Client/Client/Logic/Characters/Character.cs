using Client.Logic;
using Client.UI;
using Microsoft.Xna.Framework;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Client
{
    public enum Direction
    {
        Down,
        Up,
        Left,
        Right
    }

    public enum AnimationType
    {
        Walking,
        Attacking,
        Dying
    }

    public class Character : ColoredSprite
    {
        protected float speed;
        protected Direction currentDirection;

        public AnimationManager am;
        public CharacterHealthBar HealthBar;
        public int MaxHealth;
        public bool IsDead;

        public Character(Vector2 position, Color color, int width, int height, float speed, int maxHealth)
            : base(null, position, width, height, color)
        {
            this.speed = speed;
            MaxHealth = maxHealth;
            currentDirection = Direction.Down;
            HealthBar = new CharacterHealthBar();
            IsDead = false;
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(width * am.GetActiveFrame(), height * (int)currentDirection, width, height);
        }

        public void SetAnimation(AnimationType type)
        {
            am.SetAnimation(type);
        }

        public virtual void Update(GameTime gameTime, EntityStateDTO state) 
        {
            SetAnimation(AnimationType.Walking);

            if (state.Health <= 0)
            {
                SetAnimation(AnimationType.Dying);
            }
            else if (state.LastInteractionName.Equals("AttackBehaviourBase"))
            {
                SetAnimation(AnimationType.Attacking);  
            }

            if (am.CheckDeadAnimation())
            {
                IsDead = true;
            }

            am.Update(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            SetAnimation(AnimationType.Walking);

            if (am.CheckDeadAnimation())
            {
                IsDead = true;
            }

            am.Update(gameTime);
        }

        public void SetCurrentDirection(Position2D currentPosition, Position2D newPosition)
        {
            if (newPosition.X > currentPosition.X)
            {
                currentDirection = Direction.Right;
            }
            else if (newPosition.X < currentPosition.X)
            { 
                currentDirection = Direction.Left;
            }
            else if (newPosition.Y < currentPosition.Y)
            {
                currentDirection = Direction.Up;
            }
            else if (newPosition.Y > currentPosition.Y)
            {
                currentDirection = Direction.Down;
            }
        }
    }
}
