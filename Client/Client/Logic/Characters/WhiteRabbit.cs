using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Client.Common;
using Client.Logic;

namespace Client
{
    public class WhiteRabbit : Character
    {
        public WhiteRabbit(Vector2 position, Color color)
            : base(position, color, 192, 192, 150f, 4, 12)
        {
            am = new AnimationManager(16);
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
                SetNewAnimation(0);
            }

            if (inputManager.CheckIfPressingKey(Keys.Space))
            {
                SetNewAnimation(1);
            }
            if (inputManager.CheckIfPressingKey(Keys.O))
            {
                SetNewAnimation(2);
            }
            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 16), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
