using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3D;
using Wafle3D.Core;

public class Movement : WafleBehaviour
{
    public override void OnLoad()
    {
        //Console.WriteLine("mo");
        Console.WriteLine("OnStart");
    }

    public int x = 0;

    public override void OnUpdate()
    {
        //GameObject go = new GameObject();
        //go.FindChild();

        //Console.WriteLine("OnUpdate");

        gameEngine.SetPosition(new OpenTK.Vector3(x, 0, 0), 1);
        x++;


        //GameObject.FindChild().a();
    }
}