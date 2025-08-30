using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.Bestiary_Panel
{
    public class InventorySlot
    {
        private Texture2D background;
        private Texture2D slotTexture;
        private Rectangle backgroundBounds;
        private Rectangle slotBounds;
        private Color backgroundColor;

        public InventorySlot(GameManager manager, Vector2 position, int type)
        {
            SetCreatureTexture(manager, type);
            background = manager.ContentManager.Load<Texture2D>("Panels/Inventory/Inventory_slot");
            SetPosition(position);
            backgroundColor = Color.White;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundBounds, backgroundColor);
            spriteBatch.Draw(slotTexture, slotBounds, Color.White);
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
        }

        public void SetCreatureTexture(GameManager manager, int type)
        {
            switch (type)
            {
                case 0:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant1");
                    break;
                case 1:
                    slotTexture = manager.ContentManager.Load<Texture2D>("Panels/Inventory/InventoryImages/Bestiary_Plant2");
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
    }
}
