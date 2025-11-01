using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateLobby.Parameters
{
    public class ParameterRow
    {
        private Text parameterName;
        private Text parameterValue;

        private Texture2D background;
        private Rectangle rect;

        public ParameterRow(ContentManager contentManager, string name, string value, Vector2 position, int type)
        {
            parameterName = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), name, true, position, 190, 46);

            if(type==0)
            {
                background = contentManager.Load<Texture2D>("UI/CreateModules/Stats/box_Button");
                rect = new Rectangle((int)position.X + 229, (int)position.Y, 86, 46);
                parameterValue = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), value, true, new Vector2(position.X + 229, position.Y), 86, 46);
            }
            else if (type==1)
            {
                background = contentManager.Load<Texture2D>("UI/CreateLobby/Text_Box");
                rect = new Rectangle((int)position.X + 202, (int)position.Y, 141, 46);
                parameterValue = new Text(contentManager.Load<SpriteFont>("Fonts/SettingsNumbers"), value, true, new Vector2(position.X + 202, position.Y), 141, 46);
            }

            parameterValue.SetTextColor(Color.White);
            parameterName.SetTextColor(Color.White);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            parameterName.Draw(spriteBatch);

            spriteBatch.Draw(background, rect, Color.White);
            parameterValue.Draw(spriteBatch);
        }

    }
}
