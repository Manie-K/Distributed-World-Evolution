using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client
{
    public class ParametersLobbyRow
    {
        private Text ParameterName;
        private TextBox ParameterValue;
        private Rectangle Bounds;

        public ParametersLobbyRow(SpriteFont font,string ParameterName, int value, Vector2 position, int width, int height)
        {
            this.ParameterName = new Text(font, ParameterName, true, position, width, height);
            this.ParameterName.SetTextColor(Color.White);
            this.ParameterValue = new TextBox(null, font, new Vector2(position.X + 174, position.Y), width, height, Color.White, true);
            this.ParameterValue.SetTextColor(Color.White);

            this.ParameterValue.SetText(value.ToString());

            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public void CheckLeftClick(Vector2 cursorPosition)
        {
            ParameterValue.CheckLeftClick(cursorPosition);
        }

        public void Update()
        {
            ParameterValue.Update();
        }

        public void SetParameter(string parameter)
        {
            ParameterValue.SetText(parameter);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ParameterName.Draw(spriteBatch);
            ParameterValue.Draw(spriteBatch);
        }

        public string GetParameterValue()
        {
            return ParameterValue.GetText();
        }
    }
}
