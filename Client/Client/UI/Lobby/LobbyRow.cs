using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class LobbyRow
    {
        private Text GameName;
        private Text PlayersText;
        private Rectangle Bounds;

        public LobbyRow(SpriteFont font, string name, string numberOfPlayers, Vector2 position, int width, int height)
        {
            int nameWidth = (int)(width * 0.8f);
            int playersWidth = width - nameWidth;

            GameName = new Text(font, name, true, position, nameWidth, height);
            PlayersText = new Text(font, numberOfPlayers, true,
                new Vector2(position.X + nameWidth, position.Y), playersWidth, height);

            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public bool Update(Vector2 cursorPosition, bool isSelected)
        {
            if (isSelected)
            {
                GameName.SetTextColor(Color.HotPink);
                PlayersText.SetTextColor(Color.HotPink);
            }
            else if (Bounds.Contains(cursorPosition))
            {
                GameName.SetTextColor(Color.Gold);
                PlayersText.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                GameName.SetTextColor(Color.White);
                PlayersText.SetTextColor(Color.White);
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameName.Draw(spriteBatch);
            PlayersText.Draw(spriteBatch);
        }
    }
}