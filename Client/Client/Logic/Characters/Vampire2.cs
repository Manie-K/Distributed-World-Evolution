using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Vampire2 : Character
    {
        public Vampire2(Vector2 position, int maxHealth, Color color)
           : base(position, color, 110, 110, 150f, maxHealth)
        {
            am = new AnimationManager(10);
            SpriteDrawingOffset = new Vector2(-32, -47);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 10), GetPosition(), GetSourceRectangle(), Color.White);
            if (currentDirection == Direction.Left)
            {
                HealthBar.Draw(spriteBatch, Position, 1, -30);
            }
            else if (currentDirection == Direction.Right)
            {
                HealthBar.Draw(spriteBatch, Position, 5, -30);
            }
            else
            {
                HealthBar.Draw(spriteBatch, Position, 3, -30);
            }
        }
    }
}
