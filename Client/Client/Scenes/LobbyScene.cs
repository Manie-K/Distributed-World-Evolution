using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Messages;
using System.Collections.Generic;

namespace Client
{ 
    public class LobbyScene : IScene
    {
        private GameManager manager;

        private Texture2D backGround;
        private SwitchPage switchPage;
        private Button createButton;
        private Button joinButton;
        private Button refreshButton;
        private Button backButton;

        private bool isLoadingLobbies;
        private bool isJoiningLobby;
        private double timer;
        private double timeoutTimer;

        public LobbyScene(GameManager manager)
        {
            this.manager = manager;

            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Lobby_BG");
            switchPage = new SwitchPage(manager.ContentManager.Load<Texture2D>("UI/White Left"), manager.ContentManager.Load<Texture2D>("UI/White Right"),
                                             manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(542, 628), manager.ContentManager);
            joinButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/JoinButton"), null, null, new Vector2(953, 240), 180, 70, new Color(255, 255, 128));
            refreshButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/RefreshButton"), null, null, new Vector2(953, 330), 180, 70, new Color(255, 255, 128));
            createButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/CreateButton"), null, null, new Vector2(953, 420), 180, 70, new Color(255, 255, 128));
            backButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/Back_Button"), null, null, new Vector2(10, 10), 120, 46, new Color(255, 255, 128));

            isJoiningLobby = false;
            timer = 0;
            timeoutTimer = 0;

            LoadLobbies();
            _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.LobbyList));
            isLoadingLobbies = true;
            manager.ClientManager.LobbyListReady = ActionStatus.PENDING;
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (timeoutTimer >= 5)
            {
                isLoadingLobbies = false;
                isJoiningLobby = false;
                timeoutTimer = 0;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.WindowManager.ShowErrorMessage("Timeout with server");
                
            }

            if (timeoutTimer > 1 && !manager.WindowManager.LoadingWindow.IsEnabled)
            {
                if (isLoadingLobbies)
                {
                    manager.WindowManager.EnableLoadingWindow("Loading lobbies");
                }
                else if (isJoiningLobby)
                {
                    manager.WindowManager.EnableLoadingWindow("Joining lobby");
                }
            }

            if (isLoadingLobbies)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                timeoutTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0.1) return;

                if (manager.ClientManager.LobbyListReady == ActionStatus.SUCCESS)
                {
                    LoadLobbies();
                    manager.ClientManager.LobbyListReady = ActionStatus.IDLE;
                    isLoadingLobbies = false;
                    manager.WindowManager.LoadingWindow.IsEnabled = false;
                    timeoutTimer = 0;
                }
                else if (manager.ClientManager.LobbyListReady == ActionStatus.FAILED)
                {
                    manager.ClientManager.LobbyListReady = ActionStatus.IDLE;
                    isLoadingLobbies = false;
                    manager.WindowManager.LoadingWindow.IsEnabled = false;
                    timeoutTimer = 0;
                    manager.WindowManager.ShowErrorMessage("Failed to load lobbies");
                }

                timer = 0;
                return;
            }
            else if (isJoiningLobby)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                timeoutTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0.1) return;

                if (manager.ClientManager.LobbyJoined == ActionStatus.SUCCESS)
                {
                    timeoutTimer = 0;
                    isJoiningLobby = false;
                    manager.WindowManager.LoadingWindow.IsEnabled = false;
                    manager.SceneManager.AddScene(new GameScene(manager, switchPage.GetSelectedLobby().MapID));
                }
                else if (manager.ClientManager.LobbyJoined == ActionStatus.FAILED)
                {
                    timeoutTimer = 0;
                    isJoiningLobby = false;
                    manager.WindowManager.LoadingWindow.IsEnabled = false;
                    manager.ClientManager.LobbyJoined = ActionStatus.IDLE;
                    manager.WindowManager.ShowErrorMessage("Failed to join lobby");
                }

                timer = 0;
                return;
            }

            bool isPressed = false;

            if (manager.InputManager.CheckIfLeftClick())
            {
                switchPage.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (createButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.AddScene(new CreateLobbyScene(manager));
                }
                else if (joinButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    if (switchPage.GetSelectedLobby() == null)
                    { 
                        return;
                    }

                    _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new JoinLobbyMessage(switchPage.GetSelectedLobby().LobbyID, manager.UserSettings.PlayerName));
                    isJoiningLobby = true;
                    manager.ClientManager.LobbyJoined = ActionStatus.PENDING;
                }
                else if (refreshButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.LobbyList));
                    isLoadingLobbies = true;
                    manager.ClientManager.LobbyListReady = ActionStatus.PENDING;
                }
                else if(backButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                isPressed= true;    
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            createButton.Update(manager.InputManager.GetMousePosition());
            joinButton.Update(manager.InputManager.GetMousePosition());
            refreshButton.Update(manager.InputManager.GetMousePosition());
            backButton.Update(manager.InputManager.GetMousePosition());
            switchPage.UpdateRows(manager.InputManager.GetMousePosition(), isPressed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPage.Draw(spriteBatch);
            createButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            joinButton.Draw(spriteBatch);
            refreshButton.Draw(spriteBatch);
        }

        private void LoadLobbies()
        {
            IReadOnlyList<LobbyDTO> lobbies = manager.ClientManager.Lobbies;
            switchPage.ClearRows();
            string mapName;

            foreach (LobbyDTO lobby in lobbies)
            {
                mapName = lobby.MapID switch
                {
                    0 => "Forest",
                    _ => "Other",
                };

                switchPage.AddRow(lobby.Name, mapName, lobby.MapID, $"{lobby.CurrentPlayers}/{lobby.MaxPlayers}", lobby.ID);
            }
        }
    }
}
