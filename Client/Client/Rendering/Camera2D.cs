using Microsoft.Xna.Framework;

namespace Client.Rendering
{
    public class Camera2D
    {
        public ScreenSize ScreenSize { get; }
        public Vector2 Position { get; private set; }
        public Matrix Transform => Matrix.CreateTranslation(new Vector3(-Position, 0));

        public Camera2D(ScreenSize screenSize) 
        {
            this.ScreenSize = screenSize;
        }

        public void CenterOn(Vector2 target)
        {
            Position = target - new Vector2(ScreenSize.Width / 2f, ScreenSize.Height / 2f);
        }
    }
}
