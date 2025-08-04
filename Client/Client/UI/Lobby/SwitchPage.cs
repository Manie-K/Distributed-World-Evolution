using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SwitchPage
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<LobbyRow> rows;
        private int pageNumber;
        private int selectedRow;

        public SwitchPage(Texture2D LeftTexture, Texture2D RightTexture, SpriteFont FontNumbers, Vector2 Position, ContentManager ContentManager)
        {
            contentManager = ContentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(LeftTexture, FontNumbers, "", Position, 20, 20, Color.White);
            pageButtons[1] = new Button(RightTexture, FontNumbers, "", new Vector2(Position.X + 200, Position.Y), 20, 20, Color.White);
            pageNumberText = new Text(FontNumbers, "1", true, new Vector2(Position.X + 100, Position.Y), 20, 20);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            rows = new List<LobbyRow>();
            selectedRow = -1;
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (pageButtons[0].CheckLeftClick(clickPosition))
            {
                if (pageNumber > 1) pageNumber--;
            }else if (pageButtons[1].CheckLeftClick(clickPosition))
            {
                pageNumber++;
                if (GetRowsOnPage() == 0) pageNumber--;
            }

            pageNumberText.SetText(pageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
            pageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                rows[(pageNumber - 1) * 6 + i].Draw(spriteBatch);
            }
        }

        public void AddRow(string lobbyName)
        {
            rows.Add(new LobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), lobbyName, "4/6", new Vector2(95, 153 + 68 * (rows.Count % 6)), 760, 65));
        }

        public void UpdateRows(Vector2 cursorPosition, bool ispressed)
        {
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if((pageNumber - 1) * 6 + i != selectedRow)
                {
                    if (rows[(pageNumber - 1) * 6 + i].Update(cursorPosition, false) && ispressed) selectedRow = (pageNumber - 1) * 6 + i;
                }
                else
                {
                    rows[(pageNumber - 1) * 6 + i].Update(cursorPosition, true);
                }
            }
        }

        public int GetRowsOnPage()
        {
            int totalRows = rows.Count;
            int rowsPerPage = 6;
            int startIndex = (pageNumber - 1) * rowsPerPage;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(rowsPerPage, remainingRows);
        }
    }
}
