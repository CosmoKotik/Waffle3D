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

        //gameEngine.CreateObject()
    }

    public int IDOffset = 1;

    public float x = 0;
    public float y = 0;
    public float z = 0;
    public bool isGoingBack = false;
    public float Speed = 1f;

    public override void OnUpdate()
    {
        //GameObject go = new GameObject();
        //go.FindChild();

        //Console.WriteLine("OnUpdate");
        gameEngine.SetRotation(new OpenTK.Vector3(z, z, z), IDOffset - 1);
        //gameEngine.SetPosition(new OpenTK.Vector3(z, 0, 0), 0);

        gameEngine.SetPosition(new OpenTK.Vector3(x, 0, 0), 1 * IDOffset);
        gameEngine.SetPosition(new OpenTK.Vector3(0, x, 0), 2 * IDOffset);
        gameEngine.SetPosition(new OpenTK.Vector3(0, 0, x), 3 * IDOffset);
        
        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 1 * IDOffset);
        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 2 * IDOffset);
        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 3 * IDOffset);
        
        gameEngine.SetPosition(-new OpenTK.Vector3(x, 0, 0), 4 * IDOffset);
        gameEngine.SetPosition(-new OpenTK.Vector3(0, x, 0), 5 * IDOffset);
        gameEngine.SetPosition(-new OpenTK.Vector3(0, 0, x), 6 * IDOffset);
        
        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 4 * IDOffset);
        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 5 * IDOffset);
        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 6 * IDOffset);

        Speed = Input.GetAxis("Scroll");

        //gameEngine.SetPosition(new OpenTK.Vector3(x, 0, 0), 6);
        if (x >= 8f)
            isGoingBack = true;
        else if (x <= 0.1f)
            isGoingBack = false;

        if (!isGoingBack)
            x += 0.1f * Speed;
        else if (isGoingBack)
            x -= 0.1f * Speed;

        y += 1f;
        z += 1f;
        //GameObject.FindChild().a();
    }
}