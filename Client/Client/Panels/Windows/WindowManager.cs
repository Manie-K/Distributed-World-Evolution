using Microsoft.Xna.Framework.Graphics;

namespace Client.Panels.Windows
{
    public enum WindowType
    {
        Warning,
        Error,
        Information
    }

    public class WindowManager
    {
        private GameManager gameManager;
        public Window ErrorWindow;
        public Window WarningWindow;
        public LoadingWindow LoadingWindow;
        public Window InformationWindow;

        public WindowManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
            ErrorWindow = new Window(gameManager, WindowType.Error);
            WarningWindow = new Window(gameManager, WindowType.Warning);
            LoadingWindow = new LoadingWindow(gameManager);
            InformationWindow = new Window(gameManager, WindowType.Information);

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

            if (InformationWindow.isEnabled)
            {
                if (gameManager.InputManager.CheckIfLeftClick())
                {
                    InformationWindow.CheckLeftClick(gameManager.InputManager.GetMousePosition());
                }
                InformationWindow.Update(gameManager.InputManager.GetMousePosition());

                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (WarningWindow.isEnabled) WarningWindow.Draw(spriteBatch);
            if (ErrorWindow.isEnabled) ErrorWindow.Draw(spriteBatch);
            if (LoadingWindow.IsEnabled) LoadingWindow.Draw(spriteBatch);
            if (InformationWindow.isEnabled) InformationWindow.Draw(spriteBatch);
        }

        public void ShowInformationWindow(string information)
        {
            InformationWindow.SetInformation(information);
        }

        public void ShowErrorWindow(string information)
        {
            ErrorWindow.SetInformation(information);
        }

        public void ShowWarningWindow(string information)
        {
            WarningWindow.SetInformation(information);
        }

        public void ShowLoadingWindow(string loadinginformation)
        {
            LoadingWindow.IsEnabled = true;
            LoadingWindow.SetLoadingInformation(loadinginformation);
            LoadingWindow.StartTimer();
        }

        private void ShowServerError()
        {
            ShowErrorWindow("Failed connection to the server.");
        }

    }
}
