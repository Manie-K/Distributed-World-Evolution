using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.UI.Bestiary_Panel
{
    public class Bestiary_Slot
    {
        private Texture2D background;
        private Texture2D creatureTexture;
        private Rectangle backgroundBounds;
        private Rectangle creatureTextureBounds;
        private Color backgroundColor;

        public Bestiary_Slot(GameManager manager, Vector2 position, int type)
        {
            SetCreatureTexture(manager, type);
            background = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/Bestiary_slot");
            SetPosition(position);
            backgroundColor = Color.White;
        }

        public void Update()
        {

        }

        public bool CheckLeftClick(Vector2 position)
        {
            if (backgroundBounds.Contains(position))
            {
                backgroundColor = Color.Black;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, backgroundBounds, backgroundColor);
            spriteBatch.Draw(creatureTexture, creatureTextureBounds, Color.White);
        }

        public void ResetColor()
        {
            backgroundColor = Color.White;
        }

        public void SetPosition(Vector2 position)
        {
            backgroundBounds = new Rectangle((int)position.X, (int)position.Y, 73, 50);
            creatureTextureBounds = new Rectangle((int)position.X + 16, (int)position.Y + 7, 40, 36);
        }

        public void SetCreatureTexture(GameManager manager, int type)
        {
            switch(type)
            {
                case 0:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant1");
                    break;
                case 1:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant2");
                    break;
                case 2:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Plant3");
                    break;
                case 3:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Slime1");
                    break;
                case 4:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Slime2");
                    break;
                case 5:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Slime3");
                    break;
                case 6:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Boar");
                    break;
                case 7:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Orc1");
                    break;
                case 8:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Orc2");
                    break;
                case 9:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Orc3");
                    break;
                case 10:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Vampire1");
                    break;
                case 11:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Vampire2");
                    break;
                case 12:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Vampire3");
                    break;
                case 13:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Rabbit1");
                    break;
                case 14:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Rabbit2");
                    break;
                case 15:
                    creatureTexture = manager.ContentManager.Load<Texture2D>("Panels/Bestiary_Panel/CreaturesImages/Bestiary_Pig");
                    break;


            }
        }
    }
}
