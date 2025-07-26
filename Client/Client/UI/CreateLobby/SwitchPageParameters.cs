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
        private Button[] PageButtons;
        private Text PageNumberText;
        private List<ParametersLobbyRow> Parameters;
        private int PageNumber;
        private int AmountOfRows;

        public SwitchPageParameters( SpriteFont fontNumbers, Vector2 position, ContentManager contentManager, int amountOfRows)
        {
            this.contentManager = contentManager;
            this.PageButtons = new Button[2];
            this.PageButtons[0] = new Button(null, fontNumbers, "", position, 35, 40, Color.White);
            this.PageButtons[1] = new Button(null, fontNumbers, "", new Vector2(position.X + 183, position.Y), 35, 40, Color.White);
            this.PageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 91, position.Y ), 35, 40);
            this.PageNumberText.SetTextColor(Color.Gold);
            this.PageNumber = 1;
            this.Parameters = new List<ParametersLobbyRow>();
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

            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                Parameters[(PageNumber - 1) * AmountOfRows + i].CheckLeftClick(clickPosition);
            }
        }


        public void SetParametersCreatureData(AnimalData animal)
        {
            Parameters[0].SetParameter(animal.health.ToString());
            Parameters[1].SetParameter(animal.damage.ToString());
            Parameters[2].SetParameter(animal.speed.ToString());
            Parameters[3].SetParameter(animal.attackRange.ToString());
            Parameters[4].SetParameter(animal.Defense.ToString());
            Parameters[5].SetParameter(animal.ReproduceCooldown.ToString());
        }
        public void SetParametersCreatureData(PlantData Plant)
        {
            Parameters[0].SetParameter(Plant.ReproduceCooldown.ToString());
            Parameters[1].SetParameter(Plant.ToxicityDamage.ToString());

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            PageButtons[0].Draw(spriteBatch);
            PageButtons[1].Draw(spriteBatch);
            PageNumberText.Draw(spriteBatch);
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                Parameters[(PageNumber - 1) * AmountOfRows + i].Draw(spriteBatch);
            }
        }

        public void AddRowPlant()
        {
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                              "Reproduce Cooldown", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Toxicity Damage", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
        }

        public void AddRowAnimal()
        {
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Health", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Damage", 110, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Speed", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                            "Attack Range", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                      "Defense", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
            Parameters.Add(new ParametersLobbyRow(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                  "Reproduce Cooldown", 0, new Vector2(675, 180 + 74 * (Parameters.Count % AmountOfRows)), 438, 58));
        }

        public void UpdateRows()
        {
            for (int i = 0; i < GetRowsOnPage(); i++)
            {
                Parameters[(PageNumber - 1) * AmountOfRows + i].Update();
            }

        }

        public int GetRowsOnPage()
        {
            int totalRows = Parameters.Count;
            int startIndex = (PageNumber - 1) * AmountOfRows;

            if (startIndex >= totalRows)
                return 0;

            int remainingRows = totalRows - startIndex;
            return Math.Min(AmountOfRows, remainingRows);
        }

        public void SavePlantParameters(PlantData plant)
        {
            plant.ReproduceCooldown = int.Parse(Parameters[0].GetParameterValue());
            plant.ToxicityDamage = int.Parse(Parameters[1].GetParameterValue());
        }

        public void SaveAnimalParameters(AnimalData animal)
        {
            animal.health = int.Parse(Parameters[0].GetParameterValue());
            animal.damage = int.Parse(Parameters[1].GetParameterValue());
            animal.speed = int.Parse(Parameters[2].GetParameterValue());
            animal.attackRange = int.Parse(Parameters[3].GetParameterValue());
            animal.Defense = int.Parse(Parameters[4].GetParameterValue());
            animal.ReproduceCooldown = int.Parse(Parameters[5].GetParameterValue());

        }
    }
}
