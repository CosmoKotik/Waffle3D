using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace Wafle3D.Main.Modules
{
    public class Input
    {
        public static bool GetKeyDown(Key key)
        {
            //Initialising keyboard
            KeyboardState keyboard = Keyboard.GetState();;

            return keyboard.IsKeyDown(key);
        }
        public static bool GetKeyUp(Key key)
        {
            //Initialising keyboard
            KeyboardState keyboard = Keyboard.GetState();

            return keyboard.IsKeyUp(key);
        }
        public static bool IsAnyKeyDown()
        {
            //Initialising keyboard
            KeyboardState keyboard = Keyboard.GetState();

            return keyboard.IsAnyKeyDown;
        }

        public static bool GetMouseDown(MouseButton button)
        {
            //Initialising mouse
            MouseState mouse = Mouse.GetState();

            return mouse.IsButtonDown(button);
        }

        public static bool GetMouseUp(MouseButton button)
        {
            //Initialising mouse
            MouseState mouse = Mouse.GetState();

            return mouse.IsButtonUp(button);
        }

        public static int GetAxis(string axis)
        {
            //Initialising keyboard and mouse
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            int output = 0;

            switch (axis)
            {
                case "Mouse X":
                    output = mouse.X;
                    break;
                case "Mouse Y":
                    output = mouse.Y;
                    break;
                case "Keyboard X":
                    int aKey = GetKeyDown(Key.A) ? -1 : 0;
                    int dKey = GetKeyDown(Key.D) ? 1 : 0;

                    output = aKey + dKey;
                    break;
                case "Keyboard Y":
                    int wKey = GetKeyDown(Key.W) ? 1 : 0;
                    int sKey = GetKeyDown(Key.S) ? -1 : 0;

                    output = wKey + sKey;
                    break;
            }

            return output;
        }
    }
}
