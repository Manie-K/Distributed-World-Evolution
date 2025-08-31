using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.Bestiary_Panel
{
    public class InventorySlot
    {
        private Texture2D background;
        private Texture2D slotTexture;
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
            SetCreatureTexture(manager, type);
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
            spriteBatch.Draw(slotTexture, slotBounds, Color.White);
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
            slotBounds = new Rectangle((int)position.X + 7, (int)position.Y + 7, 36, 30);
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

        public void SetCreatureTexture(GameManager manager, int type)
        {
            switch (type)
            {
                case 0:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant1");
                    break;
                case 1:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant2");
                    break;
                case 2:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Inventory/InventoryImages/Bestiary_Plant3");
                    break;
                case 3:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Inventory/InventoryImages/Bestiary_Slime1");
                    break;
                case 4:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Inventory/InventoryImages/Bestiary_Slime2");
                    break;
            }
        }

        public int GetItemType()
        {
            return itemType;
        }
    }
}
