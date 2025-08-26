using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary
{
    public class Tilemap
    {
        private readonly JsonSerializerOptions options;

        public string? Name { get; set; }
        public string? TexturePath { get; set; }
        public int TileSize { get; set; }
        public int TilesetWidth { get; set; }
        public int TilesetHeight { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<TileProperty>? TilesetData { get; set; }
        public int[][]? Tiles { get; set; }

        public Tilemap()
        {
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }

        /// <summary>
        /// Loads the tilemap from a JSON file and sets the current map data.
        /// </summary>
        /// <param name="filePath">The path to the JSON file containing the map data.</param>
        /// <returns>True if the map was loaded successfully; otherwise, false.</returns>
        public bool LoadMap(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"[LoadMap] File not found: {filePath}");
                    return false;
                }

                string json = File.ReadAllText(filePath);
                Tilemap? loadedMap = JsonSerializer.Deserialize<Tilemap>(json, options);

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
                TilesetData = loadedMap.TilesetData;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadMap] Error loading map from {filePath}: {ex.Message}");
                return false;
            }
        }

        public int GetTileIdAtPosition(Vector2 position)
        {
            int tileX = (int)(position.X / TileSize);
            int tileY = (int)(position.Y / TileSize);

            if (Tiles == null || tileX < 0 || tileY < 0 || tileX >= Width || tileY >= Height)
                return -1;

            return Tiles[tileY][tileX];
        }
    }
}
