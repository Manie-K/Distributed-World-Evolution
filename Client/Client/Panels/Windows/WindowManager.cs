using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Panels.Windows
{
    public class WindowManager
    {
        private GameManager gameManager;
        public ErrorWindow ErrorWindow;
        public WarningWindow WarningWindow;

        public WindowManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            ErrorWindow = new ErrorWindow(gameManager);
            WarningWindow = new WarningWindow(gameManager);
        }

        public bool Update()
        {
            if (ErrorWindow.isEnabled)
            {
                if (gameManager.InputManager.CheckIfLeftClick())
                {
                    ErrorWindow.CheckLeftClick(gameManager.InputManager.GetMousePosition());
                }
                ErrorWindow.Update(gameManager.InputManager.GetMousePosition());

                return true;
            }

            if (WarningWindow.isEnabled)
            {
                if (gameManager.InputManager.CheckIfLeftClick())
                {
                    WarningWindow.CheckLeftClick(gameManager.InputManager.GetMousePosition());
                }
                WarningWindow.Update(gameManager.InputManager.GetMousePosition());

                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (WarningWindow.isEnabled) WarningWindow.Draw(spriteBatch);
            if (ErrorWindow.isEnabled) ErrorWindow.Draw(spriteBatch);
        }

        public void EnableWarningWindow()
        {
            WarningWindow.isEnabled = true;
        }

        public void EnableErrorWindow()
        {
            ErrorWindow.isEnabled = true;
        }
    }
}
