using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class EnemyPlant1 : Character
    {
        public EnemyPlant1(Vector2 position, int maxHealth, Color color)
            : base(position, color, 100, 100, 150f, maxHealth)
        {
            am = new AnimationManager(3);
            SpriteDrawingOffset = new Vector2(-32, -47);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 3), GetPosition(), GetSourceRectangle(), Color.White);
            if (currentDirection == Direction.Left)
            {
                HealthBar.Draw(spriteBatch, Position, -5, -35);
            }
            else if (currentDirection == Direction.Right)
            {
                HealthBar.Draw(spriteBatch, Position, 3, -35);
            }
            else
            {
                HealthBar.Draw(spriteBatch, Position, -1, -35);
            }
        }
    }
}
