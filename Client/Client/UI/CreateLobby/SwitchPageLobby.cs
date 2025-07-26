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
        private Button[] PageButtons;
        private Text PageNumberText;
        private List<CreateLobbyRow> Creatures;
        private int PageNumber;
        private int SelectedRow;
        private int PreviousRow;
        private int AmountOfRows;

        public SwitchPageLobby(SpriteFont fontNumbers, Vector2 position, ContentManager contentManager, int amountOfRows)
        {
            this.contentManager = contentManager;
            this.PageButtons = new Button[2];
            this.PageButtons[0] = new Button(null, fontNumbers, "", new Vector2(position.X, position.Y + 155), 74, 50, Color.White);
            this.PageButtons[1] = new Button(null, fontNumbers, "", position, 74, 50, Color.White);
            this.PageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X, position.Y + 50), 74, 105);
            this.PageNumberText.SetTextColor(Color.Gold);
            this.PageNumber = 1;
            this.Creatures = new List<CreateLobbyRow>();
            this.PreviousRow = -1;
            this.SelectedRow = -1;
            AmountOfRows = amountOfRows;
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (PageButtons[0].CheckLeftClick(clickPosition))
            {
                if (PageNumber > 1) PageNumber--;
            }
            else if (PageButtons[1].CheckLeftClick(clickPosition))
            {
                PageNumber++;
                if (GetRowsOnPage() == 0) PageNumber--;
            }

            PageNumberText.SetText(PageNumber.ToString());
        }

        public ICreatureData GetSelectedCreatureData(int index)
        {
            return Creatures[index].GetCreatureData();
        }

        public int GetPreviousRow()
        {
            return PreviousRow;
        }

        public int GetSelectedRow()
        {
            return SelectedRow;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                Creatures[(PageNumber - 1) * AmountOfRows + i].Draw(spriteBatch);
            }
        }

        public void AddRow(AnimalData animal)
        {
            Creatures.Add(new CreateLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              animal,new Vector2(245, 283 + 58 * (Creatures.Count % AmountOfRows)), 438, 58));
        }

        public void AddRow(PlantData plant)
        {
            Creatures.Add(new CreateLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              plant, new Vector2(245, 283 + 58 * (Creatures.Count % AmountOfRows)), 438, 58));
        }

        public bool UpdateRows(Vector2 cursorPosition, bool ispressed)
        {
            bool isNewSelected = false;
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if ((PageNumber - 1) * AmountOfRows + i != SelectedRow)
                {
                    if (Creatures[(PageNumber - 1) * AmountOfRows + i].Update(cursorPosition, false) && ispressed)
                    {
                        PreviousRow = SelectedRow;
                        SelectedRow = (PageNumber - 1) * AmountOfRows + i;
                        isNewSelected = true;
                    }
                }
                else
                {
                    Creatures[(PageNumber - 1) * AmountOfRows + i].Update(cursorPosition, true);
                }
            }
            return isNewSelected;
        }

        public int GetRowsOnPage()
        {
            int totalRows = Creatures.Count;
            int startIndex = (PageNumber - 1) * AmountOfRows;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(AmountOfRows, remainingRows);
        }

        public List<ICreatureData> GetCreaturesList()
        {
            List<ICreatureData> creatureDatas = new List<ICreatureData>();

            for(int i=0;i< Creatures.Count; i++)
            {
                creatureDatas.Add(Creatures[i].GetCreatureData());
            }

            return creatureDatas;
        }
    }
}
