using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client
{
    public class CreateLobbyRow
    {
        private ICreatureData creatureData;
        private Text CreatureName;
        private Rectangle Bounds;

        public CreateLobbyRow(SpriteFont font, AnimalData animal, Vector2 position, int width, int height)
        {

            this.creatureData = animal;
            CreatureName = new Text(font, animal.name, true, position, width, height);

            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public CreateLobbyRow(SpriteFont font, PlantData plant, Vector2 position, int width, int height)
        {

            this.creatureData = plant;
            CreatureName = new Text(font, plant.name, true, position, width, height);

            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public bool Update(Vector2 cursorPosition, bool isSelected)
        {
            if (isSelected)
            {
                CreatureName.SetTextColor(Color.HotPink);
            }
            else if (Bounds.Contains(cursorPosition))
            {
                CreatureName.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                CreatureName.SetTextColor(Color.White);
            }
            return false;
        }

        public ICreatureData GetCreatureData()
        {
            return creatureData;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CreatureName.Draw(spriteBatch);
        }


    }
}
