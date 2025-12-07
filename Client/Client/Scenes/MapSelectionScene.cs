using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class MapSelectionScene : IScene
    {
        private GameManager manager;
        private Texture2D backGround;
        private Button exitButton;
        private SwitchPageMapSelection switchPageMapSelection;
        private Button saveButton;
        private SelectedMapData data;

        public MapSelectionScene(GameManager manager, ref SelectedMapData data)
        {
            this.manager = manager;
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("UI/White Close 2"), manager.ContentManager.Load<SpriteFont>("Fonts/ButtonFont"), "", new Vector2(1220, 25), 35, 35, Color.Red);
            backGround = manager.ContentManager.Load<Texture2D>("UI/Scenes/MapSelection_BG");
            saveButton = new Button(manager.ContentManager.Load<Texture2D>("UI/Buttons/MapSelection_SaveButton"), null, null, new Vector2(534, 584), 211, 62, new Color(255, 255, 128));
            this.data = data;
            switchPageMapSelection = new SwitchPageMapSelection( manager.ContentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), new Vector2(287, 335), data, manager.ContentManager);
        }

        public void Load() {}

        public void Update(GameTime gameTime)
        {
            if (manager.InputManager.CheckIfLeftClick())
            {
                switchPageMapSelection.CheckLeftClick(manager.InputManager.GetMousePosition());        
                if (exitButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    manager.SceneManager.RemoveScene();
                }
                else if (saveButton.CheckLeftClick(manager.InputManager.GetMousePosition()))
                {
                    switchPageMapSelection.SetSelectedData(ref data);
                    manager.SceneManager.RemoveScene();
                }
            }

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                manager.SceneManager.RemoveScene();
            }

            exitButton.Update(manager.InputManager.GetMousePosition());
            saveButton.Update(manager.InputManager.GetMousePosition());
        }

        public void Draw(SpriteBatch spriteBatch) {}

        public void DrawStatic(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, new Rectangle(0, 0, manager.Camera.ScreenSize.Width, manager.Camera.ScreenSize.Height), Color.White);
            saveButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            switchPageMapSelection.Draw(spriteBatch);
        }
    }
}
