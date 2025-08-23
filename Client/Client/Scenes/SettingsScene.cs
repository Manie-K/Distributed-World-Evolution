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
        private SwitchButton[] switchButtons;
        private Texture2D backGround;
        private Texture2D[] keyBoardKeysImages;
        private Text[] keyBoardKeysText;
        private TextBox playerNameTextBox;

        private Text nicknameTextBoxText;
        private Texture2D nicknameTextBoxTexture;

        public SettingsScene(GameManager manager)
        {
            this.manager = manager;

            switchButtons = new SwitchButton[2];
            keyBoardKeysImages = new Texture2D[7];
            keyBoardKeysText = new Text[4];

            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1180, 30), 70, 70, Color.Red);
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
            playerNameTextBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(688, 564), 190, 56, Color.Black);
            playerNameTextBox.SetText(manager.UserSettings.PlayerName);

            
            nicknameTextBoxTexture = manager.ContentManager.Load<Texture2D>("UI/Buttons/Username_TextBox");
            nicknameTextBoxText = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "Nickname", false, new Vector2(730, 525), 106, 40);

            backGround = manager.ContentManager.Load<Texture2D>("UI/BG_Settings");

        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (manager.InputManager.CheckIfLeftClick())
            {
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    ExitSettings();
                }

                UpdatePage(manager.InputManager.GetMousePosition());
            }


            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                ExitSettings();
            }


            exitButton.Update(manager.InputManager.GetMousePosition());
            playerNameTextBox.Update();
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

        private void UpdatePage(Vector2 position)
        {
            if (switchButtons[0].GetLeftButton().CheckLeftClick(position))
            {
                manager.AudioManager.DecreaseMusicVolume();
                manager.UserSettings.GlobalMusicVolume = manager.AudioManager.GetGlobalMusicVolume();
            }
            else if (switchButtons[0].GetRightButton().CheckLeftClick(position))
            {
                manager.AudioManager.IncreaseMusicVolume();
                manager.UserSettings.GlobalMusicVolume = manager.AudioManager.GetGlobalMusicVolume();
            }
            else if (switchButtons[1].GetLeftButton().CheckLeftClick(position))
            {
                manager.AudioManager.DecreaseEffectVolume();
                manager.UserSettings.GlobalEffectVolume = manager.AudioManager.GetGlobalEffectVolume();
            }
            else if (switchButtons[1].GetRightButton().CheckLeftClick(position))
            {
                manager.AudioManager.IncreaseEffectVolume();
                manager.UserSettings.GlobalEffectVolume = manager.AudioManager.GetGlobalEffectVolume();
            }

            if (!manager.IsInGame)
            {
                playerNameTextBox.CheckLeftClick(position);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            exitButton.Draw(spriteBatch);

            DrawPage(spriteBatch);
        }

        private void DrawPage(SpriteBatch spriteBatch)
        {
            switchButtons[0].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalMusicVolume() * 100f));
            switchButtons[1].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalEffectVolume() * 100f));

            for (int i = 0; i < keyBoardKeysText.Length; i++) keyBoardKeysText[i].Draw(spriteBatch);
            for (int i = 0; i < 4; i++) spriteBatch.Draw(keyBoardKeysImages[i], new Rectangle(460 + 45 * i, 100, 60, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[4], new Rectangle(467, 155, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[5], new Rectangle(454, 210, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[6], new Rectangle(800, 100, 85, 60), Color.White);

            spriteBatch.Draw(nicknameTextBoxTexture, new Rectangle(688, 564, 190, 56), Color.White);
            nicknameTextBoxText.Draw(spriteBatch);
            playerNameTextBox.Draw(spriteBatch);
        }
    }
}
