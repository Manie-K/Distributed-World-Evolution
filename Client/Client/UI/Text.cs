using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class Text
    {
        private SpriteFont Font;
        private string text;
        private Vector2 Position;
        private Rectangle Bounds;
        private Color TextColor;
        private bool isCentered;

        public Text(SpriteFont font, string text, bool isCentered, Vector2 position, int width, int height)
        {
            this.Font = font;
            this.text = text;
            this.isCentered = isCentered;
            this.Position = position;
            this.Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            this.TextColor = Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            Vector2 textSize = Font.MeasureString(text);
            Vector2 textPosition;
            if (isCentered)
            {
                textPosition = new Vector2(
                Position.X + (Bounds.Width - textSize.X) / 2,
                Position.Y + (Bounds.Height - textSize.Y) / 2
            );
            }
            else
            {
                textPosition = new Vector2(
                                Position.X,
                                Position.Y + (Bounds.Height - textSize.Y) / 2
                            );
            }

            spriteBatch.DrawString(Font, text, textPosition, TextColor);
            


        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void SetTextColor(Color color)
        {
            TextColor = color;
        }
    }
}