using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3D;
using Wafle3D.Core;

public class CubeArray3 : WafleBehaviour
{
    public override void OnLoad()
    {
        //Console.WriteLine("mo");
        Console.WriteLine("OnStart");

        //gameEngine.CreateObject()
    }

    public int IDOffset = 14;

    public float x = 0;
    public float xx = 0;
    public float y = 0;
    public float z = 0;
    public bool isGoingBack = false;
    public bool isGoingBack2 = false;
    public float Speed = 1f;

    public bool CanRotate = false;
    public override void OnUpdate()
    {
        //GameObject go = new GameObject();
        //go.FindChild();

        //Console.WriteLine("OnUpdate");
        gameEngine.SetRotation(new OpenTK.Vector3(z, z, z), IDOffset);
        gameEngine.SetPosition(new OpenTK.Vector3(xx, 0, 0), IDOffset);

        gameEngine.SetPosition(new OpenTK.Vector3(x, 0, 0), 1 + IDOffset);
        gameEngine.SetPosition(new OpenTK.Vector3(0, x, 0), 2 + IDOffset);
        gameEngine.SetPosition(new OpenTK.Vector3(0, 0, x), 3 + IDOffset);

        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 1 + IDOffset);
        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 2 + IDOffset);
        gameEngine.SetRotation(new OpenTK.Vector3(y, y, y), 3 + IDOffset);

        gameEngine.SetPosition(-new OpenTK.Vector3(x, 0, 0), 4 + IDOffset);
        gameEngine.SetPosition(-new OpenTK.Vector3(0, x, 0), 5 + IDOffset);
        gameEngine.SetPosition(-new OpenTK.Vector3(0, 0, x), 6 + IDOffset);

        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 4 + IDOffset);
        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 5 + IDOffset);
        gameEngine.SetRotation(-new OpenTK.Vector3(y, y, y), 6 + IDOffset);

        Speed = Input.GetAxis("Scroll");

        //gameEngine.SetPosition(new OpenTK.Vector3(x, 0, 0), 6);
        if (x >= 6f)
            isGoingBack = true;
        else if (x <= 0.1f)
            isGoingBack = false;

        if (!isGoingBack)
            x += 0.1f * Speed;
        else if (isGoingBack)
            x -= 0.1f * Speed;

        if (xx >= 12f)
            isGoingBack2 = true;
        else if (xx <= 1f)
            isGoingBack2 = false;

        if (!isGoingBack2)
            xx += 0.2f * Speed;
        else if (isGoingBack2)
            xx -= 0.2f * Speed;

        if (Input.GetKeyDown(OpenTK.Input.Key.Number1) && CanRotate)
            CanRotate = false;
        else if (Input.GetKeyDown(OpenTK.Input.Key.Number1) && !CanRotate)
            CanRotate = true;

        if (CanRotate)
            y = z += 1f;
        else
            y = z = 0;
        //GameObject.FindChild().a();
    }
}