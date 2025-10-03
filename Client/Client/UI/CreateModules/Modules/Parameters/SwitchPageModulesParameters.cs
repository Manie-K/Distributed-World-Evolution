using Client.UI.CreateLobby.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary;
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

        private int lastParameterClicked;

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
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)),0,"Health","statystyka pierwsza jooow1"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Damage", "statystyka pierwsza jooow2"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Hunger", "statystyka pierwsza jooow3"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Carnivore", "Herbivore", "Omnivore" }, 1, "Consumption", "behaviour pierwsza jooow1"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "HP > X", "Enemy HP < X", "Always", "Random" }, 1, "Combat", "behaviour pierwsza jooow2"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Same Species", "Hunger > X and Same Species", "If Reproduction Drive > random_float()" }, 1, "Breeding", "behaviour pierwsza jooow3"));
        }

        public void AddRow2()
        {
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Health", "statystyka pierwsza jooow1"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Hunger", "statystyka pierwsza jooow2"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "HP > X", "Enemy HP < X", "Always", "Random" }, 1, "Combat", "behaviour pierwsza jooow1"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), new List<string> { "Same Species", "Hunger > X and Same Species", "If Reproduction Drive > random_float()" }, 1, "Breeding", "behaviour pierwsza jooow2"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Health", "statystyka pierwsza jooow3"));
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

        public string GetLastDescription()
        {
            return parameters[lastParameterClicked].Description;
        }

        public int GetValueOnIndex(int index)
        {
            return parameters[index].GetValue();
        }

        public List<Tuple<int, int>> GetBehavioursList()
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            
            for(int i=0;i< parameters.Count; i++)
            {
                if (parameters[i] is ModuleBehaviourParameter behaviourParam
                    && parameters[i].Type == 1
                    && behaviourParam.IsPicked())
                {
                    list.Add(new Tuple<int, int>(i, GetValueOnIndex(i)));
                }
            }

            return list;
        }

    }
}
