using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class EnemyPlant3 : Character
    {
        public EnemyPlant3(Vector2 position, Color color)
            : base(position, color, 100, 100, 150f, 4, 12)
        {
            am = new AnimationManager(5);
            SpriteDrawingOffset = new Vector2(-32, -47);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 5), GetPosition(), GetSourceRectangle(), Color.White);
            if(CurrentDirection == Direction.left)
            {
                HealthBar.Draw(spriteBatch, Position, -6, -35);
            }else if(CurrentDirection == Direction.right)
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
