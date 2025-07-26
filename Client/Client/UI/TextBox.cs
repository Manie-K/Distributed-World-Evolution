using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TextBox
    {
        private Texture2D BackGroundTexture;
        private SpriteFont Font;
        private string Text;
        private Vector2 Position;
        private Rectangle Bounds;
        private Color TextColor;
        private Color BackgroundColor;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private bool isFocused = false;
        private bool OnlyNumbers;

        public TextBox(Texture2D backgroundTexture, SpriteFont font, Vector2 position, int width, int height, Color color, bool onlynumbers = false)
        {
            BackGroundTexture = backgroundTexture;
            Font = font;
            Text = "|";
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
            TextColor = color;
            BackgroundColor = Color.White;
            OnlyNumbers = onlynumbers;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BackGroundTexture != null)
            {
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

        public bool CheckLeftClick(Vector2 clickPosition)
        {
            if (Bounds.Contains(clickPosition))
            {
                isFocused = true;
                Text = "";
                return true;
            }
            if (isFocused && OnlyNumbers && string.IsNullOrWhiteSpace(Text))
            {
                Text = "0";
            }
            isFocused = false;
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
                    string keyString = KeyToString(key);
                    if (!string.IsNullOrEmpty(keyString))
                    {
                        Text += keyString;
                    }
                    if (key == Keys.Back && Text.Length > 0)
                    {
                        Text = Text.Substring(0, Text.Length - 1);
                    }
                }
            }
        }
        

        public void SetTextColor(Color color)
        {
            TextColor = color;
        }

        public string GetText()
        {
            return Text;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public bool CheckTextIfEmpty()
        {
            return string.IsNullOrEmpty(Text) || Text == "|";
        }

        private string KeyToString(Keys key)
        {
            if (OnlyNumbers)
            {
                if (key >= Keys.D0 && key <= Keys.D9)
                    return ((char)('0' + (key - Keys.D0))).ToString();

                if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                    return ((char)('0' + (key - Keys.NumPad0))).ToString();

                return "";
            }
            else
            {
                if (key >= Keys.A && key <= Keys.Z)
                    return key.ToString().ToLower();

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
