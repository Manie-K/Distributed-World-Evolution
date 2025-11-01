using Client.Logic.Plants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary.Helpers;

namespace Client.UI.Bestiary_Panel
{
    public class InventorySlot
    {
        private Vector2 position;
        private Texture2D background;
        private Texture2D slotAmount;
        private Text textAmount;
        private Rectangle backgroundBounds;
        private Rectangle slotBounds;
        private Rectangle slotAmountBounds;
        private Color backgroundColor;

        private int amount;
        private int itemType;

        public InventorySlot(GameManager manager, Vector2 position, int type)
        {
            SetCreatureTexture(type);
            background = manager.ContentManager.Load<Texture2D>("Panels/Inventory/Inventory_slot");
            slotAmount = manager.ContentManager.Load<Texture2D>("Panels/Inventory/Invetory_amount");
            textAmount = new Text(manager.ContentManager.Load<SpriteFont>("Fonts/InvenotryNumbers"), "1", true, new Vector2((int)position.X + 38, (int)position.Y + 31), 19, 20);
            textAmount.SetTextColor(Color.White);
            SetPosition(position);
            backgroundColor = Color.White;
            amount = 1;
            itemType = type;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundBounds, backgroundColor);
            spriteBatch.Draw(PlantsAssetsManager.GetInstance().GetSpritesheet(), position, slotBounds, Color.White);
            spriteBatch.Draw(slotAmount, slotAmountBounds, Color.White);
            textAmount.Draw(spriteBatch);
        }

        public void ResetColor()
        {
            backgroundColor = Color.White;
        }

        public void SetColor(Color color)
        {
            backgroundColor = color;
        }

        public void SetPosition(Vector2 position)
        {
            backgroundBounds = new Rectangle((int)position.X, (int)position.Y, 50, 44);
            this.position = new Vector2((int)position.X + 9, (int)position.Y + 6);   
            slotAmountBounds = new Rectangle((int)position.X + 38, (int)position.Y + 31, 18, 21);
            textAmount.SetPosition(new Vector2((int)position.X + 38, (int)position.Y + 31));
        }

        public bool AddItem()
        {
            if (amount < 24)
            {
                amount++;
                textAmount.SetText(amount.ToString());
                return true;
            }
            return false;
        }

        public bool RemoveItem()
        {
            amount--;
            if (amount == 0) return true;

            textAmount.SetText(amount.ToString());
            return false;
        }

        public void SetCreatureTexture(int type)
        {
            switch (type)
            {
                case 0:
                    slotBounds = new Rectangle(0, 0, 32, 32);
                    break;
                case 1:
                    slotBounds = new Rectangle(32, 0, 32, 32);
                    break;
                case 2:
                    slotBounds = new Rectangle(64, 0, 32, 32);
                    break;
                case 3:
                    slotBounds = new Rectangle(96, 0, 32, 32);
                    break;
                case 4:
                    slotBounds = new Rectangle(128, 0, 32, 32);
                    break;
                case 5:
                    slotBounds = new Rectangle(160, 0, 32, 32);
                    break;
                case 6:
                    slotBounds = new Rectangle(0, 32, 32, 32);
                    break;
                case 7:
                    slotBounds = new Rectangle(32, 32, 32, 32);
                    break;
                case 8:
                    slotBounds = new Rectangle(64, 32, 32, 32);
                    break;
                case 9:
                    slotBounds = new Rectangle(96, 32, 32, 32);
                    break;
                case 10:
                    slotBounds = new Rectangle(128, 32, 32, 32);
                    break;
                case 11:
                    slotBounds = new Rectangle(160, 32, 32, 32);
                    break;
            }
        }

        public int GetItemType()
        {
            return itemType;
        }
    }
}
