using System.Text.Json.Serialization;

namespace SharedLibrary.Helpers
{
    public class Position2D
    {
        public int X { get; set; }
        public int Y { get; set; }
        [JsonConstructor]
        public Position2D(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Position2D(Position2D other)
        {
            X = other.X;
            Y = other.Y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Position2D pos)
            {
                return pos.X == X && pos.Y == Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Position2D operator +(Position2D a, Position2D b) => new Position2D(a.X + b.X, a.Y + b.Y);
        public static Position2D operator -(Position2D a, Position2D b) => new Position2D(a.X - b.X, a.Y - b.Y);
        public static Position2D operator *(Position2D a, int scalar) => new Position2D(a.X * scalar, a.Y * scalar);
    }
}
