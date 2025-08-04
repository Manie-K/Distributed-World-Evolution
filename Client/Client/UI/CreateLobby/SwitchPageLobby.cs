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

namespace Client
{
    public class SwitchPageLobby
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<CreateLobbyRow> creatures;
        private int pageNumber;
        private int selectedRow;
        private int previousRow;
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
            previousRow = -1;
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

        public ICreatureData GetSelectedCreatureData(int index)
        {
            return creatures[index].GetCreatureData();
        }

        public int GetPreviousRow()
        {
            return previousRow;
        }

        public int GetSelectedRow()
        {
            return selectedRow;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                creatures[(pageNumber - 1) * amountOfRows + i].Draw(spriteBatch);
            }
        }

        public void AddRow(AnimalData animal)
        {
            creatures.Add(new CreateLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              animal,new Vector2(245, 283 + 58 * (creatures.Count % amountOfRows)), 438, 58));
        }

        public void AddRow(PlantData plant)
        {
            creatures.Add(new CreateLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              plant, new Vector2(245, 283 + 58 * (creatures.Count % amountOfRows)), 438, 58));
        }

        public bool UpdateRows(Vector2 cursorPosition, bool ispressed)
        {
            bool isNewSelected = false;
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if ((pageNumber - 1) * amountOfRows + i != selectedRow)
                {
                    if (creatures[(pageNumber - 1) * amountOfRows + i].Update(cursorPosition, false) && ispressed)
                    {
                        previousRow = selectedRow;
                        selectedRow = (pageNumber - 1) * amountOfRows + i;
                        isNewSelected = true;
                    }
                }
                else
                {
                    creatures[(pageNumber - 1) * amountOfRows + i].Update(cursorPosition, true);
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

        public List<ICreatureData> GetCreaturesList()
        {
            List<ICreatureData> creatureDatas = new List<ICreatureData>();

            for(int i=0;i< creatures.Count; i++)
            {
                creatureDatas.Add(creatures[i].GetCreatureData());
            }

            return creatureDatas;
        }
    }
}
