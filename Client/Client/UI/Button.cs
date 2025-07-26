using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Client
{
    public class Button
    {
        private Texture2D BackGroundTexture;
        private SpriteFont Font;
        private string Text;
        private Vector2 Position;
        private Rectangle Bounds;
        private Color TextColor;
        private Color BackgroundColor;
        private Color HoverColor;
        private bool IsHovered;


        public Button(Texture2D backgroundTexture, SpriteFont font, string text, Vector2 position, int width, int height, Color color)
        {
            BackGroundTexture = backgroundTexture;
            Font = font;
            Text = text;
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            TextColor = Color.Black;
            BackgroundColor = Color.White;
            HoverColor = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BackGroundTexture != null)
            {
                BackgroundColor = IsHovered ? HoverColor : Color.White;
                spriteBatch.Draw(BackGroundTexture, Bounds, BackgroundColor);
            }

            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                Vector2 textSize = Font.MeasureString(Text);
                Vector2 textPosition = new Vector2(
                    Position.X + (Bounds.Width - textSize.X) / 2,
                    Position.Y + (Bounds.Height - textSize.Y) / 2
                );

                spriteBatch.DrawString(Font, Text, textPosition, TextColor);
            }
        }

        public void Update(Vector2 mousePosition)
        {
            IsHovered = Bounds.Contains(mousePosition);
        }

        public bool CheckLeftClick(Vector2 clickPosition)
        {
            if (Bounds.Contains(clickPosition))
            {
                return true;
            }
            return false;

        }
    }
}