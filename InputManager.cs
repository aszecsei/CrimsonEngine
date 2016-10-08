using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CrimsonEngine
{
    public class InputManager
    {
        private GamePadState _previousGamePadState;
        private GamePadState _gamePadState;

        private KeyboardState _previousKeyboardState;
        private KeyboardState _keyboardState;

        private MouseState _previousMouseState;
        private MouseState _mouseState;

        private PlayerIndex _playerIndex;

        public PlayerIndex PlayerIndex
        {
            get
            {
                return _playerIndex;
            }
        }

        public InputManager(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
        }
        
        public void FlushInput()
        {
            _previousGamePadState = _gamePadState;
            _previousKeyboardState = _keyboardState;
            _previousMouseState = _mouseState;

            _gamePadState = GamePad.GetState(_playerIndex);
            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
        }

        public bool IsButtonPressed(Buttons button)
        {
            return _previousGamePadState.IsButtonUp(button) && _gamePadState.IsButtonDown(button);
        }

        public bool IsButtonReleased(Buttons button)
        {
            return _previousGamePadState.IsButtonDown(button) && _gamePadState.IsButtonUp(button);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _previousKeyboardState.IsKeyUp(key) && _keyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _keyboardState.IsKeyUp(key);
        }

        public bool IsLeftMousePressed()
        {
            return _previousMouseState.LeftButton == ButtonState.Released && _mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightMousePressed()
        {
            return _previousMouseState.RightButton == ButtonState.Released && _mouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsLeftMouseReleased()
        {
            return _previousMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released;
        }

        public bool IsRightMouseReleased()
        {
            return _previousMouseState.RightButton == ButtonState.Pressed && _mouseState.RightButton == ButtonState.Released;
        }

        public GamePadState GamePadState
        {
            get
            {
                return _gamePadState;
            }
        }

        public KeyboardState KeyboardState
        {
            get
            {
                return _keyboardState;
            }
        }

        public MouseState MouseState
        {
            get
            {
                return _mouseState;
            }
        }
    }
}
