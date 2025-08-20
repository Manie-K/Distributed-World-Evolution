using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Common;

namespace Client
{
    public class EnemyPlant3 : Character
    {


        public EnemyPlant3(Vector2 position, Color color, ref AnimationTexturesLoader ATL)
            : base(position, color, 100, 100, 150f, ref ATL, 22)
        {
        }

        public override void Update(GameTime gameTime, InputManager inputManager)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (inputManager.CheckIfPressingKey(Keys.W))
            {
                CurrentDirection = Direction.up;
                movement.Y -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.S))
            {
                CurrentDirection = Direction.down;
                movement.Y += 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.A))
            {
                CurrentDirection = Direction.left;
                movement.X -= 1;
            }
            if (inputManager.CheckIfPressingKey(Keys.D))
            {
                CurrentDirection = Direction.right;
                movement.X += 1;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Position += movement * speed * delta;
                am.SetAnimationWithDuration(23, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(22, CurrentDirection, 1, 36);
            }


            if (inputManager.CheckIfPressingKey(Keys.Space))
            {
                am.SetAnimationWithDuration(20, CurrentDirection, 2, 36, true);
            }


            if (am.GetAcctualAnimationIndex() == 20)
            {
                speed = 70f;
                am.SetAnimationWithDuration(20, CurrentDirection, 2, 36, true);
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
