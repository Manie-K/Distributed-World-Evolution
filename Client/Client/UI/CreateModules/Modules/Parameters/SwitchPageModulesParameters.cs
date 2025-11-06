using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.ModuleDTO;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void AddRow(List<BehaviourDTO> behaviours)
        {
            List<BehaviourDTO> types = [ 
                //new(1, "Human", Server.Core.EntityTypeEnum.Human), // Do we create human modules?
                new(2, "Animal", Server.Core.EntityTypeEnum.Animal, SharedLibrary.DTOs.ModuleDTO.BehaviourInteractionTypeEnum.None),
                new(4, "Plant", Server.Core.EntityTypeEnum.Plant, SharedLibrary.DTOs.ModuleDTO.BehaviourInteractionTypeEnum.None)
            ];

            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Damage", "Number of the damage dealt"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Aggresion", "Number of agression"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Reproduction Need", "The need for breeding"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Max Health", "Max Health"));
            parameters.Add(new ModuleStatsParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), 0, "Max Hunger", "Max Hunger"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), types, 1, "Type", "Type of the creature"));

            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), behaviours.Where(b => b.InteractionType == BehaviourInteractionTypeEnum.Attack).ToList(), 1, "Attack", "Attack behaviour"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), behaviours.Where(b => b.InteractionType == BehaviourInteractionTypeEnum.Eat).ToList(), 1, "Eat", "Eat behaviour"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), behaviours.Where(b => b.InteractionType == BehaviourInteractionTypeEnum.Move).ToList(), 1, "Move", "Move behaviour"));
            parameters.Add(new ModuleBehaviourParameter(contentManager, new Vector2(675, 185 + 76 * (parameters.Count % amountOfRows)), behaviours.Where(b => b.InteractionType == BehaviourInteractionTypeEnum.Reproduce).ToList(), 1, "Reproduce", "Reproduce behaviour"));
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
            return parameters[lastParameterClicked].GetDescription();
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
                    && parameters[i].Type == 1)
                {
                    list.Add(new Tuple<int, int>(i, GetValueOnIndex(i)));
                }
            }

            return list;
        }
    }
}
