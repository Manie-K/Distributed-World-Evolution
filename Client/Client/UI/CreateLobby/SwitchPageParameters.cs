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
    public class SwitchPageParameters
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<ParametersLobbyRow> parameters;
        private int pageNumber;
        private int amountOfRows;

        public SwitchPageParameters( SpriteFont FontNumbers, Vector2 Position, ContentManager ContentManager, int AmountOfRows)
        {
            contentManager = ContentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, FontNumbers, "", Position, 35, 40, Color.White);
            pageButtons[1] = new Button(null, FontNumbers, "", new Vector2(Position.X + 183, Position.Y), 35, 40, Color.White);
            pageNumberText = new Text(FontNumbers, "1", true, new Vector2(Position.X + 91, Position.Y ), 35, 40);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            parameters = new List<ParametersLobbyRow>();
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

            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                parameters[(pageNumber - 1) * amountOfRows + i].CheckLeftClick(clickPosition);
            }
        }


        public void SetParametersCreatureData(AnimalData animal)
        {
            parameters[0].SetParameter(animal.Health.ToString());
            parameters[1].SetParameter(animal.Damage.ToString());
            parameters[2].SetParameter(animal.Speed.ToString());
            parameters[3].SetParameter(animal.AttackRange.ToString());
            parameters[4].SetParameter(animal.Defense.ToString());
            parameters[5].SetParameter(animal.ReproduceCooldown.ToString());
        }
        public void SetParametersCreatureData(PlantData Plant)
        {
            parameters[0].SetParameter(Plant.ReproduceCooldown.ToString());
            parameters[1].SetParameter(Plant.ToxicityDamage.ToString());

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

        public void AddRowPlant()
        {
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              "Reproduce Cooldown", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Toxicity Damage", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
        }

        public void AddRowAnimal()
        {
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Health", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Damage", 110, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Speed", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                            "Attack Range", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                      "Defense", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Reproduce Cooldown", 0, new Vector2(675, 180 + 74 * (parameters.Count % amountOfRows)), 438, 58));
        }

        public void UpdateRows()
        {
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                parameters[(pageNumber - 1) * amountOfRows + i].Update();
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

        public void SavePlantParameters(PlantData plant)
        {
            plant.ReproduceCooldown = int.Parse(parameters[0].GetParameterValue());
            plant.ToxicityDamage = int.Parse(parameters[1].GetParameterValue());
        }

        public void SaveAnimalParameters(AnimalData animal)
        {
            animal.Health = int.Parse(parameters[0].GetParameterValue());
            animal.Damage = int.Parse(parameters[1].GetParameterValue());
            animal.Speed = int.Parse(parameters[2].GetParameterValue());
            animal.AttackRange = int.Parse(parameters[3].GetParameterValue());
            animal.Defense = int.Parse(parameters[4].GetParameterValue());
            animal.ReproduceCooldown = int.Parse(parameters[5].GetParameterValue());

        }
    }
}
