using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Client.UI
{
    public class CheckBox
    {
        private Texture2D backGroundTexture;
        private Vector2 position;
        private Rectangle bounds;
        private Rectangle TextureBounds;
        private Color backgroundColor;
        private Color hoverColor;
        private bool isHovered;
        private bool value;


        public CheckBox(Texture2D backGroundTexture, Vector2 position, int width, int height, Color color)
        {
            this.backGroundTexture = backGroundTexture;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            TextureBounds = new Rectangle((int)position.X+30, (int)position.Y+ 10, 30, 30);
            backgroundColor = Color.White;
            hoverColor = color;
            value = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (backGroundTexture != null && value)
            {
                backgroundColor = isHovered ? hoverColor : Color.White;
                spriteBatch.Draw(backGroundTexture, TextureBounds, backgroundColor);

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
