using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Windows
{
    public class WindowManager
    {
        private GameManager gameManager;
        public ErrorWindow ErrorWindow;
        public WarningWindow WarningWindow;
        public LoadingWindow LoadingWindow;

        public WindowManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            ErrorWindow = new ErrorWindow(gameManager);
            WarningWindow = new WarningWindow(gameManager);
            LoadingWindow = new LoadingWindow(gameManager);
            ClientManager.OnErrorMessageReceived += ShowServerError;
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
            if (LoadingWindow.IsEnabled) LoadingWindow.Draw(spriteBatch);
        }

        public void EnableWarningWindow()
        {
            WarningWindow.isEnabled = true;
        }

        public void EnableErrorWindow()
        {
            ErrorWindow.isEnabled = true;
        }

        public void EnableLoadingWindow(string loadinginformation)
        {
            LoadingWindow.IsEnabled = true;
            LoadingWindow.SetLoadingInformation(loadinginformation);
            LoadingWindow.StartTimer();
        }

        private void ShowServerError()
        {
            ErrorWindow.SetFailedConnectionInformation();
            EnableErrorWindow();
        }

        public void ShowErrorMessage(string message)
        {
            ErrorWindow.SetErrorInformation(message);
            EnableErrorWindow();
        }
    }
}
