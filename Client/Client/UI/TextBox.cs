using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    public class TextBox
    {
        private Texture2D backGroundTexture;
        private SpriteFont font;
        private string text;
        private Vector2 position;
        private Rectangle bounds;
        private Color textColor;
        private Color initTextColor;
        private Color backgroundColor;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private bool isFocused = false;
        private bool onlyNumbers;

        public TextBox(Texture2D BackgroundTexture, SpriteFont Font, Vector2 Position, int Width, int Height, Color Color, bool Onlynumbers = false)
        {
            backGroundTexture = BackgroundTexture;
            font = Font;
            text = "Your Name";
            position = Position;
            bounds = new Rectangle((int)position.X, (int)position.Y, Width, Height);
            textColor = Color;
            initTextColor = Color;
            backgroundColor = Color.White;
            onlyNumbers = Onlynumbers;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (backGroundTexture != null)
            {
                spriteBatch.Draw(backGroundTexture, bounds, backgroundColor);
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

        public bool CheckLeftClick(Vector2 clickPosition)
        {
            if (bounds.Contains(clickPosition))
            {
                isFocused = true;
                textColor = Color.DimGray;
                return true;
            }
            if (isFocused && onlyNumbers && string.IsNullOrWhiteSpace(text))
            {
                text = "0";
            }
            isFocused = false;
            textColor = initTextColor;
            return false;
        }


        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (!isFocused) return;

            Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (var key in pressedKeys)
            {
                if (previousKeyboardState.IsKeyUp(key))
                {
                    string keyString = KeyToString(key, currentKeyboardState);
                    if (!string.IsNullOrEmpty(keyString))
                    {
                        text += keyString;
                    }
                    if (key == Keys.Back && text.Length > 0)
                    {
                        text = text.Substring(0, text.Length - 1);
                    }
                }
            }
        }
        

        public void SetTextColor(Color color)
        {
            textColor = color;
        }

        public string GetText()
        {
            return text;
        }

        public void SetText(string Text)
        {
            text = Text;
        }

        public bool CheckTextIfEmpty()
        {
            return string.IsNullOrEmpty(text) || text == "|";
        }

        private string KeyToString(Keys key, KeyboardState state)
        {
            if (onlyNumbers)
            {
                if (key >= Keys.D0 && key <= Keys.D9)
                    return ((char)('0' + (key - Keys.D0))).ToString();

                if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    return ((char)('0' + (key - Keys.NumPad0))).ToString();

                return "";
            }
            else
            {
                bool shift = state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift);
                bool capsLock = state.CapsLock;

                if (key >= Keys.A && key <= Keys.Z)
                {
                    if ((shift && !capsLock) || (capsLock && !shift))
                    {
                        return key.ToString();
                    }
                    return key.ToString().ToLower();
                }

                if (key >= Keys.D0 && key <= Keys.D9)
                    return ((char)('0' + (key - Keys.D0))).ToString();

                if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    return ((char)('0' + (key - Keys.NumPad0))).ToString();

                if (key == Keys.Space)
                    return " ";

                return "";
            }
        }
    }
}
