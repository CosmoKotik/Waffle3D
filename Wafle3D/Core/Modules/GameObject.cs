using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Wafle3D.Core
{
    public class GameObject
    {
        public string Name;
        public Transform transform;

        private WafleBehaviour _behaviour;

        public GameObject()
        {
            transform = new Transform();
        }

        public GameObject(WafleBehaviour behaviour)
        {
            this._behaviour = behaviour;
            //Console.WriteLine(b.Name);

            transform = new Transform();
        }

        public GameObject getGameObject()
        {
            return this;
        }

        public GameObject FindChild(int id = 0, string name = "")
        {
            //behaviour = (WafleBehaviour)cls.GetFields()[0].GetValue(behaviour);

            //Console.WriteLine("param: " + behaviour.Name);
            //Console.WriteLine(gameEngine.Title);

            return this;
        }

        public object GetComponent(string name)
        {
            foreach(WafleBehaviour b in _behaviour.gameEngine.Scripts)
            {
                if (b.Name == name)
                    return b;
            }

            return null;
        }

        public int a()
        {
            return 0;
        }
        public int b()
        {
            return 0;
        }
    }
}
