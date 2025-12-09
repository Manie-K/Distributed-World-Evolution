using System.Text.Json.Serialization;

namespace SharedLibrary.Helpers
{
    /// <summary>
    /// Position in 2D space.
    /// </summary>
    public class Position2D
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x"> X coordinate. </param>
        /// <param name="y"> Y coordinate. </param>
        [JsonConstructor]
        public Position2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="other"> Other position. </param>
        public Position2D(Position2D other)
        {
            X = other.X;
            Y = other.Y;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is Position2D pos)
            {
                return pos.X == X && pos.Y == Y;
            }
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static Position2D operator +(Position2D a, Position2D b) => new Position2D(a.X + b.X, a.Y + b.Y);
        
        public static Position2D operator -(Position2D a, Position2D b) => new Position2D(a.X - b.X, a.Y - b.Y);
        
        public static Position2D operator *(Position2D a, int scalar) => new Position2D(a.X * scalar, a.Y * scalar);
 
    }

}