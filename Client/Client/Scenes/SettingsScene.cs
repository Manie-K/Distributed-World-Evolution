using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using Client.Rendering;

namespace Client
{
    public class SettingsScene : IScene
    {
        private Button exitButton;
        private SwitchButton[] switchButtons;
        private Texture2D backGround;
        private SceneManager sceneManager;
        private MouseState previousMouseState;
        private AudioManager audioManager;
        private Texture2D[] keyBoardKeysImages;
        private Text[] keyBoardKeysText;
        private KeyboardState previousKeyboardState;

        public SettingsScene(ContentManager contentManager, SceneManager sceneManager, AudioManager audioManager)
        {
            this.sceneManager = sceneManager;
            switchButtons = new SwitchButton[2];
            keyBoardKeysImages = new Texture2D[7];
            keyBoardKeysText = new Text[4];

            exitButton = new Button(contentManager.Load<Texture2D>("UI/White Close 2"), contentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1180, 30), 70, 70, Color.Red);
            switchButtons[0] = new SwitchButton(contentManager.Load<Texture2D>("UI/White Left"),
                                                     contentManager.Load<Texture2D>("UI/White Right"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsText"), 
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MUSIC", new Vector2(370, 375));
            switchButtons[1] = new SwitchButton(contentManager.Load<Texture2D>("UI/White Left"),
                                                     contentManager.Load<Texture2D>("UI/White Right"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsText"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "EFFECTS", new Vector2(650, 375));
            keyBoardKeysImages[0] = contentManager.Load<Texture2D>("UI/Keyboard_keys/w");
            keyBoardKeysImages[1] = contentManager.Load<Texture2D>("UI/Keyboard_keys/s");
            keyBoardKeysImages[2] = contentManager.Load<Texture2D>("UI/Keyboard_keys/a");
            keyBoardKeysImages[3] = contentManager.Load<Texture2D>("UI/Keyboard_keys/d");
            keyBoardKeysText[0] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MOVING", false, new Vector2(340, 100), 100, 60); ;
            keyBoardKeysImages[4] = contentManager.Load<Texture2D>("UI/Keyboard_keys/space");
            keyBoardKeysText[1] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "ATTACK", false, new Vector2(340, 155), 100, 60);
            keyBoardKeysImages[5] = contentManager.Load<Texture2D>("UI/Keyboard_keys/e");
            keyBoardKeysText[2] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "USE", false, new Vector2(340, 210), 100, 60);
            keyBoardKeysImages[6] = contentManager.Load<Texture2D>("UI/Keyboard_keys/esc");
            keyBoardKeysText[3] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MENU", false, new Vector2(700, 100), 100, 60);

            backGround = contentManager.Load<Texture2D>("UI/BG_Settings");

            previousKeyboardState = Keyboard.GetState();
            this.audioManager = audioManager;
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
                    sceneManager.RemoveScene();
                    audioManager.UnmuteAll();
                }
                if (switchButtons[0].GetLeftButton().CheckLeftClick(position)) audioManager.DecreaseMusicVolume();
                if (switchButtons[0].GetRightButton().CheckLeftClick(position)) audioManager.IncreaseMusicVolume();
                if (switchButtons[1].GetLeftButton().CheckLeftClick(position)) audioManager.DecreaseEffectVolume();
                if (switchButtons[1].GetRightButton().CheckLeftClick(position)) audioManager.IncreaseEffectVolume();
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                sceneManager.RemoveScene();
                audioManager.UnmuteAll();
            }

            exitButton.Update(position);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, camera.ScreenSize.Width, camera.ScreenSize.Height), Color.White);
            exitButton.Draw(spriteBatch);
            switchButtons[0].Draw(spriteBatch, (int)Math.Round(audioManager.GetGlobalMusicVolume() * 100f));
            switchButtons[1].Draw(spriteBatch, (int)Math.Round(audioManager.GetGlobalEffectVolume() * 100f));

            for(int i = 0; i < keyBoardKeysText.Length; i++) keyBoardKeysText[i].Draw(spriteBatch);
            for(int i = 0; i < 4; i++) spriteBatch.Draw(keyBoardKeysImages[i], new Rectangle(460 + 45 * i, 100, 60, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[4], new Rectangle(467, 155, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[5], new Rectangle(454, 210, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[6], new Rectangle(800, 100, 85, 60), Color.White);
        }
    }
}
