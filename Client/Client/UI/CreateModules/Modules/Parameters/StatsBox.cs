using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class StatsBox
    {
        private Texture2D textureStatsBox;
        private Rectangle rect;
        private TextBox parameterValue;

        public StatsBox(Texture2D texture, SpriteFont font, int value, Vector2 position)
        {
            parameterValue = new TextBox(null, font, new Vector2(position.X + 338, position.Y-2), 86, 46, Color.White, true);
            textureStatsBox = texture;
            rect = new Rectangle((int) position.X+338, (int) position.Y-2, 86, 46);
            parameterValue.SetTextColor(Color.White);

            parameterValue.SetText(value.ToString());
        }

        public bool CheckLeftClick(Vector2 cursorPosition)
        {
            return parameterValue.CheckLeftClick(cursorPosition);

        }

        public void Update()
        {
            parameterValue.Update();
        }

        public void SetParameter(string parameter)
        {
            parameterValue.SetText(parameter);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureStatsBox, rect, Color.White);
            parameterValue.Draw(spriteBatch);
        }

        public int GetParameter()
        {
            try
            {
                return int.Parse(parameterValue.GetText());
            }
            catch
            { 
                return 1;
            }
        }
    }
}
