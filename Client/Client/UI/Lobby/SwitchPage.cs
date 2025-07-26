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
        private Button[] PageButtons;
        private Text PageNumberText;
        private List<LobbyRow> Rows;
        private int PageNumber;
        private int SelectedRow;

        public SwitchPage(Texture2D LeftTexture, Texture2D RightTexture, SpriteFont fontNumbers, Vector2 position, ContentManager contentManager)
        {
            this.contentManager = contentManager;
            this.PageButtons = new Button[2];
            this.PageButtons[0] = new Button(LeftTexture, fontNumbers, "", position, 20, 20, Color.White);
            this.PageButtons[1] = new Button(RightTexture, fontNumbers, "", new Vector2(position.X + 200, position.Y), 20, 20, Color.White);
            this.PageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 100, position.Y), 20, 20);
            this.PageNumberText.SetTextColor(Color.Gold);
            this.PageNumber = 1;
            this.Rows = new List<LobbyRow>();
            this.SelectedRow = -1;
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (PageButtons[0].CheckLeftClick(clickPosition))
            {
                if (PageNumber > 1) PageNumber--;
            }else if (PageButtons[1].CheckLeftClick(clickPosition))
            {
                PageNumber++;
                if (GetRowsOnPage() == 0) PageNumber--;
            }

            PageNumberText.SetText(PageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PageButtons[0].Draw(spriteBatch);
            PageButtons[1].Draw(spriteBatch);
            PageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                Rows[(PageNumber - 1) * 6 + i].Draw(spriteBatch);
            }
        }

        public void AddRow(string lobbyName)
        {
            Rows.Add(new LobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), lobbyName, "4/6", new Vector2(95, 153 + 68 * (Rows.Count % 6)), 760, 65));
        }

        public void UpdateRows(Vector2 cursorPosition, bool ispressed)
        {
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if((PageNumber - 1) * 6 + i != SelectedRow)
                {
                    if (Rows[(PageNumber - 1) * 6 + i].Update(cursorPosition, false) && ispressed) SelectedRow = (PageNumber - 1) * 6 + i;
                }
                else
                {
                    Rows[(PageNumber - 1) * 6 + i].Update(cursorPosition, true);
                }
            }
        }

        public int GetRowsOnPage()
        {
            int totalRows = Rows.Count;
            int rowsPerPage = 6;
            int startIndex = (PageNumber - 1) * rowsPerPage;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(rowsPerPage, remainingRows);
        }
    }
}
