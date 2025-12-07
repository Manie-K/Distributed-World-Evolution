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
        private bool isLoadingModules;
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
            isLoadingModules = false;
            timer = 0;
            timeoutTimer = 0;

            LoadLobbies();
            _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.LobbyList));
            isLoadingLobbies = true;
            manager.ClientManager.LobbyListReady = ActionStatus.PENDING;
        }

        public void Load() {}

        public void Update(GameTime gameTime)
        {
            if (timeoutTimer > ClientManager.SERVER_TIMOUT_TIME)
            {
                ResetLoadingState();
            }

            if (timeoutTimer > 1 && !manager.WindowManager.LoadingWindow.IsEnabled)
            {
                ShowLoadingWindow();
            }

            if (isLoadingLobbies)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateLoadingLobbies();
                return;
            }
            else if (isLoadingModules)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateLoadingModules();
                return;
            }
            else if (isJoiningLobby)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateJoiningLobby();
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

                    _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.ModuleList));
                    isLoadingModules = true;
                    manager.ClientManager.ModuleListReady = ActionStatus.PENDING;
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

        public void Draw(SpriteBatch spriteBatch) {}

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
                    1 => "Standard",
                    2 => "TwoBridges",
                    3 => "SmallStandard",
                    _ => "Other",
                };

                switchPage.AddRow(lobby.Name, mapName, lobby.MapID, $"{lobby.CurrentPlayers}/{lobby.MaxPlayers}", lobby.ID);
            }
        }

        private void ResetLoadingState()
        {
            isLoadingLobbies = false;
            isJoiningLobby = false;
            isLoadingModules = false;
            timeoutTimer = 0;
            manager.WindowManager.LoadingWindow.IsEnabled = false;
            manager.WindowManager.ShowErrorWindow("Timeout with server");
        }

        private void ShowLoadingWindow()
        {
            if (isLoadingLobbies)
            {
                manager.WindowManager.ShowLoadingWindow("Loading lobbies");
            }
            else if (isJoiningLobby)
            {
                manager.WindowManager.ShowLoadingWindow("Joining lobby");
            }
            else if (isLoadingModules)
            {
                manager.WindowManager.ShowLoadingWindow("Loading modules");
            }
        }

        private bool ShouldSkipUpdate(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            timeoutTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (timer < 0.1) return true;

            timer = 0;
            return false;
        }

        private void UpdateLoadingLobbies()
        {
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
                manager.WindowManager.ShowErrorWindow("Failed to load lobbies");
            }
        }

        private void UpdateLoadingModules()
        {
            if (manager.ClientManager.ModuleListReady == ActionStatus.SUCCESS)
            {
                _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new JoinLobbyMessage(switchPage.GetSelectedLobby().LobbyID, manager.UserSettings.PlayerName));
                isJoiningLobby = true;
                manager.ClientManager.LobbyJoined = ActionStatus.PENDING;

                manager.ClientManager.ModuleListReady = ActionStatus.IDLE;
                isLoadingModules = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                timeoutTimer = 0;
            }
            else if (manager.ClientManager.ModuleListReady == ActionStatus.FAILED)
            {
                isLoadingModules = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.ModuleListReady = ActionStatus.IDLE;
                timeoutTimer = 0;
                manager.WindowManager.ShowErrorWindow("Failed to load modules");
            }
        }

        private void UpdateJoiningLobby()
        {
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
                manager.WindowManager.ShowErrorWindow("Failed to join lobby");
            }
        }
    }
}
