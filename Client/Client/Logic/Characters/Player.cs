using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{

    public class Player : Character
    {

        private Text text;

        public Player(Vector2 position, Color color, Text text, ref AnimationTexturesLoader ATL)
            : base(position, color, 140, 108, 150f, ref ATL, 0)
        {
            this.text = text;

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
                am.SetAnimationWithDuration(1, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(0, CurrentDirection, 1, 36);
            }


            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);               
            }


            if (am.GetAcctualAnimationIndex() == 2)
            {
                speed = 70f;
                am.SetAnimationWithDuration(2, CurrentDirection, 2, 36, true);
            }
            else speed = 200f;
            

            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(am.GetAcctualTexture(), Rect, am.GetFrame(), Color.White);
            text.Draw(spriteBatch);
        }
    }
}
