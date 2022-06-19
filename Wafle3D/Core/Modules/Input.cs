using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using Wafle3D.Core;

namespace Wafle3D.Core
{
    public class Input
    {
        public static bool GetKeyDown(Key key)
        {
            //Initialising keyboard
            KeyboardState keyboard = Keyboard.GetState(); ;

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
                case "Scroll":
                    output = (int)mouse.Scroll.Y;
                    break;
                case "Keyboard X":
                    /*int aKey = GetKeyDown(Key.A) ? -1 : 0;
                    int dKey = GetKeyDown(Key.D) ? 1 : 0;
                    
                    int leftKey = GetKeyDown(Key.Left) ? -1 : 0;
                    int rightKey = GetKeyDown(Key.Right) ? 1 : 0;*/

                    int keyX = GetKeyDown(Key.A) ? -1 : GetKeyDown(Key.D) ? 1 : 0;
                    int numX = GetKeyDown(Key.Left) ? -1 : GetKeyDown(Key.Right) ? 1 : 0;

                    if (keyX + numX == 2)
                        numX = 0;
                    else if (keyX + numX == -2)
                        numX = 0;
                    
                    output = keyX + numX;
                    break;
                case "Keyboard Y":
                    /*int wKey = GetKeyDown(Key.W) ? 1 : 0;
                    int sKey = GetKeyDown(Key.S) ? -1 : 0;*/

                    int keyY = GetKeyDown(Key.S) ? -1 : GetKeyDown(Key.W) ? 1 : 0;
                    int numY = GetKeyDown(Key.Down) ? -1 : GetKeyDown(Key.Up) ? 1 : 0;

                    if (keyY + numY == 2)
                        numY = 0;
                    else if (keyY + numY == -2)
                        numY = 0;

                    output = keyY + numY;
                    break;
            }

            return output;
        }
    }
}
