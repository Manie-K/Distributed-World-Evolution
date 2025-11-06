using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Client.Common;
using Client.Logic;

namespace Client
{
    public class Slime1 : Character
    {
        public Slime1(Vector2 position, Color color)
            : base(position, color, 100, 100, 150f, 6, 9)
        {
            am = new AnimationManager(6);
            SpriteDrawingOffset = new Vector2(-35, -37);
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

            SetAnimation(0);

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Position += movement * speed * delta;
                SetAnimation(0);
            }

            if (inputManager.CheckIfPressingKey(Keys.Space))
            {
                SetAnimation(1);
            }
            if (inputManager.CheckIfPressingKey(Keys.O))
            {
                SetAnimation(2);
            }
            am.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 6), GetPosition(), GetSourceRectangle(), Color.White);
        }
    }
}
