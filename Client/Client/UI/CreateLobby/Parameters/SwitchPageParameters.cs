using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateLobby.Parameters
{
    public class SwitchPageParameters
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private List<Parameter> parameters;
        private int pageNumber;
        private int amountOfRows;

        public SwitchPageParameters(SpriteFont fontNumbers, Vector2 position, ContentManager contentManager, int amountOfRows)
        {
            this.contentManager = contentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, fontNumbers, "", position, 35, 40, Color.White);
            pageButtons[1] = new Button(null, fontNumbers, "", new Vector2(position.X + 183, position.Y), 35, 40, Color.White);
            pageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 91, position.Y), 35, 40);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            parameters = new List<Parameter>();
            this.amountOfRows = amountOfRows;
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
            parameters[1].SetParameter(animal.AttackPower.ToString());
            parameters[2].SetParameter(animal.MoveSpeed.ToString());
            parameters[3].SetParameter(animal.AttackRange.ToString());
            parameters[4].SetParameter(animal.Armor.ToString());
            parameters[5].SetParameter(animal.BreedingCooldown.ToString());
            parameters[6].SetParameter(animal.IsAggressive);
            parameters[7].SetParameter(animal.IsFertile);
            parameters[8].SetParameter(animal.IsTamable);
        }
        public void SetParametersCreatureData(PlantData Plant)
        {
            parameters[0].SetParameter(Plant.BreedingCooldown.ToString());
            parameters[1].SetParameter(Plant.ToxicityDamage.ToString());
            parameters[2].SetParameter(Plant.ToxicityDamage.ToString());
            parameters[3].SetParameter(Plant.IsFertile);
            parameters[4].SetParameter(Plant.IsToxicOnContact);
            parameters[5].SetParameter(Plant.IsToxicWhenEaten);
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
                                              "Reproduce Cooldown", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Thorns Damage", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                      "Poisonous Damage", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                      "Fertile", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                        "Thorns", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                         "Poisonous", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
        }

        public void AddRowAnimal()
        {
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Health", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Attack power", 110, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Move speed", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                            "Attack range", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                      "Armor", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Breeding cooldown", 0, new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                      "Aggressive", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                      "Fertile", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
            parameters.Add(new ParametersLobbyRow2(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), contentManager.Load<Texture2D>("UI/White_Check"),
                      "Tamable", new Vector2(850, 180 + 76 * (parameters.Count % amountOfRows)), 88, 53));
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

        public void SavePlantParameters(PlantData plant)
        {
            plant.BreedingCooldown = int.Parse(parameters[0].GetStringParameterValue());
            plant.ContactDamage = int.Parse(parameters[1].GetStringParameterValue());
            plant.ToxicityDamage = int.Parse(parameters[2].GetStringParameterValue());
            plant.IsFertile = parameters[3].GetBoolParameterValue();
            plant.IsToxicOnContact = parameters[4].GetBoolParameterValue();
            plant.IsToxicWhenEaten = parameters[5].GetBoolParameterValue();
        }

        public void SaveAnimalParameters(AnimalData animal)
        {
            animal.Health = int.Parse(parameters[0].GetStringParameterValue());
            animal.AttackPower = int.Parse(parameters[1].GetStringParameterValue());
            animal.MoveSpeed = int.Parse(parameters[2].GetStringParameterValue());
            animal.AttackRange = int.Parse(parameters[3].GetStringParameterValue());
            animal.Armor = int.Parse(parameters[4].GetStringParameterValue());
            animal.BreedingCooldown = int.Parse(parameters[5].GetStringParameterValue());
            animal.IsAggressive = parameters[6].GetBoolParameterValue();
            animal.IsFertile = parameters[7].GetBoolParameterValue();
            animal.IsTamable = parameters[8].GetBoolParameterValue();
        }
    }
}
