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
        Dying,
        Attacking,
        Walking
    }

    public class Character : ColoredSprite
    {
        private const float ANIMATION_TIMER_TRIGGER = 1.0f;

        private AnimationType animationType;
        private float animationTimer;
        protected float speed;
        protected Direction CurrentDirection;
        public AnimationManager am;
        public CharacterHealthBar HealthBar;
        public int MaxHealth;
        public bool isDead;

        public Character(Vector2 position, Color color, int width, int height, float speed, int maxHealth)
            : base(null, position, width, height, color)
        {
            animationType = AnimationType.Walking;
            animationTimer = 0;
            this.speed = speed;
            MaxHealth = maxHealth;
            CurrentDirection = Direction.Down;
            HealthBar = new CharacterHealthBar();
            isDead = false;
        }

        public Rectangle GetSourceRectangle()
        {
            return new Rectangle(width * am.GetActiveFrame(), height * (int)CurrentDirection, width, height);
        }

        public void SetAnimation(int type)
        {
            am.SetAnimation(type);
        }

        public virtual void Update(GameTime gameTime, EntityStateDTO state) 
        {
            UpdateTimer(gameTime);
            SetAnimation(0);

            if (state.Health <= 0)
            {
                SetAnimation(2);
                animationType = AnimationType.Dying;
                animationTimer = 0.0f;
            }
            else if (state.LastInteractionName.Equals("AttackBehaviourBase"))
            {
                SetAnimation(1);
                if (animationType != AnimationType.Attacking)
                {
                    animationType = AnimationType.Attacking;
                    animationTimer = 0.0f;
                }
            }

            am.Update(gameTime);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateTimer(gameTime);
            SetAnimation(0);

            if (animationType == AnimationType.Dying)
            {
                SetAnimation(2);
            }
            else if (animationType == AnimationType.Attacking)
            {
                SetAnimation(1);
            }

            am.Update(gameTime);
        }

        public void SetCurrentDirection(Position2D currentPosition, Position2D newPosition)
        {
            if (newPosition.X > currentPosition.X)
            {
                CurrentDirection = Direction.Right;
            }
            else if (newPosition.X < currentPosition.X)
            { 
                CurrentDirection = Direction.Left;
            }
            else if (newPosition.Y < currentPosition.Y)
            {
                CurrentDirection = Direction.Up;
            }
            else if (newPosition.Y > currentPosition.Y)
            {
                CurrentDirection = Direction.Down;
            }
        }

        private void UpdateTimer(GameTime gameTime)
        {
            if (animationType == AnimationType.Dying || animationType == AnimationType.Attacking)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                animationTimer += delta;
            }

            if (animationType == AnimationType.Dying && animationTimer > ANIMATION_TIMER_TRIGGER)
            {
                isDead = true;
            }
            else if (animationType == AnimationType.Attacking && animationTimer > ANIMATION_TIMER_TRIGGER)
            {
                animationType = AnimationType.Walking;
                animationTimer = 0.0f;
            }
        }
    }
}
