using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            background = manager.ContentManager.Load<Texture2D>("Panels/Error_Window/Error_Windows");
            okButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Error_Window/Ok_Button"), null, null, new Vector2(558, 442), 169, 52, Color.Brown);
            information = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/WindowTextFont"), "", false,
                       new Vector2(507, 283), 150, 100);
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

            spriteBatch.Draw(background, new Rectangle(460, 210, 361, 299), Color.White);
            okButton.Draw(spriteBatch);
            information.Draw(spriteBatch);

        }

        public void SetFailedConnectionInformation()
        {
            information.SetText("Failed connection to\nthe server.");
        }
    }
}