using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules
{
    public class DescriptionBox
    {
        public Button DescriptionButton;
        private Texture2D descriptionBackground;
        private Rectangle rect;
        private Text descitpionText;
        public bool IsClickedButton;


        //example of description text "The monster instantly reacts to any move-\nment, hurling itself at its prey with sa-\nvage fury. It slashes with claws and fangs."
        public DescriptionBox(SpriteFont fontNumbers, int offset, ContentManager contentManager)
        {
            DescriptionButton= new Button(contentManager.Load<Texture2D>("UI/CreateModules/DescriptionIcon"), null, "", new Vector2(1120, 484), 19, 37, Color.Lime);
            descitpionText= new Text(fontNumbers, "", true, new Vector2(740 + offset, 548), 389, 100);
            descriptionBackground = contentManager.Load<Texture2D>("UI/CreateModules/Create_Module_Description");
            rect = new Rectangle(740 + offset, 554, 389, 100);
            IsClickedButton = false;

            descitpionText.SetTextColor(Color.White);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            DescriptionButton.Draw(spriteBatch);
            if (IsClickedButton)
            {
                spriteBatch.Draw(descriptionBackground, rect, Color.White);
                descitpionText.Draw(spriteBatch);
            }
        }

        public void ChangeButton()
        {
            IsClickedButton = !IsClickedButton;
        }

        public void SetDescriptionText(string text)
        {
            descitpionText.SetText(FormatText(text));
        }

        private string FormatText(string text)
        {
            string output = "";
            int sumWordsInLine = 0;

            foreach (string word in text.Split(" "))
            {
                if (sumWordsInLine + word.Length > 43)
                {
                    output += "\n";
                    sumWordsInLine = 0;
                }
                output += word + " ";
                sumWordsInLine += word.Length + 1;
            }

            return output;
        }
    }
}
