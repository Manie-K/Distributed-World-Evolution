using Client.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Panels
{
    public class Bestiary_Panel
    {
        private Texture2D background;
        private Button releaseButton;
        private Button exitButton;
        private GameManager manager;

        public Bestiary_Panel(GameManager manager)
        {
            this.manager = manager;
            background = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_background");
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_Exit"), null, null, new Vector2(909, 148), 60, 42, Color.Gold);
            releaseButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_Release"), null, null, new Vector2(514, 588), 253, 37, Color.Gold);

        }


        public bool Update(Vector2 position)
        {
            exitButton.Update(position);
            releaseButton.Update(position);

            if (manager.InputManager.CheckIfCanPressKey(Keys.Escape))
            {
                return true;
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.B))
            {
                return true;
            }

            return false;
        }

        public bool CheckLeftClick(Vector2 position)
        {
            if (releaseButton.CheckLeftClick(position))
            {

            }
            else if (exitButton.CheckLeftClick(position))
            {
                return true;
            }
            return false;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(244, 89, 792, 542), Color.White);
            releaseButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }
    }
}
