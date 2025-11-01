using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Client
{
    public class SwitchButton
    {
        private Button left;
        private Button right;
        private Text name;
        private Text valueText;

        public SwitchButton(Texture2D LeftTexture, Texture2D RightTexture, SpriteFont FontText, SpriteFont fontNumbers, string Name, Vector2 Position)
        {
            left = new Button(LeftTexture, fontNumbers, "", new Vector2(Position.X+30, Position.Y + 50), 25, 25, Color.White);
            right = new Button(RightTexture, fontNumbers, "", new Vector2(Position.X + 200, Position.Y + 50), 25, 25, Color.White);
            name = new Text(FontText, Name, true, Position, 250, 40);
            valueText = new Text(fontNumbers, "100 %", true, new Vector2(Position.X + 117, Position.Y + 50), 25, 25);
        }

        public void Draw(SpriteBatch spriteBatch, int value)
        {
            left.Draw(spriteBatch);
            right.Draw(spriteBatch);
            name.Draw(spriteBatch);
            valueText.SetText(value.ToString() + " %");
            valueText.Draw(spriteBatch);
        }

        public Button GetLeftButton()
        {
            return left;
        }
        public Button GetRightButton()
        {
            return right;
        }


    }
}
