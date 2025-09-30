using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class CheckBox
    {
        private Texture2D backGroundTexture;
        private Texture2D applyTexture;
        private Rectangle bounds;
        private Color backgroundColor;
        private Color hoverColor;
        private bool isHovered;
        private bool value;


        public CheckBox(Texture2D backGroundTexture, Texture2D applyTexture, Vector2 position, int width, int height, Color color)
        {
            this.backGroundTexture = backGroundTexture;
            this.applyTexture = applyTexture;
            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            backgroundColor = Color.White;
            hoverColor = color;
            value = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGroundTexture, bounds, Color.White);
            if (value)
            {
                backgroundColor = isHovered ? hoverColor : Color.White;
                spriteBatch.Draw(applyTexture, bounds, backgroundColor);

            }
        }

        public void Update(Vector2 mousePosition)
        {
            isHovered = bounds.Contains(mousePosition);
        }

        public bool CheckLeftClick(Vector2 clickPosition)
        {
            if (bounds.Contains(clickPosition))
            {
                value = !value;
                return true;
            }
            return false;
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor = color;
        }

        public bool GetValue()
        {
            return value;
        }

        public void SetValue(bool val)
        {
            value = val;
        }
    }
}
