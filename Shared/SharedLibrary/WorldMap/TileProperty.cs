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
}
