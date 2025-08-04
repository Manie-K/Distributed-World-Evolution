using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Client
{
    public class CreateLobbyRow
    {
        private ICreatureData creatureData;
        private Text creatureName;
        private Rectangle bounds;

        public CreateLobbyRow(SpriteFont Font, AnimalData Animal, Vector2 Position, int Width, int Height)
        {

            creatureData = Animal;
            creatureName = new Text(Font, Animal.Name, true, Position, Width, Height);

            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public CreateLobbyRow(SpriteFont font, PlantData plant, Vector2 position, int width, int height)
        {

            creatureData = plant;
            creatureName = new Text(font, plant.Name, true, position, width, height);

            bounds = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public bool Update(Vector2 cursorPosition, bool isSelected)
        {
            if (isSelected)
            {
                creatureName.SetTextColor(Color.HotPink);
            }
            else if (bounds.Contains(cursorPosition))
            {
                creatureName.SetTextColor(Color.Gold);
                return true;
            }
            else
            {
                creatureName.SetTextColor(Color.White);
            }
            return false;
        }

        public ICreatureData GetCreatureData()
        {
            return creatureData;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            creatureName.Draw(spriteBatch);
        }


    }
}
