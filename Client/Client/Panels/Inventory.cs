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
        }

        public void Update()
        {
            if (manager.InputManager.CheckIfCanPressKey(Keys.Q))
            {
                RemoveSlot();
            }

            else if (manager.InputManager.CheckIfCanPressKey(Keys.D1))
            {
                PickSlot(0);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D2))
            {
                PickSlot(1);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D3))
            {
                PickSlot(2);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D4))
            {
                PickSlot(3);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D5))
            {
                PickSlot(4);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D6))
            {
                PickSlot(5);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D7))
            {
                PickSlot(6);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D8))
            {
                PickSlot(7);
            }
            else if (manager.InputManager.CheckIfCanPressKey(Keys.D9))
            {
                PickSlot(8);
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

        public bool AddSlot(int type)
        {
            if (slots.Count < 9)
            {
                slots.Add(new InventorySlot(manager, new Vector2(378 + (59 * slots.Count), 627), type));
                return true;
            }
            return false;
        }

        public bool CollectItem(int type)
        {
            foreach (var slot in slots)
            {
                if (slot.GetItemType() == type)
                {
                    slot.AddItem();
                    return true;
                }
            }
            if (AddSlot(type)) return true;
            
            return false;
        }

        public bool UseItem(int type)
        {
            int counter = 0;
            foreach (var slot in slots)
            {
                if (slot.GetItemType() == type)
                {
                    if (slot.RemoveItem()) RemoveSlot(counter);
                    return true;
                }
                counter++;
            }
            return false;
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

        public void RemoveSlot(int slotIndex)
        {
            slots.RemoveAt(slotIndex);
            selectedSlot = -1;

            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].SetPosition(new Vector2(378 + (59 * i), 627));
            }
        }

        public void ResetSlot()
        {
            if (selectedSlot != -1)
            {
                slots[selectedSlot].ResetColor();
                selectedSlot = -1;
            }
        }

        private void PickSlot(int index)
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

