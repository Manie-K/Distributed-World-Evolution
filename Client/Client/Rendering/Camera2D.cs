using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace Client.Rendering
{
    public class Camera2D
    {
        public Size ScreenSize { get; }
        public Size MapSize { get; set; }
        public Vector2 Position { get; private set; }
        public Vector2 LastSavedPosition { get; set; }
        public Matrix Transform => Matrix.CreateTranslation(new Vector3(-Position, 0));

        public Camera2D(Size screenSize) 
        {
            ScreenSize = screenSize;
            MapSize = new Size(0, 0);
            Position = Vector2.Zero;
            LastSavedPosition = Vector2.Zero;
        }

        public void ResetPosition()
        {
            LastSavedPosition = Position;
            Position = Vector2.Zero;
        }

        public void SetLastPosition()
        {
            Position = LastSavedPosition;
        }

        public void CenterOn(Vector2 target)
        {
            Position = target - new Vector2(ScreenSize.Width / 2f, ScreenSize.Height / 2f);
            ClampPosition();
        }

        private void ClampPosition()
        {
            float maxX = MapSize.Width - ScreenSize.Width;
            float maxY = MapSize.Height - ScreenSize.Height;

            Position = new Vector2(
                MathHelper.Clamp(Position.X, 0, Math.Max(0, maxX)),
                MathHelper.Clamp(Position.Y, 0, Math.Max(0, maxY))
            );
        }
    }
}
