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
    public class WarningWindow
    {
        private GameManager manager;
        private Texture2D background;
        private Button okButton;
        private Text information;
        public bool isEnabled;

        public WarningWindow(GameManager manager)
        {
            this.manager = manager;
            background = manager.ContentManager.Load<Texture2D>("Panels/Warning_Window/Warning_Window");
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

        public void SetEmptyNameInformation()
        {
            information.SetText("Name is empty.");
        }

        public void SetWrongParametersInCreateLobby()
        {
            information.SetText("  Invalid name or incorrect\nnumber of players (max 32).");
        }


    }
}
