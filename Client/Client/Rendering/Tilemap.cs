using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Text.Json;

namespace Client.Rendering
{
    public class Tilemap
    {
        private Texture2D tilesetTexture;
        private Rectangle[] tileSourceRects;

        public string Name { get; set; }
        public string TexturePath { get; set; }
        public int TileSize { get; set; }
        public int TilesetWidth { get; set; }
        public int TilesetHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int[][] Tiles { get; set; }

        /// <summary>
        /// Loads the tilemap from a JSON file and sets the current map data.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the map data.</param>
        /// <returns>True if the map was loaded successfully; otherwise, false.</returns>
        public bool LoadMap(string filePath, ContentManager contentManager)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[LoadMap] File not found: {filePath}");
                    return false;
                }

                string json = File.ReadAllText(filePath);
                Tilemap loadedMap = JsonSerializer.Deserialize<Tilemap>(json);

                if (loadedMap == null)
                {
                    Console.WriteLine($"[LoadMap] Failed to deserialize JSON: {filePath}");
                    return false;
                }

                Name = loadedMap.Name;
                Width = loadedMap.Width;
                Height = loadedMap.Height;
                Tiles = loadedMap.Tiles;
                TexturePath = loadedMap.TexturePath;
                TileSize = loadedMap.TileSize;
                TilesetWidth = loadedMap.TilesetWidth;
                TilesetHeight = loadedMap.TilesetHeight;
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
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadMap] Error loading map from {filePath}: {ex.Message}");
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
