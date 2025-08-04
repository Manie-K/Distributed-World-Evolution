using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class LobbyRow
    {
        private Text gameName;
        private Text playersText;
        private Rectangle bounds;

        public LobbyRow(SpriteFont Font, string Name, string NumberOfPlayers, Vector2 Position, int Width, int Height)
        {
            int nameWidth = (int)(Width * 0.8f);
            int playersWidth = Width - nameWidth;

            gameName = new Text(Font, Name, true, Position, nameWidth, Height);
            playersText = new Text(Font, NumberOfPlayers, true,
                new Vector2(Position.X + nameWidth, Position.Y), playersWidth, Height);

            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public bool Update(Vector2 cursorPosition, bool isSelected)
        {
            if (isSelected)
            {
                gameName.SetTextColor(Color.HotPink);
                playersText.SetTextColor(Color.HotPink);
            }
            else if (bounds.Contains(cursorPosition))
            {
                gameName.SetTextColor(Color.Gold);
                playersText.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                gameName.SetTextColor(Color.White);
                playersText.SetTextColor(Color.White);
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gameName.Draw(spriteBatch);
            playersText.Draw(spriteBatch);
        }
    }
}