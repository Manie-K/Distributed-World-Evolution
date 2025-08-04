using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client
{
    public class ParametersLobbyRow
    {
        private Text parameterName;
        private TextBox parameterValue;
        private Rectangle bounds;

        public ParametersLobbyRow(SpriteFont Font,string ParameterName, int Value, Vector2 Position, int Width, int Height)
        {
            parameterName = new Text(Font, ParameterName, true, Position, Width, Height);
            parameterName.SetTextColor(Color.White);
            parameterValue = new TextBox(null, Font, new Vector2(Position.X + 174, Position.Y), Width, Height, Color.White, true);
            parameterValue.SetTextColor(Color.White);

            parameterValue.SetText(Value.ToString());

            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void CheckLeftClick(Vector2 cursorPosition)
        {
            parameterValue.CheckLeftClick(cursorPosition);
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
            parameterName.Draw(spriteBatch);
            parameterValue.Draw(spriteBatch);
        }

        public string GetParameterValue()
        {
            return parameterValue.GetText();
        }
    }
}
