using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public enum Direction
    {
        up,
        right,
        down,
        left
    }
    public class Character : ColoredSprite
    {
        protected float speed;
        protected AnimationManager am;
        protected Direction CurrentDirection;
        protected Rectangle CharacterSize; // postacie wyswietlaja sie na wiekszych rozmiarach niz naprawde sa i lepiej dla kazdej postaci ustalic ich size zeby lepiej wykrywac kolizje pozniej

        public Character(Vector2 position, Color color, int width, int height, float speed, ref AnimationTexturesLoader ATL, int DefaultAnimationIndex)
            : base(null, position, width, height, color)
        {
            this.speed = speed;
            am = new AnimationManager(ref ATL, DefaultAnimationIndex);
            CurrentDirection = Direction.down;
        }

        public virtual void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState) { }
 
    }
}
