using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Client
{
    public class Button
    {
        private Texture2D backGroundTexture;
        private SpriteFont font;
        private string text;
        private Vector2 position;
        private Rectangle bounds;
        private Color textColor;
        private Color backgroundColor;
        private Color hoverColor;
        private bool isHovered;
        private bool isDisabled;


        public Button(Texture2D BackgroundTexture, SpriteFont Font, string Text, Vector2 Position, int Width, int Height, Color Color)
        {
            backGroundTexture = BackgroundTexture;
            font = Font;
            text = Text;
            position = Position;
            bounds = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            textColor = Color.Black;
            backgroundColor = Color.White;
            hoverColor = Color;
            isDisabled = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (backGroundTexture != null)
            {
                if (!isDisabled)
                {
                    backgroundColor = isHovered ? hoverColor : Color.White;
                    spriteBatch.Draw(backGroundTexture, bounds, backgroundColor);
                }else
                {
                    spriteBatch.Draw(backGroundTexture, bounds, backgroundColor * 0.7f);
                }
            }

            if (font != null && !string.IsNullOrEmpty(text))
            {
                Vector2 textSize = font.MeasureString(text);
                Vector2 textPosition = new Vector2(
                    position.X + (bounds.Width - textSize.X) / 2,
                    position.Y + (bounds.Height - textSize.Y) / 2
                );

                spriteBatch.DrawString(font, text, textPosition, textColor);
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
                return true;
            }
            return false;

        }

        public void SetBackgroundColor(Color color)
        {
            backgroundColor=color;
        }

        public void DisableButton()
        {
            isDisabled= true;   
        }
    }
}