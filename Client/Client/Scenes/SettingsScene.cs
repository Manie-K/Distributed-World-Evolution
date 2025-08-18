using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;

namespace Client
{
    public class SettingsScene : IScene
    {
        private GameManager manager;

        private Button exitButton;
        private SwitchButton[] switchButtons;
        private Texture2D backGround;
        private MouseState previousMouseState;
        private Texture2D[] keyBoardKeysImages;
        private Text[] keyBoardKeysText;
        private KeyboardState previousKeyboardState;

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
            keyBoardKeysText[0] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MOVING", false, new Vector2(340, 100), 100, 60); ;
            keyBoardKeysImages[4] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/space");
            keyBoardKeysText[1] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "ATTACK", false, new Vector2(340, 155), 100, 60);
            keyBoardKeysImages[5] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/e");
            keyBoardKeysText[2] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "USE", false, new Vector2(340, 210), 100, 60);
            keyBoardKeysImages[6] = manager.ContentManager.Load<Texture2D>("UI/Keyboard_keys/esc");
            keyBoardKeysText[3] = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MENU", false, new Vector2(700, 100), 100, 60);

            backGround = manager.ContentManager.Load<Texture2D>("UI/BG_Settings");

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
                    manager.Camera.SetLastPosition();
                    manager.SceneManager.RemoveScene();
                    manager.AudioManager.UnmuteAll();
                }
                if (switchButtons[0].GetLeftButton().CheckLeftClick(position)) manager.AudioManager.DecreaseMusicVolume();
                if (switchButtons[0].GetRightButton().CheckLeftClick(position)) manager.AudioManager.IncreaseMusicVolume();
                if (switchButtons[1].GetLeftButton().CheckLeftClick(position)) manager.AudioManager.DecreaseEffectVolume();
                if (switchButtons[1].GetRightButton().CheckLeftClick(position)) manager.AudioManager.IncreaseEffectVolume();
            }

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                manager.Camera.SetLastPosition();
                manager.SceneManager.RemoveScene();
                manager.AudioManager.UnmuteAll();
            }

            exitButton.Update(position);

            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            exitButton.Draw(spriteBatch);
            switchButtons[0].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalMusicVolume() * 100f));
            switchButtons[1].Draw(spriteBatch, (int)Math.Round(manager.AudioManager.GetGlobalEffectVolume() * 100f));

            for(int i = 0; i < keyBoardKeysText.Length; i++) keyBoardKeysText[i].Draw(spriteBatch);
            for(int i = 0; i < 4; i++) spriteBatch.Draw(keyBoardKeysImages[i], new Rectangle(460 + 45 * i, 100, 60, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[4], new Rectangle(467, 155, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[5], new Rectangle(454, 210, 85, 60), Color.White);
            spriteBatch.Draw(keyBoardKeysImages[6], new Rectangle(800, 100, 85, 60), Color.White);
        }
    }
}
