using Client.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class Player : Character
    {
        private Text playerName;
        private Vector2 playerNameOffset;

        public Player(Vector2 position, Color color, Text playerName, ref AnimationTexturesLoader ATL)
            : base(position, color, 140, 108, 150f, ref ATL, 0)
        {
            this.playerName = playerName;
            playerNameOffset = new Vector2(34, 0);
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
                am.SetAnimationWithDuration(1, CurrentDirection, 1, 36, false);
            }
            else
            {
                am.SetAnimationWithDuration(0, CurrentDirection, 1, 36);
            }


            if (inputManager.CheckIfPressingKey(Keys.Space))
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
            playerName.Draw(spriteBatch, Position + playerNameOffset);
        }
    }
}
