using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Client.UI.CreateLobby.Parameters;

namespace Client
{
    public class SwitchPageLobby
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<CreateLobbyRow> creatures;
        private int pageNumber;
        public int selectedRow;
        private int amountOfRows;

        public SwitchPageLobby(SpriteFont FontNumbers, Vector2 Position, ContentManager ContentManager, int AmountOfRows)
        {
            contentManager = ContentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, FontNumbers, "", new Vector2(Position.X, Position.Y + 155), 74, 50, Color.White);
            pageButtons[1] = new Button(null, FontNumbers, "", Position, 74, 50, Color.White);
            pageNumberText = new Text(FontNumbers, "1", true, new Vector2(Position.X, Position.Y + 50), 74, 105);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            creatures = new List<CreateLobbyRow>();
            selectedRow = -1;
            amountOfRows = AmountOfRows;
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (pageButtons[0].CheckLeftClick(clickPosition))
            {
                if (pageNumber > 1) pageNumber--;
            }
            else if (pageButtons[1].CheckLeftClick(clickPosition))
            {
                pageNumber++;
                if (GetRowsOnPage() == 0) pageNumber--;
            }

            pageNumberText.SetText(pageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                creatures[(pageNumber - 1) * amountOfRows + i].Draw(spriteBatch);
            }
        }

        public void AddRow(ModuleData module)
        {
            creatures.Add(new CreateLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply"),
                                             contentManager.Load<Texture2D>("UI/CreateModules/CheckBox/Box_Apply_Check"), contentManager.Load<Texture2D>("UI/CreateLobby/verif_icon"),
                                             module, new Vector2(245, 283 + 57 * (creatures.Count % amountOfRows)), 368, 58));
        }


        public bool UpdateRows(Vector2 cursorPosition, bool ispressed)
        {
            bool isNewSelected = false;
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if ((pageNumber - 1) * amountOfRows + i != selectedRow)
                {
                    if (creatures[(pageNumber - 1) * amountOfRows + i].Update(cursorPosition, ispressed, false) && ispressed)
                    {
                        selectedRow = (pageNumber - 1) * amountOfRows + i;
                        isNewSelected = true;
                    }
                }
                else
                {
                    creatures[(pageNumber - 1) * amountOfRows + i].Update(cursorPosition, ispressed, true);
                }
            }
            return isNewSelected;
        }

        public int GetRowsOnPage()
        {
            int totalRows = creatures.Count;
            int startIndex = (pageNumber - 1) * amountOfRows;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(amountOfRows, remainingRows);
        }

        public ModuleData GetPickedModule()
        {
            return creatures[selectedRow].GetData();
        }
    }
}
