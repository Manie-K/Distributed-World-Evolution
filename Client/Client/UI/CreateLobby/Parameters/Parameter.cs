using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client.UI.CreateLobby.Parameters
{
    public abstract class Parameter
    {
        protected Text parameterName;

        public Parameter(SpriteFont font, string name, Vector2 position, int width, int height)
        {
            parameterName = new Text(font, name, true, position, width, height);
            parameterName.SetTextColor(Color.White);
        }

        public abstract void CheckLeftClick(Vector2 cursorPosition);

        public abstract void Update(Vector2 cursorPosition);

        public abstract void SetParameter(string parameter);
        public abstract void SetParameter(bool parameter);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract string GetStringParameterValue();
        public abstract bool GetBoolParameterValue();
    }
}
