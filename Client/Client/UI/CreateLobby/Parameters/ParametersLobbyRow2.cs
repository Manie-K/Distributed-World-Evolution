using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client.UI.CreateLobby.Parameters
{
    public class ParametersLobbyRow2 : Parameter
    {
        private CheckBox parameterValue;

        public ParametersLobbyRow2(SpriteFont font, Texture2D background, string name, Vector2 position, int width, int height) : base(font, name, position, width, height)
        {
            parameterValue = new CheckBox(background, new Vector2(position.X + 174, position.Y), width, height, Color.Gold);
        }

        public override void CheckLeftClick(Vector2 cursorPosition)
        {
            parameterValue.CheckLeftClick(cursorPosition);
        }

        public override void Update(Vector2 cursorPosition)
        {
            parameterValue.Update(cursorPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            parameterName.Draw(spriteBatch);
            parameterValue.Draw(spriteBatch);
        }

        public override bool GetBoolParameterValue()
        {
            return parameterValue.GetValue();
        }

        public override void SetParameter(bool parameter)
        {
            parameterValue.SetValue(parameter);
        }



        public override string GetStringParameterValue()
        {
            return null;
        }

        public override void SetParameter(string parameter)
        {
        }
    }
}
