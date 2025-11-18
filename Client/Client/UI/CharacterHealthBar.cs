using Client.Logic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI
{
    public class CharacterHealthBar
    {
        private float rangeBar;

        public CharacterHealthBar()
        {
            rangeBar = 1.0f;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int offsetX, int offsetY)
        {
            spriteBatch.Draw(AssetsManager.GetInstance().GetHealthBarTexture(0), new Vector2(position.X + offsetX, position.Y + offsetY), Color.White);
            spriteBatch.Draw(
                AssetsManager.GetInstance().GetHealthBarTexture(1),
                new Rectangle((int)position.X + offsetX + 2, (int)position.Y + offsetY + 2, (int)(37 * rangeBar), 5),
                new Rectangle(0, 0, (int)(37 * rangeBar), 5),
                Color.White);
        }

        public void SetRangeBar(float range)
        {          
            if (range < 0.0f) rangeBar = 0.0f;
            else if (range > 1.0f) rangeBar = 1.0f;
            else rangeBar = range;
        }
    }
}
