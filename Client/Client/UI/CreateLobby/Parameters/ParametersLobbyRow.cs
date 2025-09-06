using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client.UI.CreateLobby.Parameters
{
    public class ParametersLobbyRow : Parameter
    {
        private TextBox parameterValue;

        public ParametersLobbyRow(SpriteFont font, string name, int value, Vector2 position, int width, int height) : base(font, name, position, width, height)
        {
            parameterValue = new TextBox(null, font, new Vector2(position.X + 174, position.Y), width, height, Color.White, true);
            parameterValue.SetTextColor(Color.White);

            parameterValue.SetText(value.ToString());
        }

        public override void CheckLeftClick(Vector2 cursorPosition) 
        {
            parameterValue.CheckLeftClick(cursorPosition);
        }

        public override void Update(Vector2 cursorPosition)
        {
            parameterValue.Update();
        }

        public override void SetParameter(string parameter)
        {
            parameterValue.SetText(parameter);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            parameterName.Draw(spriteBatch);
            parameterValue.Draw(spriteBatch);
        }

        public override string GetStringParameterValue()
        {
            return parameterValue.GetText();
        }





        public override bool GetBoolParameterValue()
        {
            return false;
        }

        public override void SetParameter(bool val)
        {

        }
    }
}
