using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Client.Logic;

namespace Client
{
    public class Pig : Character
    {
        public Pig(Vector2 position, int maxHealth, Color color)
            : base(position, color, 173, 173, 150f, maxHealth)
        {
            am = new AnimationManager(14);
            SpriteDrawingOffset = new Vector2(-72, -80);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetCharacterTexture(am.ActiveAnimation, 14), GetPosition(), GetSourceRectangle(), Color.White);
            if (CurrentDirection == Direction.Left)
            {
                HealthBar.Draw(spriteBatch, Position, 0, -15);
            }
            else if (CurrentDirection == Direction.Right)
            {
                HealthBar.Draw(spriteBatch, Position, -8, -15);
            }
            else
            {
                HealthBar.Draw(spriteBatch, Position, -4, -15);
            }
        }
    }
}
