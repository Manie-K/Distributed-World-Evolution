using Client.UI.CreateModules.Modules;
using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
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
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Boar_Module"), "Boar", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Rabbit_Brown_Module"), "Brown Rabbit", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Rabbit_White_Module"), "White Rabbit", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant1_Module"), "Red Plant", ModuleType.Animal));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Pig_Module"), "Pig", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant3_Module"), "Purple Plant", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc1_Module"), "Orc", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc2_Module"), "Blue Orc", ModuleType.Animal));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Orc3_Module"), "Darkgreen Orc", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Plant2_Module"), "Blue Plant", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime1_Module"), "Slime", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime2_Module"), "Water Slime", ModuleType.Animal));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Slime3_Module"), "Fire Slime", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire1_Module"), "Vampire", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire2_Module"), "Blue Vampire", ModuleType.Animal));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/CreateModules/Vampire3_Module"), "Red Vampire", ModuleType.Animal));

            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/BG_Settings"), "Rose", ModuleType.Plant));
            modules.Add(new ModuleData(contentManager.Load<Texture2D>("UI/BG_Forest"), "Mushroom", ModuleType.Plant));
        }

        public ModuleType GetAcctualType()
        {
            return modules[pageNumber - 1].type;
        }

        public int GetIndex()
        {
            return pageNumber - 1;
        }
    }
}
