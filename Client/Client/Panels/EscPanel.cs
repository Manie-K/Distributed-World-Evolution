using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.Messages;

namespace Client.Panels
{
    public class EscPanel
    {
        private Texture2D background;
        private Button playButton;
        private Button settingsButton;
        private Button exitButton;
        private GameManager manager;

        public EscPanel(GameManager manager)
        {
            this.manager = manager;
            background =  manager.ContentManager.Load<Texture2D>("Panels/Esc_Panel/panel_esc");
            playButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Esc_Panel/Play_Button2"), null, null, new Vector2(485, 196), 309, 68, Color.Gold);
            settingsButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Esc_Panel/Settings_Button2"), null, null, new Vector2(485, 277), 309, 65, Color.Gold);
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Esc_Panel/Exit_Button2"), null, null, new Vector2(485, 356), 309, 66, Color.Gold);
        }

        public bool Update(Vector2 position)
        {
            playButton.Update(position);
            settingsButton.Update(position);
            exitButton.Update(position);

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                return true;
            }

            return false;
        }

        public bool CheckLeftClick(Vector2 position)
        {
            if (playButton.CheckLeftClick(position))
            {
                if (!manager.UserSettings.PlayerName.Equals(""))
                {
                    return true;
                }
            }
            else if (settingsButton.CheckLeftClick(position))
            {
                manager.Camera.ResetPosition();
                manager.SceneManager.AddScene(new SettingsScene(manager));
                return true;
            }
            else if (exitButton.CheckLeftClick(position))
            {
                manager.Camera.ResetPosition();
                manager.IsInGame = false;
                manager.SceneManager.RemoveScene();
                _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new DisjoinLobbyMessage(manager.ClientManager.LobbyData.ID));
                manager.ClientManager.LobbyJoined = ActionStatus.IDLE;
                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(376, 100, 530, 398), Color.White);
            playButton.Draw(spriteBatch);
            settingsButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }
    }
}
