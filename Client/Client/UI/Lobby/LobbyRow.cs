using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class LobbyRow
    {
        public int LobbyID;

        private Text gameName;
        private Text mapName;
        private Text playersText;
        private Rectangle bounds;

        public LobbyRow(SpriteFont font, string name, string mapName, string numberOfPlayers, Vector2 position, int width, int height)
        {
            int nameWidth = (int)(width * 0.33f);
            int mapNameWidth = (int)(width * 0.38f);
            int playersWidth = (int)(width * 0.2f);

            LobbyID = 0;
            gameName = new Text(font, name, true, position, nameWidth, height);
            this.mapName = new Text(font, mapName, true,
                new Vector2(position.X + nameWidth, position.Y), mapNameWidth, height);
            playersText = new Text(font, numberOfPlayers, true,
                new Vector2(position.X + (width - playersWidth), position.Y), playersWidth, height);

            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public bool Update(Vector2 cursorPosition, bool isSelected)
        {
            if (isSelected)
            {
                gameName.SetTextColor(Color.HotPink);
                mapName.SetTextColor(Color.HotPink);
                playersText.SetTextColor(Color.HotPink);
            }
            else if (bounds.Contains(cursorPosition))
            {
                gameName.SetTextColor(Color.Gold);
                mapName.SetTextColor(Color.Gold);
                playersText.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                gameName.SetTextColor(Color.White);
                mapName.SetTextColor(Color.White);
                playersText.SetTextColor(Color.White);
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gameName.Draw(spriteBatch);
            mapName.Draw(spriteBatch);
            playersText.Draw(spriteBatch);
        }
    }
}