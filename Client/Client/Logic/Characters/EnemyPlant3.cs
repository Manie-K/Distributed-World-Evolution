using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class EnemyPlant3 : Character
    {
        public EnemyPlant3(Vector2 position, int maxHealth, Color color)
            : base(position, color, 100, 100, 150f, maxHealth)
        {
            am = new AnimationManager(5);
            SpriteDrawingOffset = new Vector2(-32, -47);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (am.CheckDeadAnimation()) return;

            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture((int) am.ActiveAnimation, 5), GetPosition(), GetSourceRectangle(), Color.White);
            if(CurrentDirection == Direction.Left)
            {
                HealthBar.Draw(spriteBatch, Position, -6, -35);
            }else if(CurrentDirection == Direction.Right)
            {
                HealthBar.Draw(spriteBatch, Position, 5, -35);
            }
            else
            {
                HealthBar.Draw(spriteBatch, Position, -1, -35);
            }
        }
    }
}
