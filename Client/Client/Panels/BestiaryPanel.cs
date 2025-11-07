using Client.Rendering;
using Client.UI.Bestiary_Panel;
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
            SetList();
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

        public void AddSlot(int Type)
        {
            if (slots.Count < 24)
            {
                slots.Add(new BestiarySlot(manager, GetPosition(slots.Count), Type));
            }
        }

        private void SetList()
        {
            AddSlot(0);
            AddSlot(1);
            AddSlot(2);
            AddSlot(3);
            AddSlot(4);
            AddSlot(5);
            AddSlot(6);
            AddSlot(7);
            AddSlot(8);
            AddSlot(9);
            AddSlot(10);
            AddSlot(11);
            AddSlot(12);
            AddSlot(13);
            AddSlot(14);
            AddSlot(15);
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
