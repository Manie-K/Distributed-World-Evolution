using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using static System.Net.Mime.MediaTypeNames;

namespace Client
{

    public class Player : ColoredSprite
    {
        private float speed = 200f;
        private Text text;
        private AnimationManager am;
        private int CurentDirection;

        public Player(Vector2 position, Color color, Text text, ref AnimationTexturesLoader ATL)
            : base(null, position, 140, 108, color)
        {
            this.text = text;
            am = new AnimationManager(ref ATL);
            CurentDirection = 2;
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                CurentDirection = 0;
                movement.Y -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                CurentDirection = 2;
                movement.Y += 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                CurentDirection = 3;
                movement.X -= 1;
            }
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                CurentDirection = 1;
                movement.X += 1;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                position += movement * speed * delta;
                am.SetAnimation(1, CurentDirection, 1, false);
            }
            else
            {
                am.SetAnimation(0, CurentDirection, 1);
            }


            if (currentKeyboardState.IsKeyDown(Keys.Space))
            {
                am.SetAnimation(2, CurentDirection, 2, true);               
            }


            if (am.GetAcctualAnimationIndex() == 2)
            {
                speed = 70f;
                am.SetAnimation(2, CurentDirection, 2, true);
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
