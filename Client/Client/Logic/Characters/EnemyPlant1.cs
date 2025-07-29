using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class EnemyPlant1 : ColoredSprite
    {
        private float speed = 150f;
        private AnimationManager am;
        private int CurrentDirection;

        public EnemyPlant1(Vector2 position, Color color, ref AnimationTexturesLoader ATL)
            : base(null, position, 100, 100, color)
        {
            am = new AnimationManager(ref ATL, 5);
            CurrentDirection = 2;
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                CurrentDirection = 0;
                movement.Y -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                CurrentDirection = 2;
                movement.Y += 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                CurrentDirection = 3;
                movement.X -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                CurrentDirection = 1;
                movement.X += 1;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                position += movement * speed * delta;
                am.SetAnimationWithDuration(6, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(5, CurrentDirection, 1, 36);
            }


            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                am.SetAnimationWithDuration(3, CurrentDirection, 2, 36, true);
            }


            if (am.GetAcctualAnimationIndex() == 3)
            {
                speed = 70f;
                am.SetAnimationWithDuration(3, CurrentDirection, 2, 36, true);
            }
            else speed = 200f;


            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(am.GetAcctualTexture(), Rect, am.GetFrame(), Color.White);
        }
    }
}
