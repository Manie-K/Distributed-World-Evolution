using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Client.UI.CreateLobby.Parameters
{
    public class SwitchPageParameters
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<ParameterRow> parameters;
        private int pageNumber;
        private int amountOfRows;

        private int lastParameterClicked;

        public SwitchPageParameters(SpriteFont fontNumbers, Vector2 position, ContentManager contentManager, int amountOfRows)
        {
            this.contentManager = contentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, fontNumbers, "", position, 35, 40, Color.White);
            pageButtons[1] = new Button(null, fontNumbers, "", new Vector2(position.X + 183, position.Y), 35, 40, Color.White);
            pageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 91, position.Y), 35, 40);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            parameters = new List<ParameterRow>();
            this.amountOfRows = amountOfRows;
        }

        public bool CheckLeftClick(Vector2 clickPosition)
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

            bool isClicked = false;
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                if (parameters[(pageNumber - 1) * amountOfRows + i].CheckLeftClick(clickPosition))
                {
                    lastParameterClicked = (pageNumber - 1) * amountOfRows + i;
                    isClicked = true;
                }
            }
            return isClicked;
        }

        public void SetParameters(List<ModuleParametersData> moduleParameters)
        {
            parameters.Clear();

            foreach(ModuleParametersData moduleParameter in moduleParameters)
            {
                parameters.Add(new ParameterRow(contentManager, moduleParameter.Name, moduleParameter.Value, moduleParameter.AdditionalDescription, new Vector2(785, 183 + 76 * (parameters.Count % amountOfRows)), moduleParameter.Type));
            }

            pageNumber = 1;
            pageNumberText.SetText(pageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
            pageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                parameters[(pageNumber - 1) * amountOfRows + i].Draw(spriteBatch);
            }
        }

        public int GetRowsOnPage()
        {
            int totalRows = parameters.Count;
            int startIndex = (pageNumber - 1) * amountOfRows;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(amountOfRows, remainingRows);
        }

        public string GetLastDescription()
        {
            return parameters[lastParameterClicked].GetDescription();
        }
    }
}
