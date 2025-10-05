using Client.UI.CreateLobby;
using Client.UI.CreateLobby.Parameters;
using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Client
{
    public class CreateLobbyScene : IScene
    {
        private GameManager manager;
        private SwitchPage lobbyswitchPage; //do zmienienia robienie lobby(na serwer przesylac informacje)

        private Texture2D backGround;
        private Button exitButton;
        private SwitchPageLobby switchPageLobby;
        private Button saveButton;
        private TextBox gameNameBox;
        private Button mapButton;
        private TextBox playerAmountBox;
        private SelectedMapData mapData;
        private ModulesImageDisplay modulesImageDisplay;

        private SwitchPageParameters switchPageParameters;

        public CreateLobbyScene(GameManager manager, SwitchPage lobbyswitchPage)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Create_Lobby");
            switchPageLobby = new SwitchPageLobby(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(155, 295), manager.ContentManager, 4);
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(540, 570), 211, 79, new Color(255, 255, 128));
            mapButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/MapSelectionButton"), null, null, new Vector2(606, 126), 42, 44, Color.Gold);
            gameNameBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(288, 129), 170, 36, Color.White);
            playerAmountBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(523, 129), 50, 36, Color.White, true);
            playerAmountBox.SetText("4");
            mapData = new SelectedMapData();
            modulesImageDisplay = new ModulesImageDisplay(manager.ContentManager);

            this.lobbyswitchPage = lobbyswitchPage;
            this.switchPageParameters = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);

            InitializeCreaturesRows();
        }

        public void Load()
        {

        }


        public void InitializeCreaturesRows()
        {
            switchPageLobby.AddRow(new ModuleData("Boar", 0, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "200", 0), new ModuleParametersData("Damage", "300", 0), new ModuleParametersData("Hunger", "130", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Brown Rabbit", 2, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "800", 0), new ModuleParametersData("Damage", "50", 0), new ModuleParametersData("Hunger", "800", 1), new ModuleParametersData("Consumption", "Herbivore", 1), new ModuleParametersData("Combat", "Enemy HP < X", 1) }));
            switchPageLobby.AddRow(new ModuleData("White Rabbit", 3, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "400", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "120", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Breeding", "Same Species", 1) }));
            switchPageLobby.AddRow(new ModuleData("Red Plant", 4, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "400", 0), new ModuleParametersData("Damage", "150", 0), new ModuleParametersData("Hunger", "130", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Blue Plant", 5, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Herbivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Purple Plant", 6, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Orc", 10, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Carnivore", 1) }));
            switchPageLobby.AddRow(new ModuleData("Blue Orc", 11, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1) }));
            switchPageLobby.AddRow(new ModuleData("Darkgreen Orc", 12, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Pig", 1, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Slime", 7, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Water Slime", 8, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Fire Slime", 9, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1) }));
            switchPageLobby.AddRow(new ModuleData("Vampire", 13, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Attack", "500", 0), new ModuleParametersData("Fertile", "100", 1) }));
            switchPageLobby.AddRow(new ModuleData("Blue Vampire", 14, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Consumption", "Carnivore", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Red Vampire", 15, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Hunger", "100", 1), new ModuleParametersData("Combat", "HP > X", 1) }));
            switchPageLobby.AddRow(new ModuleData("Rose", 1, false, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0), new ModuleParametersData("Fertile", "100", 1) }));
            switchPageLobby.AddRow(new ModuleData("Mushroom", 1, true, new List<ModuleParametersData> { new ModuleParametersData("Health", "100", 0), new ModuleParametersData("Damage", "500", 0) }));
        }

        public void Update(GameTime gameTime)
        {
            bool isPressed = false;

            if (manager.InputManager.CheckIfLeftClick())
            {
                gameNameBox.CheckLeftClick(manager.InputManager.GetMousePosition());
                playerAmountBox.CheckLeftClick(manager.InputManager.GetMousePosition());

                switchPageLobby.CheckLeftClick(manager.InputManager.GetMousePosition());
                switchPageParameters.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    if (!gameNameBox.CheckTextIfEmpty() && playerAmountBox.CheckText(32))
                    {

                        IEnumerable<int> modules = new List<int>() { 0 };
                        CreateLobbyMessage message = new CreateLobbyMessage(gameNameBox.GetText(), int.Parse(playerAmountBox.GetText()), mapData.index, modules);
                        MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                        //LobbyInfoSerialization();
                        lobbyswitchPage.AddRow(gameNameBox.GetText(), mapData.name, $"1/{playerAmountBox.GetText()}");
                        manager.SceneManager.RemoveScene();
                        manager.SceneManager.AddScene(new GameScene(manager));
                    }
                    else
                    {
                        manager.WindowManager.WarningWindow.SetWrongParametersInCreateLobby();
                        manager.WindowManager.EnableWarningWindow();
                    }
                }
                else if (mapButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.AddScene(new MapSelectionScene(manager, ref mapData));
                }

                isPressed = true;
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            exitButton.Update(manager.InputManager.GetMousePosition());
            saveButton.Update(manager.InputManager.GetMousePosition());
            mapButton.Update(manager.InputManager.GetMousePosition());
            gameNameBox.Update();
            playerAmountBox.Update();

            if (switchPageLobby.UpdateRows(manager.InputManager.GetMousePosition(), isPressed))
            {
                switchPageParameters.SetParameters(switchPageLobby.GetPickedModule().ModuleParameters);
                modulesImageDisplay.SetImage(switchPageLobby.GetPickedModule().GraphicIndex);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPageLobby.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            mapButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            gameNameBox.Draw(spriteBatch);
            playerAmountBox.Draw(spriteBatch);
            modulesImageDisplay.Draw(spriteBatch);

            if (switchPageLobby.selectedRow != -1)
            {
                switchPageParameters.Draw(spriteBatch);
            }
        }

        private void LobbyInfoSerialization()
        {
            var json = JsonSerializer.Serialize(new
            {
                LobbyName = gameNameBox.GetText()
            });

            Console.WriteLine(json);
        }
    }
}
