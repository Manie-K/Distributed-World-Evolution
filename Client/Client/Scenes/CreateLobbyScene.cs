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

        private Texture2D backGround;
        private Button exitButton;
        private SwitchPageLobby switchPageLobby;
        private Button saveButton;
        private TextBox gameNameBox;
        private Button mapButton;
        private TextBox playerAmountBox;
        private SelectedMapData mapData;

        private SwitchPageParameters switchPageParametersAnimals;
        private SwitchPageParameters switchPageParametersPlants;

        private bool isCreatingLobby;
        private bool isJoiningLobby;
        private double timer;

        public CreateLobbyScene(GameManager manager)
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

            this.switchPageParametersAnimals = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);
            this.switchPageParametersPlants = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);

            InitializeCreaturesRows();
            InitalizeParametersPanel();

            isCreatingLobby = false;
            isJoiningLobby = false;
            timer = 0;
        }

        public void Load()
        {

        }

        public void InitializeCreaturesRows()
        {
            switchPageLobby.AddRow(new AnimalData("Boar", 131, 220, 5, 3, 1, 25, true , true, true));
            switchPageLobby.AddRow(new AnimalData("Brown Rabbit",400, 300,45, 3, 10, 35, true, true, true));
            switchPageLobby.AddRow(new AnimalData("White Rabbit", 400, 300, 45, 3, 10, 35, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Red Plant", 100, 400, 35, 3, 13, 65, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Blue Plant", 500, 2500, 5, 23, 12, 4, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Purple Plant", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Orc", 100, 400, 35, 3, 13, 65, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Blue Orc", 500, 2500, 5, 23, 12, 4, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Darkgreen Orc", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Pig", 100, 400, 35, 3, 13, 65, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Slime", 500, 2500, 5, 23, 12, 4, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Water Slime", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Fire Slime", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Vampire", 500, 2500, 5, 23, 12, 4, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Blue Vampire", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new AnimalData("Red Vampire", 150, 260, 5, 13, 155, 25, true, true, true));
            switchPageLobby.AddRow(new PlantData("Rose", 222, 10, 20, true, true, true));
            switchPageLobby.AddRow(new PlantData("Mushroom", 265, 105, 20, true, true, true));
        }

        public void InitalizeParametersPanel()
        {
            switchPageParametersAnimals.AddRowAnimal();
            switchPageParametersPlants.AddRowPlant();
        }

        public void Update(GameTime gameTime)
        {
            if (isCreatingLobby)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0.1) return;

                if (manager.ClientManager.LobbyCreated == ActionStatus.SUCCESS)
                {
                    isJoiningLobby = true;
                    isCreatingLobby = false;
                    manager.ClientManager.LobbyCreated = ActionStatus.WAITING;
                }
                else if (manager.ClientManager.LobbyCreated == ActionStatus.FAILED)
                {
                    isCreatingLobby = false;
                    manager.ClientManager.LobbyCreated = ActionStatus.WAITING;
                }

                timer = 0;
                return;
            }
            else if (isJoiningLobby)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0.1) return;

                if (manager.ClientManager.LobbyJoined == ActionStatus.SUCCESS)
                {
                    manager.SceneManager.RemoveScene();
                    manager.SceneManager.AddScene(new GameScene(manager));
                }
                else if (manager.ClientManager.LobbyJoined == ActionStatus.FAILED)
                {
                    isJoiningLobby = false;
                    manager.ClientManager.LobbyJoined = ActionStatus.WAITING;
                }

                timer = 0;
                return;
            }

            bool isPressed = false;

            if (manager.InputManager.CheckIfLeftClick())
            {
                gameNameBox.CheckLeftClick(manager.InputManager.GetMousePosition());
                playerAmountBox.CheckLeftClick(manager.InputManager.GetMousePosition());

                switchPageLobby.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    if (!gameNameBox.CheckTextIfEmpty() && playerAmountBox.CheckText(32))
                    {
                        if (switchPageLobby.GetSelectedRow() != -1)
                        {
                            ICreatureData tmp = switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow());
                            if (tmp.Type == CreatureType.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData)tmp);
                            else if (tmp.Type == CreatureType.Plant) switchPageParametersPlants.SavePlantParameters((PlantData)tmp);
                        }

                        IEnumerable<int> modules = new List<int>() {};
                        CreateLobbyMessage message = new CreateLobbyMessage(gameNameBox.GetText(), int.Parse(playerAmountBox.GetText()), mapData.index, modules);
                        _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                        isCreatingLobby = true;
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

                if (switchPageLobby.GetSelectedRow() != -1)
                {
                    if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Animal) switchPageParametersAnimals.CheckLeftClick(manager.InputManager.GetMousePosition());
                    else if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Plant) switchPageParametersPlants.CheckLeftClick(manager.InputManager.GetMousePosition());
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

            if (switchPageLobby.GetSelectedRow() != -1)
            {
                if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Animal) switchPageParametersAnimals.UpdateRows(manager.InputManager.GetMousePosition());
                else if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Plant) switchPageParametersPlants.UpdateRows(manager.InputManager.GetMousePosition());
            }

            if (switchPageLobby.UpdateRows(manager.InputManager.GetMousePosition(), isPressed))
            {
                if (switchPageLobby.GetPreviousRow() != -1)
                {
                    ICreatureData tmp = switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetPreviousRow());
                    if (tmp.Type == CreatureType.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData) tmp);
                    else if (tmp.Type == CreatureType.Plant) switchPageParametersPlants.SavePlantParameters((PlantData) tmp);
                }
                SetParameters();
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

            if (switchPageLobby.GetSelectedRow() != -1)
            {
                if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Animal) 
                    switchPageParametersAnimals.Draw(spriteBatch);
                else if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Plant) 
                    switchPageParametersPlants.Draw(spriteBatch);
            }
        }

        private void SetParameters()
        {
            if (switchPageLobby.GetSelectedRow() != -1)
            {
                ICreatureData CreatureSelected = switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow());
                
                if (CreatureSelected.Type == CreatureType.Animal) switchPageParametersAnimals.SetParametersCreatureData((AnimalData) CreatureSelected);               
                else if (CreatureSelected.Type == CreatureType.Plant) switchPageParametersPlants.SetParametersCreatureData((PlantData) CreatureSelected);
                
            }
        }

        private void LobbyInfoSerialization()
        {
            var json = JsonSerializer.Serialize(new
            {
                LobbyName = gameNameBox.GetText(),
                Creatures = switchPageLobby.GetCreaturesList()
            });

            Console.WriteLine(json);
        }
    }
}
