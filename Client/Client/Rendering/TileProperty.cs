namespace Client.Rendering
{
    public enum TileType
    {
        MapBorder,
        Grass,
        Wall,
        Water,
        Sand,
        Dirt,
    }

    public class TileProperty
    {
        public int Id { get; set; }
        public TileType Type { get; set; }
        public bool Walkable { get; set; }
    }
}
