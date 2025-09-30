using Client.UI.CreateLobby.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class SwitchPageModulesParameters
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<ModuleParameter> parameters;
        private int pageNumber;
        private int amountOfRows;

        public SwitchPageModulesParameters(SpriteFont fontNumbers, Vector2 position, ContentManager contentManager, int amountOfRows)
        {
            this.contentManager = contentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, fontNumbers, "", position, 37, 43, Color.White);
            pageButtons[1] = new Button(null, fontNumbers, "", new Vector2(position.X + 218, position.Y), 37, 43, Color.White);
            pageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 128, position.Y), 0, 43);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            parameters = new List<ModuleParameter>();
            this.amountOfRows = amountOfRows;
        }

        public void AddRow()
        {
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)),0,"Health","statystyka pierwsza jooow"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Damage", "statystyka pierwsza jooow"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Hunger", "statystyka pierwsza jooow"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Ala", "ma", "kota" }, 0, "Aggresion", "behaviour pierwsza jooow"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Kot", "ma", "Ale" }, 0, "Movement", "behaviour pierwsza jooow"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Aggression", "nie ma", "psa" }, 0, "Attack", "behaviour pierwsza jooow"));
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

            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                parameters[(pageNumber - 1) * amountOfRows + i].CheckLeftClick(clickPosition);
            }
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


        public void UpdateRows(Vector2 cursorPosition)
        {
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                parameters[(pageNumber - 1) * amountOfRows + i].Update(cursorPosition);
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



    }
}
