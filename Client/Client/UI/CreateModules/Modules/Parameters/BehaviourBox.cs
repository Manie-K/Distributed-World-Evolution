using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.DTOs.ModuleDTO;
using System.Collections.Generic;

namespace Client.UI.CreateModules.Modules.Parameters
{
    public class BehaviourBox
    {
        private Texture2D textureBehaviourBox;
        private Rectangle rect;

        private Button[] pageButtons;
        private Text behaviourText;
        private List<BehaviourDTO> behaviourParameters;
        public int selectedParameter;

        public BehaviourBox(Texture2D texture, SpriteFont font, List<BehaviourDTO> values, Vector2 position)
        {
            behaviourText = new Text(font,"",true, new Vector2(position.X + 280, position.Y - 2), 143, 46);
            textureBehaviourBox = texture;
            rect = new Rectangle((int)position.X + 245, (int)position.Y - 2, 213, 46);
            behaviourParameters = values;

            pageButtons = new Button[2];
            pageButtons[0] = new Button(null, null, "", new Vector2(position.X +245, position.Y-2), 35, 46, Color.White);
            pageButtons[1] = new Button(null, null, "", new Vector2(position.X + 421, position.Y-2), 35, 46, Color.White);
            selectedParameter = 0;

            behaviourText.SetTextColor(Color.White);
            if (values.Count > 0)
            {
                behaviourText.SetText(values[0].DatabaseID.ToString());
            }
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

            if (behaviourParameters.Count > 0)
            {
                behaviourText.SetText(behaviourParameters[selectedParameter].DatabaseID.ToString());
            }
            return isClicked;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureBehaviourBox, rect, Color.White);
            behaviourText.Draw(spriteBatch);
            pageButtons[0].Draw(spriteBatch);
            pageButtons[1].Draw(spriteBatch);
        }

        public string GetBehaviourDescription()
        {
            if (behaviourParameters.Count > 0)
            {
                return behaviourParameters[selectedParameter].Description;
            }
            else
            {
                return "";
            }
        }

        public int GetSelectedBehaviourID()
        {
            if (behaviourParameters.Count > 0)
            {
                return behaviourParameters[selectedParameter].DatabaseID;
            }
            else
            { 
                return 0;
            }
        }
    }
}
