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
    public class EnemyPlant1 : Character
    {

        public EnemyPlant1(Vector2 position, Color color, ref AnimationTexturesLoader ATL)
            : base(position, color, 100, 100, 150f, ref ATL, 5)
        {

        }

        public override void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                CurrentDirection = Direction.up;
                movement.Y -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                CurrentDirection = Direction.down;
                movement.Y += 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                CurrentDirection = Direction.left;
                movement.X -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                CurrentDirection = Direction.right;
                movement.X += 1;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Position += movement * speed * delta;
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
