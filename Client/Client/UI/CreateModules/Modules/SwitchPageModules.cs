using Client.UI.CreateModules.Modules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Server.Core;
using System.Collections.Generic;

namespace Client
{
    public class SwitchPageModules
    {
        private ContentManager contentManager;
        private Button[] pageButtons;
        private Text pageNumberText;
        private Text moduleName;
        private List<ModuleData> modules;
        private int pageNumber;

        public SwitchPageModules(SpriteFont fontNumbers, Vector2 position, ContentManager contentManager)
        {
            this.contentManager = contentManager;
            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, null, "", position, 48, 60, Color.White);
            pageButtons[1] = new Button(null, null, "", new Vector2(position.X + 475, position.Y), 48, 60, Color.White);
            pageNumberText = new Text(fontNumbers, "1", true, new Vector2(position.X + 254, position.Y + 188), 20, 20);
            pageNumberText.SetTextColor(Color.Gold);
            pageNumber = 1;
            modules = new List<ModuleData>();

            InitializeRows();

            moduleName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), modules[0].Name, true, new Vector2(193, 450), 368, 0);
            moduleName.SetTextColor(Color.White);
        }

        public void CheckLeftClick(Vector2 clickPosition)
        {
            if (pageButtons[0].CheckLeftClick(clickPosition))
            {
                if (pageNumber > 1) pageNumber--;
            }
            else if (pageButtons[1].CheckLeftClick(clickPosition))
            {
                if (pageNumber < modules.Count) pageNumber++;
            }

            moduleName.SetText(modules[pageNumber - 1].Name);
            pageNumberText.SetText(pageNumber.ToString());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
            pageNumberText.Draw(spriteBatch);
            moduleName.Draw(spriteBatch);
            modules[pageNumber - 1].Draw(spriteBatch);
        }

        public void InitializeRows()
        {
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Boar_Module"), "Boar", EntityTypeEnum.Animal, 3));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Pig_Module"), "Pig", EntityTypeEnum.Animal, 2));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Rabbit_Brown_Module"), "Brown Rabbit", EntityTypeEnum.Animal, 5));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Rabbit_White_Module"), "White Rabbit", EntityTypeEnum.Animal, 4));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant1_Module"), "Red Plant", EntityTypeEnum.Animal, 0));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant2_Module"), "Blue Plant", EntityTypeEnum.Animal, 1));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant3_Module"), "Purple Plant", EntityTypeEnum.Animal, 6));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime1_Module"), "Slime", EntityTypeEnum.Animal, 7));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime2_Module"), "Water Slime", EntityTypeEnum.Animal, 8));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime3_Module"), "Fire Slime", EntityTypeEnum.Animal, 9));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc1_Module"), "Orc", EntityTypeEnum.Animal, 10));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc2_Module"), "Blue Orc", EntityTypeEnum.Animal, 11));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc3_Module"), "Darkgreen Orc", EntityTypeEnum.Animal, 12));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire1_Module"), "Vampire", EntityTypeEnum.Animal, 13));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire2_Module"), "Blue Vampire", EntityTypeEnum.Animal, 14));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire3_Module"), "Red Vampire", EntityTypeEnum.Animal, 15));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Cosmo"), "Cosmo", EntityTypeEnum.Plant, 17));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Daffodil"), "Daffodil", EntityTypeEnum.Plant, 18));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Daisy"), "Daisy", EntityTypeEnum.Plant, 19));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Lavender"), "Lavender", EntityTypeEnum.Plant, 20));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Lily"), "Lily", EntityTypeEnum.Plant, 21));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/LilyOfTheValley"), "Lily of the valley", EntityTypeEnum.Plant, 22));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Orchid"), "Orchid", EntityTypeEnum.Plant, 23));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Pansy"), "Pansy", EntityTypeEnum.Plant, 24));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Poppy"), "Poppy", EntityTypeEnum.Plant, 25));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Rose"), "Rose", EntityTypeEnum.Plant, 26));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Sunflower"), "Sunflower", EntityTypeEnum.Plant, 27));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/PlantsAssets/Tulip"), "Tulip", EntityTypeEnum.Plant, 28));
        }

        public EntityTypeEnum GetActualType()
        {
            return modules[pageNumber - 1].type;
        }

        public int GetGraphicIndex()
        {
            return modules[pageNumber - 1].GraphicIndex;
        }
    }
}
