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
        public Error_Window Error_Window;
        public Warning_Window Warning_Window;

        public WindowManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            Error_Window = new Error_Window(gameManager);
            Warning_Window = new Warning_Window(gameManager);
        }

        public bool Update()
        {
            if (Error_Window.isEnabled)
            {
                if (gameManager.InputManager.CheckIfLeftClick())
                {
                    Error_Window.CheckLeftClick(gameManager.InputManager.GetMousePosition());
                }
                Error_Window.Update(gameManager.InputManager.GetMousePosition());

                return true;
            }

            if (Warning_Window.isEnabled)
            {
                if (gameManager.InputManager.CheckIfLeftClick())
                {
                    Warning_Window.CheckLeftClick(gameManager.InputManager.GetMousePosition());
                }
                Warning_Window.Update(gameManager.InputManager.GetMousePosition());

                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Warning_Window.isEnabled) Warning_Window.Draw(spriteBatch);
            if (Error_Window.isEnabled) Error_Window.Draw(spriteBatch);
        }

        public void EnableWarningWindow()
        {
            Warning_Window.isEnabled = true;
        }

        public void EnableErrorWindow()
        {
            Error_Window.isEnabled = true;
        }
    }
}
