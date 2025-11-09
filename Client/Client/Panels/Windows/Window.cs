using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Windows
{
    public class Window
    {
        private Texture2D background;
        private Button okButton;
        private Text information;
        public bool isEnabled;

        public Window(GameManager manager, WindowType type)
        {
            SetBackground(manager, type);
            okButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Warning_Window/OK_Button2"), null, null, new Vector2(563, 427), 155, 65, Color.Gold);
            information = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/WindowTextFont"), "", true,
                                   new Vector2(488, 308), 307, 84);
            isEnabled = false;
        }

        public void Update(Vector2 position)
        {
            okButton.Update(position);
        }

        public void CheckLeftClick(Vector2 position)
        {

            if (okButton.CheckLeftClick(position))
            {
                isEnabled = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, new Rectangle(415, 189, 452, 341), Color.White);
            okButton.Draw(spriteBatch);
            information.Draw(spriteBatch);
        }

        private void SetBackground(GameManager manager, WindowType type)
        {
            switch (type)
            {
                case WindowType.Error:
                    background = manager.ContentManager.Load<Texture2D>("Panels/Error_Window/Error_Window");
                    break;
                case WindowType.Warning:
                    background = manager.ContentManager.Load<Texture2D>("Panels/Warning_Window/Warning_Window");
                    break;
                case WindowType.Information:
                    background = manager.ContentManager.Load<Texture2D>("Panels/Information_Window/Information_Window");
                    break;
            }
        }

        public void SetInformation(string message)
        {
            information.SetText(message);
            isEnabled = true;
        }
    }
}