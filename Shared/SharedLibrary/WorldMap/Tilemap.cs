using SharedLibrary.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary
{
    public class Tilemap
    {
        private readonly JsonSerializerOptions options;

        public string? Name { get; set; }
        public string? TexturePath { get; set; }
        /// <summary>
        /// Size of a single tile texture in pixels
        /// </summary>
        public int TileSize { get; set; }
        public int TilesetTextureWidth { get; set; } // in pixels
        public int TilesetTextureHeight { get; set; } // in pixels
        public int MapWidth { get; set; } // in number of tiles
        public int MapHeight { get; set; } // in number of tiles
        /// <summary>
        /// Details about each tile from tileset texture, identified by the tile id
        /// </summary>
        public List<TileProperty>? TilesetData { get; set; }
        /// <summary>
        /// 2D tile map representation with tile ids
        /// </summary>
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
        public bool LoadMap(string filePath, int mapID)
        {
            filePath += GetMapFileName(mapID);

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
                MapWidth = loadedMap.MapWidth;
                MapHeight = loadedMap.MapHeight;
                Tiles = loadedMap.Tiles;
                TexturePath = loadedMap.TexturePath;
                TileSize = loadedMap.TileSize;
                TilesetTextureWidth = loadedMap.TilesetTextureWidth;
                TilesetTextureHeight = loadedMap.TilesetTextureHeight;
                TilesetData = loadedMap.TilesetData;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadMap] Error loading map from {filePath}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets ID of a tile on given x,y map position
        /// </summary>
        /// <returns>TilesetData ID of a tile</returns>
        public int GetTileIdAtPosition(float x, float y)
        {
            int tileX = (int)(x / TileSize);
            int tileY = (int)(y / TileSize);

            if (Tiles == null || tileX < 0 || tileY < 0 || tileX >= MapWidth || tileY >= MapHeight)
                return -1;

            return Tiles[tileY][tileX];
        }

        /// <summary>
        /// Gets ID of a tile on given x,y tile
        /// </summary>
        /// <returns>TilesetData ID of a tile</returns>
        public int GetTileIdAtPosition2D(int x, int y)
        {
            if (Tiles == null || x < 0 || y < 0 || x >= MapWidth || y >= MapHeight)
                return -1;

            return Tiles[y][x];
        }

        /// <summary>
        /// Gets x,y tile based on given x,y map position
        /// </summary>
        /// <returns>Tile's x,y</returns>
        public Position2D GetTilePosition2D(float x, float y)
        {
            int tileX = (int)(x / TileSize);
            int tileY = (int)(y / TileSize);

            if (Tiles == null || tileX < 0 || tileY < 0 || tileX >= MapWidth || tileY >= MapHeight)
                return new Position2D(-1, -1);

            return new Position2D(tileX, tileY);
        }

        public bool[][] GetWalkableTiles()
        {
            if (Tiles == null || TilesetData == null) return [[false]];

            bool[][] walkableTiles = new bool[MapWidth][];

            for (int x = 0; x < MapWidth; x++)
            {
                walkableTiles[x] = new bool[MapHeight];
                for (int y = 0; y < MapHeight; y++)
                {
                    walkableTiles[x][y] = TilesetData.First(data => data.Id == Tiles[y][x]).Walkable;
                }
            }

            return walkableTiles;
        }

        public bool[][] GetFertileTiles()
        {
            if (Tiles == null || TilesetData == null) return [[false]];

            bool[][] fertileTiles = new bool[MapWidth][];

            for (int x = 0; x < MapWidth; x++)
            {
                fertileTiles[x] = new bool[MapHeight];
                for (int y = 0; y < MapHeight; y++)
                {
                    TileProperty tile = TilesetData.First(data => data.Id == Tiles[y][x]);
                    fertileTiles[x][y] = tile.IsFertile();
                }
            }

            return fertileTiles;
        }

        public static string GetMapFileName(int mapID)
        {
            string mapName = mapID switch
            {
                0 => "Grassland.json",
                1 => "Standard.json",
                2 => "TwoBridges.json",
                _ => "null.json",
            };

            return mapName;
        }
    }
}
