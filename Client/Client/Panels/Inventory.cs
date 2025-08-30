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
    public class Inventory
    {
        private GameManager manager;
        private Texture2D background;
        private List<InventorySlot> slots;
        private int selectedSlot;

        public Inventory(GameManager manager)
        {
            this.manager = manager;
            background = manager.ContentManager.Load<Texture2D>("Panels/Inventory/Inventory");
            slots = new List<InventorySlot>();
            selectedSlot = -1;
            SetList();
        }

        public void Update()
        {
            if (manager.InputManager.CheckIfCanPressKey(Keys.Q))
            {
                RemoveSlot();
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D1))
            {
                pickSlot(0);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D2))
            {
                pickSlot(1);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D3))
            {
                pickSlot(2);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D4))
            {
                pickSlot(3);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D5))
            {
                pickSlot(4);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D6))
            {
                pickSlot(5);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D7))
            {
                pickSlot(6);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D8))
            {
                pickSlot(7);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D9))
            {
                pickSlot(8);
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(365, 615, 550, 96), Color.White);
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].Draw(spriteBatch);
            }
        }

        public void AddSlot(int Type)
        {
            if (slots.Count < 9)
            {
                slots.Add(new InventorySlot(manager, new Vector2(378 + (59 * slots.Count), 627), Type));
            }
        }

        public void RemoveSlot()
        {
            if (selectedSlot != -1)
            {
                slots.RemoveAt(selectedSlot);
                selectedSlot = -1;

                for(int i = 0; i < slots.Count; i++)
                {
                    slots[i].SetPosition(new Vector2(378 + (59 * i), 627));
                }
            }
        }

        private void SetList()
        {
            AddSlot(0);
            AddSlot(0);
            AddSlot(0);
            AddSlot(0);

        }

        public void ResetSlot()
        {
            if (selectedSlot != -1)
            {
                slots[selectedSlot].ResetColor();
                selectedSlot = -1;
            }
        }

        private void pickSlot(int index)
        {
            if (index >= 0 && index < slots.Count)
            {
                if (selectedSlot == -1 || selectedSlot != index)
                {
                    ResetSlot();
                    selectedSlot = index;
                    slots[selectedSlot].SetColor(Color.Black);
                }
                else
                {
                    ResetSlot();
                }
            }
        }


    }
}

