using Client.UI.Bestiary_Panel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Client.Panels
{
    public class BestiaryPanel
    {
        private GameManager manager;
        private Texture2D background;
        private Button exitButton;
        private List<BestiarySlot> slots;

        public BestiaryPanel(GameManager manager)
        {
            this.manager = manager;
            background = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_background");
            exitButton = new Button(manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_Exit"), null, null, new Vector2(909, 98), 60, 42, Color.Gold);
            slots = new List<BestiarySlot>();
        }

        public bool Update(Vector2 position)
        {
            exitButton.Update(position);

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
            if (exitButton.CheckLeftClick(position))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(244, 39, 792, 542), Color.White);
            for(int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }           

            exitButton.Draw(spriteBatch);
        }

        public bool AddSlot(int type)
        {
            if (slots.Count < 24 && !CheckIfExist(type))
            {
                slots.Add(new BestiarySlot(manager, GetPosition(slots.Count), type));
                return true;
            }
            return false;
        }

        public bool CheckIfExist(int type)
        {
            foreach(BestiarySlot slot in slots)
            {
                if(slot.Type == type)
                {
                    return true;
                }
            }
            return false;
        }

        private Vector2 GetPosition(int index)
        {
            int row = index % 6;
            int col = index / 6;
            int posX = 0;
            int posY = 0;

            switch (row)
            {
                case 0:
                    posX = 368;
                    break;
                case 1:
                    posX = 465;
                    break;
                case 2:
                    posX = 558;
                    break;
                case 3:
                    posX = 648;
                    break;
                case 4:
                    posX = 741;
                    break;
                case 5:
                    posX = 835;
                    break;
            }

            switch (col)
            {
                case 0:
                    posY = 154;
                    break;
                case 1:
                    posY = 230;
                    break;
                case 2:
                    posY = 304;
                    break;
                case 3:
                    posY = 378;
                    break;
            }
            return new Vector2(posX, posY);
        }
    }
}
