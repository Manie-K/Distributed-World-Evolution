using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharedLibrary;

namespace Client.Rendering
{
    public class WorldMap : Tilemap
    {
        private Texture2D tilesetTexture;
        private Rectangle[] tileSourceRects;

        /// <summary>
        /// Loads the tilemap from a JSON file and sets the current map data.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the map data.</param>
        /// <returns>True if the map was loaded successfully; otherwise, false.</returns>
        public bool InitMap(string filePath, ContentManager contentManager)
        {
            if (LoadMap(filePath))
            {
                tilesetTexture = contentManager.Load<Texture2D>(TexturePath);

                int tilesPerRow = TilesetWidth / TileSize;
                int tilesPerColumn = TilesetHeight / TileSize;
                int tileCount = tilesPerRow * tilesPerColumn;
                tileSourceRects = new Rectangle[tileCount];

                for (int i = 0; i < tileCount; i++)
                {
                    int x = (i % tilesPerRow) * TileSize;
                    int y = (i / tilesPerRow) * TileSize;

                    tileSourceRects[i] = new Rectangle(x, y, TileSize, TileSize);
                }

                return true;
            }
            else
            { 
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            int firstCol = (int)(camera.Position.X / TileSize);
            int firstRow = (int)(camera.Position.Y / TileSize);
            int cols = camera.ScreenSize.Width / TileSize + 2;
            int rows = camera.ScreenSize.Height / TileSize + 2;

            for (int y = firstRow; y < firstRow + rows; y++)
            {
                for (int x = firstCol; x < firstCol + cols; x++)
                {
                    if (x < 0 || y < 0 || x >= Width || y >= Height)
                        continue;

                    int tileID = Tiles[y][x];
                    Rectangle sourceRect = tileSourceRects[tileID];
                    Vector2 position = new Vector2(x * TileSize, y * TileSize);

                    spriteBatch.Draw(tilesetTexture, position, sourceRect, Color.White);
                }
            }
        }
    }
}
