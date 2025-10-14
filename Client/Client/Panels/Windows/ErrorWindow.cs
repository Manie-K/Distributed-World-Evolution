using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Windows
{
    public class ErrorWindow
    {
        private GameManager manager;
        private Texture2D background;
        private Button okButton;
        private Text information;
        public bool isEnabled;

        public ErrorWindow(GameManager manager)
        {
            this.manager = manager;
            background = manager.ContentManager.Load<Texture2D>("Panels/Error_Window/Error_Window");
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

        public void SetFailedConnectionInformation()
        {
            information.SetText("Failed connection to the server.");
        }

        public void SetErrorInformation(string message)
        {
            information.SetText(message);
        }
    }
}