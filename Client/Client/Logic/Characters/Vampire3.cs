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
    public class Vampire3 : Character
    {


        public Vampire3(Vector2 position, Color color, ref AnimationTexturesLoader ATL)
            : base(position, color, 110, 110, 150f, ref ATL, 58)
        {
  
        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
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
                am.SetAnimationWithDuration(59, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(58, CurrentDirection, 1, 36);
            }


            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                am.SetAnimationWithDuration(56, CurrentDirection, 2, 36, true);
            }


            if (am.GetAcctualAnimationIndex() == 56)
            {
                speed = 70f;
                am.SetAnimationWithDuration(56, CurrentDirection, 2, 36, true);
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
