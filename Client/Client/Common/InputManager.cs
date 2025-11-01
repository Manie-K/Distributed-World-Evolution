using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Common
{
    public class InputManager
    {
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;
        private Vector2 MousePosition;

        public InputManager()
        {

        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            MousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public void SetPreviousStates()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        public bool CheckIfCanPressKey(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public bool CheckIfPressingKey(Keys key)
        {
            if (currentKeyboardState.IsKeyDown(key)) return true;
            return false;
        }

        public bool CheckIfLeftClick()
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }

        public Vector2 GetMousePosition()
        {
            return MousePosition;
        }

    }
}
