using Client.UI.CreateLobby;
using Client.UI.CreateLobby.Parameters;
using Client.UI.CreateModules.Modules;
using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedLibrary;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Client
{
    public class CreateLobbyScene : IScene
    {
        private GameManager manager;

        private Texture2D backGround;
        private Button exitButton;
        private SwitchPageLobby switchPageLobby;
        private Button saveButton;
        private TextBox gameNameBox;
        private Button mapButton;
        private TextBox playerAmountBox;
        private SelectedMapData mapData;
        private ModulesImageDisplay modulesImageDisplay;
        private SwitchPageParameters switchPageParameters;

        private DescriptionBox descriptionBox;

        private bool isCreatingLobby;
        private bool isJoiningLobby;
        private bool isLoadingModules;
        private double timer;
        private double timeoutTimer;

        public CreateLobbyScene(GameManager manager)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Create_Lobby");
            switchPageLobby = new SwitchPageLobby(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(155, 295), manager.ContentManager, 4);
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(540, 570), 211, 79, new Color(255, 255, 128));
            mapButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/MapSelectionButton"), null, null, new Vector2(606, 126), 42, 44, Color.Gold);
            gameNameBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(288, 129), 170, 36, Color.White);
            playerAmountBox = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                        new Vector2(523, 129), 50, 36, Color.White, true);
            playerAmountBox.SetText("4");
            mapData = new SelectedMapData();
            modulesImageDisplay = new ModulesImageDisplay(manager.ContentManager);

            switchPageParameters = new SwitchPageParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                                 new Vector2(848, 485), manager.ContentManager, 4);
            descriptionBox = new DescriptionBox(manager.ContentManager.Load<SpriteFont>("Fonts/DescriptionFont"), 26, manager.ContentManager);

            isCreatingLobby = false;
            isJoiningLobby = false;
            timer = 0;
            timeoutTimer = 0;

            InitializeCreaturesRows();
            _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.ModuleList));
            isLoadingModules = true;
            manager.ClientManager.ModuleListReady = ActionStatus.PENDING;
        }

        public void Load()
        {

        }

        public void InitializeCreaturesRows()
        {
            IReadOnlyList<ModuleDTO> modules = manager.ClientManager.Modules;
            switchPageLobby.ClearModules();

            foreach (ModuleDTO module in modules)
            {
                List<ModuleParametersData> parameters = new List<ModuleParametersData>();
                parameters.Add(new ModuleParametersData("Type", module.Type.ToString(), 1, "Type of a creature"));
                parameters.Add(new ModuleParametersData("Damage", module.Damage.ToString(), 0, "Damage"));
                parameters.Add(new ModuleParametersData("Aggresion", module.Aggresion.ToString(), 0, "Aggresion"));
                parameters.Add(new ModuleParametersData("Reproduction", module.ReproductionNeed.ToString(), 0, "Reproduction Need"));
                parameters.Add(new ModuleParametersData("Max Health", module.MaxHealth.ToString(), 0, "Max Health"));
                parameters.Add(new ModuleParametersData("Max Hunger", module.MaxHunger.ToString(), 0, "Max Hunger"));

                if (module.Behaviours != null)
                {
                    foreach (BehaviourDTO behaviour in module.Behaviours)
                    {
                        parameters.Add(new ModuleParametersData(behaviour.InteractionType.ToString(), behaviour.DatabaseID.ToString(), 1, behaviour.Description));
                    }
                }

                switchPageLobby.AddRow(new UI.CreateLobby.Parameters.ModuleData(module.Name, module.DatabaseID, module.GraphicalRepresentationID, module.IsOfficialModule, parameters));
            }
        }

        public void Update(GameTime gameTime)
        {
            if (timeoutTimer > 5)
            {
                ResetLoadingState();
            }

            if(timeoutTimer > 1 && !manager.WindowManager.LoadingWindow.IsEnabled)
            {
                ShowLoadingWindow();
            }

            if (isCreatingLobby)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateCreatingLobby();
                return;
            }
            else if (isJoiningLobby)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateJoiningLobby();
                return;
            }
            else if (isLoadingModules)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateLoadingModules();
                return;
            }

            bool isPressed = false;

            if (manager.InputManager.CheckIfLeftClick())
            {
                gameNameBox.CheckLeftClick(manager.InputManager.GetMousePosition());
                playerAmountBox.CheckLeftClick(manager.InputManager.GetMousePosition());

                switchPageLobby.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    if (!gameNameBox.CheckTextIfEmpty() && playerAmountBox.CheckText(32))
                    {
                        Tilemap map = new Tilemap();
                        if (!map.LoadMap($"Content/Maps/", mapData.index))
                        {
                            throw new Exception("Could not load the map " + Tilemap.GetMapFileName(mapData.index));
                        }

                        IEnumerable<int> modules = switchPageLobby.GetSelectedModulesIDList();
                        CreateLobbyMessage message = new CreateLobbyMessage(gameNameBox.GetText(), manager.UserSettings.PlayerName,
                            int.Parse(playerAmountBox.GetText()), mapData.index, modules, map.GetWalkableTiles(), map.GetFertileTiles());
                        _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, message);
                        isCreatingLobby = true;
                        manager.ClientManager.LobbyCreated = ActionStatus.PENDING;
                    }
                    else
                    {
                        manager.WindowManager.ShowWarningWindow("Invalid name or incorrect number of players (max 32).");
                    }
                }
                else if (mapButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.AddScene(new MapSelectionScene(manager, ref mapData));
                }
                else if (descriptionBox.DescriptionButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    descriptionBox.ChangeButton();
                }

                if (switchPageParameters.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    descriptionBox.SetDescriptionText(switchPageParameters.GetLastDescription());
                }

                isPressed = true;
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            exitButton.Update(manager.InputManager.GetMousePosition());
            saveButton.Update(manager.InputManager.GetMousePosition());
            mapButton.Update(manager.InputManager.GetMousePosition());
            gameNameBox.Update();
            playerAmountBox.Update();
            descriptionBox.DescriptionButton.Update(manager.InputManager.GetMousePosition());

            if (switchPageLobby.UpdateRows(manager.InputManager.GetMousePosition(), isPressed))
            {
                switchPageParameters.SetParameters(switchPageLobby.GetPickedModule().ModuleParameters);
                modulesImageDisplay.SetImage(switchPageLobby.GetPickedModule().GraphicIndex);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            switchPageLobby.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            mapButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            gameNameBox.Draw(spriteBatch);
            playerAmountBox.Draw(spriteBatch);
            modulesImageDisplay.Draw(spriteBatch);
            descriptionBox.Draw(spriteBatch);

            if (switchPageLobby.selectedRow != -1)
            {
                switchPageParameters.Draw(spriteBatch);
            }
        }

        private void ResetLoadingState()
        {
            isCreatingLobby = false;
            isJoiningLobby = false;
            isLoadingModules = false;
            manager.WindowManager.LoadingWindow.IsEnabled = false;
            timeoutTimer = 0;
            manager.WindowManager.ShowErrorWindow("Timeout with server");
        }

        private void ShowLoadingWindow()
        {
            if (isCreatingLobby)
            {
                manager.WindowManager.ShowLoadingWindow("Creating lobby");
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

        private void UpdateCreatingLobby()
        {
            if (manager.ClientManager.LobbyCreated == ActionStatus.SUCCESS)
            {
                isJoiningLobby = true;
                isCreatingLobby = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.LobbyCreated = ActionStatus.IDLE;
                manager.ClientManager.SetPendingLobbyJoined();
                timeoutTimer = 0;
            }
            else if (manager.ClientManager.LobbyCreated == ActionStatus.FAILED)
            {
                isCreatingLobby = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.LobbyCreated = ActionStatus.IDLE;
                timeoutTimer = 0;
                manager.WindowManager.ShowErrorWindow("Failed to create lobby");
            }
        }

        private void UpdateJoiningLobby()
        {
            if (manager.ClientManager.LobbyJoined == ActionStatus.SUCCESS)
            {
                timeoutTimer = 0;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.SceneManager.RemoveScene();
                manager.SceneManager.AddScene(new GameScene(manager, mapData.index));
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

        private void UpdateLoadingModules()
        {
            if (manager.ClientManager.ModuleListReady == ActionStatus.SUCCESS)
            {
                InitializeCreaturesRows();
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
    }
}
