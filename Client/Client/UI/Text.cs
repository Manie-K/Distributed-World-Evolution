using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client
{
    public class Text
    {
        private SpriteFont font;
        private string text;
        private Vector2 position;
        private Rectangle bounds;
        private Color textColor;
        private bool isCentered;

        public Text(SpriteFont Font, string Text, bool IsCentered, Vector2 Position, int Width, int Height)
        {
            font = Font;
            text = Text;
            isCentered = IsCentered;
            position = Position;
            bounds = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            textColor = Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition;
            if (isCentered)
            {
                textPosition = new Vector2(
                position.X + (bounds.Width - textSize.X) / 2,
                position.Y + (bounds.Height - textSize.Y) / 2
            );
            }
            else
            {
                textPosition = new Vector2(
                                position.X,
                                position.Y + (bounds.Height - textSize.Y) / 2
                            );
            }

            spriteBatch.DrawString(font, text, textPosition, textColor);
            


        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public void SetTextColor(Color color)
        {
            textColor = color;
        }
    }
}