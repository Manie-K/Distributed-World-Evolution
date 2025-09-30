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

        public DescriptionBox(SpriteFont fontNumbers, ContentManager contentManager)
        {
            DescriptionButton= new Button(contentManager.Load<Texture2D>("UI/CreateModules/DescriptionIcon"), null, "", new Vector2(1120,484), 19, 37, Color.Lime);
            descitpionText= new Text(fontNumbers, "The monster instantly reacts to any move-\nment, hurling itself at its prey with sa-\nvage fury. It slashes with claws and fangs.", true, new Vector2(740, 548), 389, 100);
            descriptionBackground = contentManager.Load<Texture2D>("UI/CreateModules/Create_Module_Description");
            rect = new Rectangle(740, 554, 389, 100);
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
    }
}
