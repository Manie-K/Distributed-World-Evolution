using Client.UI.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Client
{
    public class SettingsScene : IScene
    {
        private GameManager manager;

        private Button exitButton;
        private Button backToMenuButton;
        private SwitchButton[] switchButtons;
        private SwitchPageSettings switchPageSettings;
        private Texture2D backGround;
        private MouseState previousMouseState;
        private Texture2D[] keyBoardKeysImages;
        private Text[] keyBoardKeysText;
        private TextBox playerNameTextBox;
        private Texture2D playerNameBackground;
        private Text playerNameText;
        private KeyboardState previousKeyboardState;

        public SettingsScene(GameManager manager)
        {
            this.manager = manager;

            switchButtons = new SwitchButton[2];
            keyBoardKeysImages = new Texture2D[7];
            keyBoardKeysText = new Text[4];

            switchPageSettings = new SwitchPageSettings(manager.ContentManager.Load<Texture2D>("UI/White Left"), manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                             manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(415, 570));
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1180, 30), 70, 70, Color.Red);
            backToMenuButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/BackToMainMenuButton"), null, null, new Vector2(1050, 550), 200, 200, Color.White);
            switchButtons[0] = new SwitchButton(manager.ContentManager.Load<Texture2D>("UI/White Left"),
                                                     manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                                     manager.ContentManager.Load<SpriteFont>("Fonts/SettingsText"), 
                                                     manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MUSIC", new Vector2(370, 375));
            switchButtons[1] = new SwitchButton(manager.ContentManager.Load<Texture2D>("UI/White Left"),
                                                     manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                                     manager.ContentManager.Load<SpriteFont>("Fonts/SettingsText"),
                                                     manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "EFFECTS", new Vector2(650, 375));
            keyBoardKeysImages[0] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/w");
            keyBoardKeysImages[1] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/s");
            keyBoardKeysImages[2] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/a");
            keyBoardKeysImages[3] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/d");
            keyBoardKeysText[0] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MOVING", false, new Vector2(340, 100), 100, 60);
            keyBoardKeysImages[4] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/space");
            keyBoardKeysText[1] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "ATTACK", false, new Vector2(340, 155), 100, 60);
            keyBoardKeysImages[5] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/e");
            keyBoardKeysText[2] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "USE", false, new Vector2(340, 210), 100, 60);
            keyBoardKeysImages[6] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/esc");
            keyBoardKeysText[3] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MENU", false, new Vector2(700, 100), 100, 60);
            playerNameTextBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), new Vector2(445, 190), 385, 87, Color.Black);
            playerNameTextBox.SetText(manager.UserSettings.PlayerName);
            playerNameText = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "USERNAME", false, new Vector2(570, 100), 100, 60);

            backGround = manager.ContentManager.Load<Texture2D>("UI/BG_Settings");
            playerNameBackground = manager.ContentManager.Load<Texture2D>("UI/Button_Large_A");

            previousKeyboardState = Keyboard.GetState();
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            KeyboardState currentKeyboardState = Keyboard.GetState(); 
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (exitButton.CheckLeftClick(position))
                {
                    ExitSettings();
                }
                if (manager.IsInGame && backToMenuButton.CheckLeftClick(position))
                {
                    manager.IsInGame = false;
                    ExitSettings();
                    manager.SceneManager.RemoveScene();
                    // TODO: send leave lobby message to the server
                }

                switchPageSettings.CheckLeftClick(position);

                switch (switchPageSettings.PageNumber)
                { 
                    case 1:
                        UpdatePage1(position);
                        break;
                    case 2:
                        UpdatePage2(position);
                        break;
                    default:
                        break;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                ExitSettings();
            }

            exitButton.Update(position);
            if (manager.IsInGame)
            {
                backToMenuButton.Update(position);
            }
            switch (switchPageSettings.PageNumber)
            {
                case 1:
                    break;
                case 2:
                    playerNameTextBox.Update();
                    break;
                default:
                    break;
            }

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        private void ExitSettings()
        {
            if (manager.IsInGame)
            {
                manager.Camera.SetLastPosition();
            }
            manager.UserSettings.PlayerName = playerNameTextBox.GetText();
            manager.UserSettings.SaveUserSettings();
            manager.SceneManager.RemoveScene();
        }

        private void UpdatePage1(Vector2 position)
        {
            if (switchButtons[0].GetLeftButton().CheckLeftClick(position))
            {
                manager.AudioManager.DecreaseMusicVolume();
                manager.UserSettings.GlobalMusicVolume = manager.AudioManager.GetGlobalMusicVolume();
            }
            if (switchButtons[0].GetRightButton().CheckLeftClick(position))
            {
                manager.AudioManager.IncreaseMusicVolume();
                manager.UserSettings.GlobalMusicVolume = manager.AudioManager.GetGlobalMusicVolume();
            }
            if (switchButtons[1].GetLeftButton().CheckLeftClick(position))
            {
                manager.AudioManager.DecreaseEffectVolume();
                manager.UserSettings.GlobalEffectVolume = manager.AudioManager.GetGlobalEffectVolume();
            }
            if (switchButtons[1].GetRightButton().CheckLeftClick(position))
            {
                manager.AudioManager.IncreaseEffectVolume();
                manager.UserSettings.GlobalEffectVolume = manager.AudioManager.GetGlobalEffectVolume();
            }
        }

        private void UpdatePage2(Vector2 position)
        {
            playerNameTextBox.CheckLeftClick(position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPageSettings.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            if (manager.IsInGame)
            {
                backToMenuButton.Draw(spriteBatch);
            }

            switch (switchPageSettings.PageNumber)
            {
                case 1:
                    DrawPage1(spriteBatch);
                    break;
                case 2:
                    DrawPage2(spriteBatch);
                    break;
                default:
                    break;
            }
        }

        private void DrawPage1(SpriteBatch spriteBatch)
        {
            switchButtons[0].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalMusicVolume() * 100f));
            switchButtons[1].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalEffectVolume() * 100f));

            for (int i = 0; i < keyBoardKeysText.Length; i++) keyBoardKeysText[i].Draw(spriteBatch);
            for (int i = 0; i < 4; i++) spriteBatch.Draw(keyBoardKeysImages[i], new Rectangle(460 + 45 * i, 100, 60, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[4], new Rectangle(467, 155, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[5], new Rectangle(454, 210, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[6], new Rectangle(800, 100, 85, 60), Color.White);
        }

        private void DrawPage2(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerNameBackground, new Rectangle(445, 190, 385, 87), Color.White);
            playerNameText.Draw(spriteBatch);
            playerNameTextBox.Draw(spriteBatch);
        }
    }
}
