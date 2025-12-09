using Microsoft.Xna.Framework;
using System;
using System.Drawing;

namespace Client.Rendering
{
    public class Camera2D
    {
        private const int DRAWING_BUFFOR_X = 25;
        private const int DRAWING_BUFFOR_Y = 25;

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

        public bool IsInCameraView(Vector2 position)
        {
            if (position.X < Position.X - DRAWING_BUFFOR_X || position.X > Position.X + ScreenSize.Width + DRAWING_BUFFOR_X)
            {
                return false;
            }
            if (position.Y < Position.Y - DRAWING_BUFFOR_Y|| position.Y > Position.Y + ScreenSize.Height + DRAWING_BUFFOR_Y)
            {
                return false;
            }
            return true;
        }
    }
}
