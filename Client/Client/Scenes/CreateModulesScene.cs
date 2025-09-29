using Client.UI.CreateLobby.Parameters;
using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client
{
    public class CreateModulesScene : IScene
    {
        private GameManager manager;

        private Texture2D backGround;
        private SwitchPageModules switchPageModules;
        private Button exitButton;
        private Button saveButton;



        public CreateModulesScene(GameManager manager)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Create_Module");
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(540, 570), 211, 79, new Color(255, 255, 128));
            switchPageModules = new SwitchPageModules(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(115, 316), manager.ContentManager);
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {


            if (manager.InputManager.CheckIfLeftClick())
            {
                switchPageModules.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                        manager.SceneManager.RemoveScene();
                }
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            exitButton.Update(manager.InputManager.GetMousePosition());
            saveButton.Update(manager.InputManager.GetMousePosition());
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            saveButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            switchPageModules.Draw(spriteBatch);
        }

    }
}
