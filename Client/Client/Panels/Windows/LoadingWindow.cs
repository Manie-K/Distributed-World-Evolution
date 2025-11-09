using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.Panels.Windows
{
    public class LoadingWindow
    {
        private Texture2D background;
        private Text information;
        private Text Timerinformation;
        private int timer;
        public bool IsEnabled;

        public LoadingWindow(GameManager manager)
        {
            background = manager.ContentManager.Load<Texture2D>("Panels/LoadingWindow/Loading_Window");
            information = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsText"), "", true,
                                   new Vector2(466, 259),353, 87);
            Timerinformation = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/SettingsText"), "", true,
                       new Vector2(552, 389), 185, 75);
            IsEnabled = false;
            timer = 1;
            Timerinformation.SetText(TimerToString());

            information.SetTextColor(Color.Wheat);
            Timerinformation.SetTextColor(Color.Wheat);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(background, new Rectangle(415, 189, 452, 341), Color.White);
            information.Draw(spriteBatch);
            Timerinformation.Draw(spriteBatch);

        }

        public void SetLoadingInformation(string loadinginformation)
        {
            information.SetText(loadinginformation);
        }

        public void StartTimer()
        {
            Task.Run(async () =>
            {
                while (IsEnabled)
                {
                    await Task.Delay(1000);
                    timer++;
                    Timerinformation.SetText(TimerToString());
                }
            });
            timer = 1;
            Timerinformation.SetText(TimerToString());
        }

        private string TimerToString()
        {
            int minutes = timer / 60;
            int seconds = timer % 60;

            string minutesInString;
            string secondsInString;

            if (minutes > 0 && minutes < 10)
            {
                minutesInString = "0" + minutes.ToString();
            }
            else if (minutes >= 10)
            {
                minutesInString = minutes.ToString();
            }
            else minutesInString = "00";

            if (seconds > 0 && seconds < 10)
            {
                secondsInString = "0" + seconds.ToString();
            }
            else if (seconds >= 10)
            {
                secondsInString = seconds.ToString();
            }
            else secondsInString = "00";

            return minutesInString + ":" + secondsInString;
        }

    }
}
