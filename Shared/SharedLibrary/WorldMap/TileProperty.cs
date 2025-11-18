namespace SharedLibrary
{
    public enum TileType
    {
        MapBorder,
        Grass,
        Wall,
        Water,
        Sand,
        Dirt,
        DarkGrass,
        Snow,
        Bridge,
        Null
    }

    public class TileProperty
    {
        /// <summary>
        /// ID of a tile from tileset texture
        /// </summary>
        public int Id { get; set; }
        public TileType Type { get; set; }
        public bool Walkable { get; set; }
    }

    public static class TileExtensions
    {
        private static readonly HashSet<TileType> FertileTypes = new()
        {
            TileType.Grass,
            TileType.Sand,
            TileType.Dirt,
            TileType.DarkGrass,
            TileType.Snow
        };

        public static bool IsFertile(this TileProperty tile)
            => tile.Walkable && FertileTypes.Contains(tile.Type);
    }
}
