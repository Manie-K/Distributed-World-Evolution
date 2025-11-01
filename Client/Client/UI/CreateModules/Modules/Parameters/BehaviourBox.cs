using Client.UI.MapSelection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class BehaviourBox
    {
        private Texture2D textureBehaviourBox;
        private Rectangle rect;

        private Button[] pageButtons;
        private Text behaviourText;
        private List<string> behaviourParameters;
        public int selectedParameter;

        public BehaviourBox(Texture2D texture, SpriteFont font, List<string> values, Vector2 position)
        {
            behaviourText = new Text(font,"",true, new Vector2(position.X + 290, position.Y - 2), 143, 46);
            textureBehaviourBox = texture;
            rect = new Rectangle((int)position.X + 255, (int)position.Y - 2, 213, 46);
            behaviourParameters = values;

            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, null, "", new Vector2(position.X +255, position.Y-2), 35, 46, Color.White);
            pageButtons[1] = new Button(null, null, "", new Vector2(position.X + 431, position.Y-2), 35, 46, Color.White);
            selectedParameter = 0;

            behaviourText.SetTextColor(Color.White);
            behaviourText.SetText(values[0]);
        }

        public bool CheckLeftClick(Vector2 cursorPosition)
        {
            bool isClicked = true;
            if (pageButtons[0].CheckLeftClick(cursorPosition))
            {
                if (selectedParameter > 0) selectedParameter--;
            }
            else if (pageButtons[1].CheckLeftClick(cursorPosition))
            {
                if (selectedParameter < behaviourParameters.Count - 1) selectedParameter++;
            }
            else isClicked = false;


            behaviourText.SetText(behaviourParameters[selectedParameter]);
            return isClicked;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureBehaviourBox, rect, Color.White);
            behaviourText.Draw(spriteBatch);
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
        }


    }
}
