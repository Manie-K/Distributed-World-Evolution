using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.Settings
{
    public class SwitchPageSettings
    {
        private Button[] pageButtons;
        private Text pageNumberText;
        private readonly int maxPageNumber;

        public int PageNumber {  get; private set; }

        public SwitchPageSettings(Texture2D LeftTexture, Texture2D RightTexture, SpriteFont FontNumbers, Vector2 Position)
        {
            pageButtons = new Button[2];
            pageButtons[0] = new Button(LeftTexture, FontNumbers, "", new Vector2(Position.X, Position.Y - 25), 70, 70, Color.White);
            pageButtons[1] = new Button(RightTexture, FontNumbers, "", new Vector2(Position.X + 370, Position.Y - 25), 70, 70, Color.White);
            pageNumberText = new Text(FontNumbers, "1", true, new Vector2(Position.X + 200, Position.Y), 30, 30);
            pageNumberText.SetTextColor(Color.Gold);
            PageNumber = 1;
            maxPageNumber = 2;
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (pageButtons[0].CheckLeftClick(clickPosition))
            {
                if (PageNumber > 1) PageNumber--;
            }
            else if (pageButtons[1].CheckLeftClick(clickPosition))
            {
                if (PageNumber < maxPageNumber) PageNumber++;
            }

            pageNumberText.SetText(PageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
            pageNumberText.Draw(spriteBatch);
        }
    }
}
