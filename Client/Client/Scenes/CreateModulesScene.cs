using Client.UI.CreateModules.Modules;
using Client.UI.CreateModules.Modules.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Server.Core;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Client
{
    public class CreateModulesScene : IScene
    {
        private GameManager manager;

        private Texture2D backGround;
        private TextBox moduleName;
        private SwitchPageModules switchPageModules;
        private DescriptionBox descriptionBox;
        private Button exitButton;
        private Button saveButton;

        private SwitchPageModulesParameters switchPageModulesParameters;

        private bool isLoadingBehaviours;
        private bool isCreatingModule;
        private double timer;
        private double timeoutTimer;

        public CreateModulesScene(GameManager manager)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/Create_Module");
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/SaveButton"), null, null, new Vector2(520, 570), 211, 79, new Color(255, 255, 128));
            switchPageModules = new SwitchPageModules(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(115, 316), manager.ContentManager);
            moduleName = new TextBox(null, manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(886, 110), 220, 42, Color.White);
            descriptionBox = new DescriptionBox(manager.ContentManager.Load<SpriteFont>("Fonts/DescriptionFont"), 0, manager.ContentManager);

            this.switchPageModulesParameters = new SwitchPageModulesParameters(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"),
                                                     new Vector2(806, 482), manager.ContentManager, 4);

            isCreatingModule = false;
            timer = 0;
            timeoutTimer = 0;

            _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new GetMessage(GetMessageTypeEnum.BehaviourList));
            isLoadingBehaviours = true;
            manager.ClientManager.BehaviourListReady = ActionStatus.PENDING;
        }

        public void Load()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (timeoutTimer > 5)
            {
                ResetLoadingState();
            }

            if (timeoutTimer > 1 && !manager.WindowManager.LoadingWindow.IsEnabled)
            {
                ShowLoadingWindow();
            }

            if (isLoadingBehaviours)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateLoadingBehaviours();
                return;
            }
            else if (isCreatingModule)
            {
                if (ShouldSkipUpdate(gameTime)) return;
                UpdateCreatingModule();
                return;
            }

            if (manager.InputManager.CheckIfLeftClick())
            {
                switchPageModules.CheckLeftClick(manager.InputManager.GetMousePosition());
                moduleName.CheckLeftClick(manager.InputManager.GetMousePosition());
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    if (!moduleName.CheckTextIfEmpty() && switchPageModulesParameters.GetBehavioursList().Count > 0)
                    {
                        EntityTypeEnum newEntityType = (EntityTypeEnum)switchPageModulesParameters.GetBehavioursList().Where(e => e.Item1 == 5).First().Item2;
                        List<int> newBehaviours = new List<int>();
                        foreach (Tuple<int, int> beh in switchPageModulesParameters.GetBehavioursList())
                        {
                            if (beh.Item1 == 5) continue;

                            newBehaviours.Add(beh.Item2);
                        }

                        CreateModuleDTO newModule = new CreateModuleDTO(moduleName.GetText(), false, switchPageModulesParameters.GetValueOnIndex(0),
                            switchPageModulesParameters.GetValueOnIndex(1), switchPageModulesParameters.GetValueOnIndex(2),
                            switchPageModulesParameters.GetValueOnIndex(4), switchPageModulesParameters.GetValueOnIndex(3),
                            newEntityType, switchPageModules.GetGraphicIndex(), newBehaviours);

                        _ = MessageManager.SendMessageAsync(manager.ClientManager.Client, new CreateModuleMessage(newModule));
                        isCreatingModule = true;
                        manager.ClientManager.ModuleCreated = ActionStatus.PENDING;
                    }
                    else
                    {
                        manager.WindowManager.WarningWindow.SetWrongParametersInCreateLobby();
                        manager.WindowManager.EnableWarningWindow();
                    }
                }
                else if (descriptionBox.DescriptionButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    descriptionBox.ChangeButton();
                }

                if (switchPageModulesParameters.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    descriptionBox.SetDescriptionText(switchPageModulesParameters.GetLastDescription());
                }
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            switchPageModulesParameters.UpdateRows(manager.InputManager.GetMousePosition());
            moduleName.Update();
            exitButton.Update(manager.InputManager.GetMousePosition());
            saveButton.Update(manager.InputManager.GetMousePosition());
            descriptionBox.DescriptionButton.Update(manager.InputManager.GetMousePosition());
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            saveButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            moduleName.Draw(spriteBatch);
            descriptionBox.Draw(spriteBatch);
            switchPageModules.Draw(spriteBatch);
            switchPageModulesParameters.Draw(spriteBatch);
        }


        public void CreateAnimalJSON()
        {
            var json = JsonSerializer.Serialize(new
            {
                LobbyName = moduleName.GetText(),
                ModuleGraphics = switchPageModules.GetIndex(),
                Health = switchPageModulesParameters.GetValueOnIndex(0),
                Damage = switchPageModulesParameters.GetValueOnIndex(1),
                Hunger = switchPageModulesParameters.GetValueOnIndex(2),
                Behaviours = switchPageModulesParameters.GetBehavioursList()
            }) ;

            Console.WriteLine(json);
        }

        private void ResetLoadingState()
        {
            isLoadingBehaviours = false;
            isCreatingModule = false;
            manager.WindowManager.LoadingWindow.IsEnabled = false;
            timeoutTimer = 0;
            manager.WindowManager.ShowErrorMessage("Timeout with server");
        }

        private void ShowLoadingWindow()
        {
            if (isLoadingBehaviours)
            {
                manager.WindowManager.EnableLoadingWindow("Loading behaviours");
            }
            else if (isCreatingModule)
            {
                manager.WindowManager.EnableLoadingWindow("Creating module");
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

        private void UpdateLoadingBehaviours()
        {
            if (manager.ClientManager.BehaviourListReady == ActionStatus.SUCCESS)
            {
                switchPageModulesParameters.AddRow(manager.ClientManager.Behaviours.ToList());
                manager.ClientManager.BehaviourListReady = ActionStatus.IDLE;
                isLoadingBehaviours = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                timeoutTimer = 0;
            }
            else if (manager.ClientManager.BehaviourListReady == ActionStatus.FAILED)
            {
                isLoadingBehaviours = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.BehaviourListReady = ActionStatus.IDLE;
                timeoutTimer = 0;
                manager.WindowManager.ShowErrorMessage("Failed to load behaviours");
            }
        }

        private void UpdateCreatingModule()
        {
            if (manager.ClientManager.ModuleCreated == ActionStatus.SUCCESS)
            {
                isCreatingModule = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.ModuleCreated = ActionStatus.IDLE;
                timeoutTimer = 0;
                manager.WindowManager.ShowErrorMessage("Module created");
            }
            else if (manager.ClientManager.ModuleCreated == ActionStatus.FAILED)
            {
                isCreatingModule = false;
                manager.WindowManager.LoadingWindow.IsEnabled = false;
                manager.ClientManager.ModuleCreated = ActionStatus.IDLE;
                timeoutTimer = 0;
                manager.WindowManager.ShowErrorMessage("Failed to create module");
            }
        }
    }
}
