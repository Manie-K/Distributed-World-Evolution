using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
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

        private SwitchPageParameters switchPageParametersAnimals;
        private SwitchPageParameters switchPageParametersPlants;

        private MouseState previousMouseState;

        public CreateLobbyScene(GameManager manager, SwitchPage lobbyswitchPage)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Create_Lobby");
            switchPageLobby = new SwitchPageLobby(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(155, 295), manager.ContentManager, 4);
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(540, 570), 211, 79, new Color(255, 255, 128));
            gameNameBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(340, 135), 280, 22, Color.White);
            this.lobbyswitchPage = lobbyswitchPage;
            this.switchPageParametersAnimals = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);
            this.switchPageParametersPlants = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);

            InitializeCreaturesRows();
            InitalizeParametersPanel();
        }

        public void Load()
        {

        }

        public void InitializeCreaturesRows()
        {
            switchPageLobby.AddRow(new AnimalData("wolf", 131, 220, 5, 3, 1, 25));
            switchPageLobby.AddRow(new AnimalData("eagle",400, 300,45, 3, 10, 35));
            switchPageLobby.AddRow(new AnimalData("mouse", 100, 400, 35, 3, 13, 65));
            switchPageLobby.AddRow(new AnimalData("rat", 500, 2500, 5, 23, 12, 4));
            switchPageLobby.AddRow(new AnimalData("cat", 150, 260, 5, 13, 155, 25));
            switchPageLobby.AddRow(new PlantData("rose", 222, 10));
            switchPageLobby.AddRow(new PlantData("mushroom", 265, 105));
        }

        public void InitalizeParametersPanel()
        {
            switchPageParametersAnimals.AddRowAnimal();
            switchPageParametersPlants.AddRowPlant();
        }



        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);
            bool isPressed = false;

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                switchPageLobby.CheckLeftClick(position);
                if (exitButton.CheckLeftClick(position))
                {
                    manager.SceneManager.RemoveScene();
                }
                if (saveButton.CheckLeftClick(position) && !gameNameBox.CheckTextIfEmpty())
                {
                    if (switchPageLobby.GetSelectedRow() != -1)
                    {
                        ICreatureData tmp = switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow());
                        if (tmp.Type == CreatureType.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData)tmp);
                        else if (tmp.Type == CreatureType.Plant) switchPageParametersPlants.SavePlantParameters((PlantData)tmp);
                    }

                    LobbyInfoSerialization();
                    lobbyswitchPage.AddRow(gameNameBox.GetText());
                    manager.SceneManager.RemoveScene();
                }

                if (switchPageLobby.GetSelectedRow() != -1)
                {
                    if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Animal) switchPageParametersAnimals.CheckLeftClick(position);
                    else if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Plant) switchPageParametersPlants.CheckLeftClick(position);
                }

                gameNameBox.CheckLeftClick(position);

                isPressed = true;
            }

            exitButton.Update(position);
            saveButton.Update(position);
            gameNameBox.Update();

            if (switchPageLobby.GetSelectedRow() != -1)
            {
                if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Animal) switchPageParametersAnimals.UpdateRows();
                else if (switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetSelectedRow()).Type == CreatureType.Plant) switchPageParametersPlants.UpdateRows();
            }


            if (switchPageLobby.UpdateRows(position, isPressed))
            {
                if (switchPageLobby.GetPreviousRow() != -1)
                {
                    ICreatureData tmp = switchPageLobby.GetSelectedCreatureData(switchPageLobby.GetPreviousRow());
                    if (tmp.Type == CreatureType.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData) tmp);
                    else if (tmp.Type == CreatureType.Plant) switchPageParametersPlants.SavePlantParameters((PlantData) tmp);
                }
                SetParameters();
            }

            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPageLobby.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            gameNameBox.Draw(spriteBatch);

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

            Debug.WriteLine(json);
        }

    }
}
