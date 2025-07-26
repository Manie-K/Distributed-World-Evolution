using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
    public class SettingsScene : IScene
    {
        private ContentManager contentManager;
        private Button ExitButton;
        private SwitchButton[] SwitchButtons;
        private Texture2D BackGround;
        private SceneManager sceneManager;
        private MouseState previousMouseState;
        private AudioManager audioManager;
        private Texture2D[] KeyBoardKeysImages;
        private Text[] KeyBoardKeysText;

        public SettingsScene(ContentManager contentManager, SceneManager sceneManager, AudioManager AudioManager)
        {
            this.contentManager = contentManager;
            this.sceneManager = sceneManager;
            this.SwitchButtons = new SwitchButton[2];
            this.KeyBoardKeysImages = new Texture2D[7];
            this.KeyBoardKeysText = new Text[4];

            this.ExitButton = new Button(contentManager.Load<Texture2D>("UI/White Close 2"), contentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1180, 30), 70, 70, Color.Red);
            this.SwitchButtons[0] = new SwitchButton(contentManager.Load<Texture2D>("UI/White Left"),
                                                     contentManager.Load<Texture2D>("UI/White Right"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsText"), 
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MUSIC", new Vector2(370, 375));
            this.SwitchButtons[1] = new SwitchButton(contentManager.Load<Texture2D>("UI/White Left"),
                                                     contentManager.Load<Texture2D>("UI/White Right"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsText"),
                                                     contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "EFFECTS", new Vector2(650, 375));
            this.KeyBoardKeysImages[0] = contentManager.Load<Texture2D>("UI/Keyboard_keys/w");
            this.KeyBoardKeysImages[1] = contentManager.Load<Texture2D>("UI/Keyboard_keys/s");
            this.KeyBoardKeysImages[2] = contentManager.Load<Texture2D>("UI/Keyboard_keys/a");
            this.KeyBoardKeysImages[3] = contentManager.Load<Texture2D>("UI/Keyboard_keys/d");
            this.KeyBoardKeysText[0] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MOVING", false, new Vector2(340, 100), 100, 60); ;
            this.KeyBoardKeysImages[4] = contentManager.Load<Texture2D>("UI/Keyboard_keys/space");
            this.KeyBoardKeysText[1] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "ATTACK", false, new Vector2(340, 155), 100, 60);
            this.KeyBoardKeysImages[5] = contentManager.Load<Texture2D>("UI/Keyboard_keys/e");
            this.KeyBoardKeysText[2] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "USE", false, new Vector2(340, 210), 100, 60);
            this.KeyBoardKeysImages[6] = contentManager.Load<Texture2D>("UI/Keyboard_keys/esc");
            this.KeyBoardKeysText[3] = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), "MENU", false, new Vector2(700, 100), 100, 60);

            this.BackGround = contentManager.Load<Texture2D>("UI/BG_Settings");
            this.audioManager = AudioManager;
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            Vector2 position = new Vector2(currentMouseState.X, currentMouseState.Y);

            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                if (ExitButton.CheckLeftClick(position))
                {
                    sceneManager.RemoveScene();
                    audioManager.UnmuteAll();
                }
                if (SwitchButtons[0].GetLeftButton().CheckLeftClick(position)) audioManager.DecreaseMusicVolume();
                if (SwitchButtons[0].GetRightButton().CheckLeftClick(position)) audioManager.IncreaseMusicVolume();
                if (SwitchButtons[1].GetLeftButton().CheckLeftClick(position)) audioManager.DecreaseEffectVolume();
                if (SwitchButtons[1].GetRightButton().CheckLeftClick(position)) audioManager.IncreaseEffectVolume();
            }

            ExitButton.Update(position);
            previousMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, new Rectangle(0, 0, 1280, 720), Color.White);
            ExitButton.Draw(spriteBatch);
            SwitchButtons[0].Draw(spriteBatch, (int)Math.Round(audioManager.GetGlobalMusicVolume() * 100f));
            SwitchButtons[1].Draw(spriteBatch, (int)Math.Round(audioManager.GetGlobalEffectVolume() * 100f));

            for(int i = 0; i < KeyBoardKeysText.Length; i++) KeyBoardKeysText[i].Draw(spriteBatch);
            for(int i = 0; i < 4; i++) spriteBatch.Draw(KeyBoardKeysImages[i], new Rectangle(460 + 45 * i, 100, 60, 60), Color.White);
            spriteBatch.Draw(KeyBoardKeysImages[4], new Rectangle(467, 155, 85, 60), Color.White);
            spriteBatch.Draw(KeyBoardKeysImages[5], new Rectangle(454, 210, 85, 60), Color.White);
            spriteBatch.Draw(KeyBoardKeysImages[6], new Rectangle(800, 100, 85, 60), Color.White);
        }
    }
}
