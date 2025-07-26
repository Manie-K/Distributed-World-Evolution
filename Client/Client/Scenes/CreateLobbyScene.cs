using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;

namespace Client
{
    public class CreateLobbyScene : IScene
    {
        private ContentManager contentManager;
        private SceneManager sceneManager;
        private SwitchPage LobbyswitchPage; //do zmienienia robienie lobby(na serwer przesylac informacje)

        private Texture2D BackGround;
        private Button ExitButton;
        private SwitchPageLobby SwitchPageLobby;
        private Button SaveButton;
        private TextBox GameNameBox;

        private SwitchPageParameters switchPageParametersAnimals;
        private SwitchPageParameters switchPageParametersPlants;

        private MouseState previousMouseState;

        public CreateLobbyScene(ContentManager contentManager, SceneManager sceneManager, SwitchPage LobbyswitchPage)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.ExitButton = new Button(contentManager.Load<Texture2D>("UI/White Close 2"), contentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            this.BackGround = contentManager.Load<Texture2D>("UI/Scenes/Create_Lobby");
            this.SwitchPageLobby = new SwitchPageLobby( contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(155, 295), contentManager, 4);
            this.SaveButton = new Button(contentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(540, 570), 211, 79, new Color(255, 255, 128));
            this.GameNameBox = new TextBox(null, contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(340, 135), 280, 22, Color.White);
            this.LobbyswitchPage = LobbyswitchPage;
            this.switchPageParametersAnimals = new SwitchPageParameters(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), contentManager, 4);
            this.switchPageParametersPlants = new SwitchPageParameters(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), contentManager, 4);

            InitializeCreaturesRows();
            InitalizeParametersPanel();
        }

        public void Load()
        {

        }

        public void InitializeCreaturesRows()
        {
            SwitchPageLobby.AddRow(new AnimalData("wolf", 131, 220, 5, 3, 1, 25));
            SwitchPageLobby.AddRow(new AnimalData("eagle",400, 300,45, 3, 10, 35));
            SwitchPageLobby.AddRow(new AnimalData("mouse", 100, 400, 35, 3, 13, 65));
            SwitchPageLobby.AddRow(new AnimalData("rat", 500, 2500, 5, 23, 12, 4));
            SwitchPageLobby.AddRow(new AnimalData("cat", 150, 260, 5, 13, 155, 25));
            SwitchPageLobby.AddRow(new PlantData("rose", 222, 10));
            SwitchPageLobby.AddRow(new PlantData("mushroom", 265, 105));
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
                SwitchPageLobby.CheckLeftClick(position);
                if (ExitButton.CheckLeftClick(position))
                {
                    sceneManager.RemoveScene();
                }
                if (SaveButton.CheckLeftClick(position) && !GameNameBox.CheckTextIfEmpty())
                {
                    if (SwitchPageLobby.GetSelectedRow() != -1)
                    {
                        ICreatureData tmp = SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow());
                        if (tmp.type == Type.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData)tmp);
                        else if (tmp.type == Type.Plant) switchPageParametersPlants.SavePlantParameters((PlantData)tmp);
                    }

                    LobbyInfoSerialization();
                    LobbyswitchPage.AddRow(GameNameBox.GetText());
                    sceneManager.RemoveScene();
                }

                if (SwitchPageLobby.GetSelectedRow() != -1)
                {
                    if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Animal) switchPageParametersAnimals.CheckLeftClick(position);
                    else if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Plant) switchPageParametersPlants.CheckLeftClick(position);
                }

                GameNameBox.CheckLeftClick(position);

                isPressed = true;
            }

            ExitButton.Update(position);
            SaveButton.Update(position);
            GameNameBox.Update();

            if (SwitchPageLobby.GetSelectedRow() != -1)
            {
                if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Animal) switchPageParametersAnimals.UpdateRows();
                else if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Plant) switchPageParametersPlants.UpdateRows();
            }


            if (SwitchPageLobby.UpdateRows(position, isPressed))
            {
                if (SwitchPageLobby.GetPreviousRow() != -1)
                {
                    ICreatureData tmp = SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetPreviousRow());
                    if (tmp.type == Type.Animal) switchPageParametersAnimals.SaveAnimalParameters((AnimalData) tmp);
                    else if (tmp.type == Type.Plant) switchPageParametersPlants.SavePlantParameters((PlantData) tmp);
                }
                SetParameters();
            }

            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            SwitchPageLobby.Draw(spriteBatch);
            SaveButton.Draw(spriteBatch);
            ExitButton.Draw(spriteBatch);
            GameNameBox.Draw(spriteBatch);


            if (SwitchPageLobby.GetSelectedRow() != -1)
            {
                if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Animal) switchPageParametersAnimals.Draw(spriteBatch);
                else if (SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow()).type == Type.Plant) switchPageParametersPlants.Draw(spriteBatch);
            }


        }


        private void SetParameters()
        {
            if (SwitchPageLobby.GetSelectedRow() != -1)
            {
                ICreatureData CreatureSelected = SwitchPageLobby.GetSelectedCreatureData(SwitchPageLobby.GetSelectedRow());
                
                if (CreatureSelected.type == Type.Animal) switchPageParametersAnimals.SetParametersCreatureData((AnimalData) CreatureSelected);               
                else if (CreatureSelected.type == Type.Plant) switchPageParametersPlants.SetParametersCreatureData((PlantData) CreatureSelected);
                
            }
        }

        private void LobbyInfoSerialization()
        {
            var json = JsonSerializer.Serialize(new
            {
                LobbyName = GameNameBox.GetText(),
                Creatures = SwitchPageLobby.GetCreaturesList()
            });

            Debug.WriteLine(json);
        }

    }
}
