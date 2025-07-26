using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Client
{
    public class SwitchButton
    {
        private Button Left;
        private Button Right;
        private Text Name;
        private Text ValueText;

        public SwitchButton(Texture2D LeftTexture, Texture2D RightTexture, SpriteFont fontText,SpriteFont fontNumbers, string name, Vector2 position)
        {
            Left = new Button(LeftTexture, fontNumbers, "", new Vector2(position.X+30, position.Y + 50), 25, 25, Color.White);
            Right = new Button(RightTexture, fontNumbers, "", new Vector2(position.X + 200, position.Y + 50), 25, 25, Color.White);
            Name = new Text(fontText, name, true, position, 250, 40);
            ValueText = new Text(fontNumbers, "100 %", true, new Vector2(position.X + 117,position.Y + 50), 25, 25);
        }

        public void Draw(SpriteBatch spriteBatch, int value)
        {
            Left.Draw(spriteBatch);
            Right.Draw(spriteBatch);
            Name.Draw(spriteBatch);
            ValueText.SetText(value.ToString() + " %");
            ValueText.Draw(spriteBatch);
        }

        public Button GetLeftButton()
        {
            return Left;
        }
        public Button GetRightButton()
        {
            return Right;
        }


    }
}
