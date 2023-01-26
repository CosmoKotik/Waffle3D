using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wafle3D;
using Wafle3D.Core;
using Wafle3D.Core.Modules;
using SimplexNoise;

public class Movement : WafleBehaviour
{
    private List<Vector3> _modelsPos = new List<Vector3>();
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

    public Vector2 Perlin;


    public override void OnUpdate()
    {
        //Simplex.Noise.Seed = 209323094; // Optional
        int length = 10, width = 15;
        float scale = 0.10f;
        //float[,] noiseValues = Simplex.Noise.Calc2D(length, width, scale);
        Console.WriteLine();

        //Random rnd = new Random();

       // gameEngine.SetPosition(new OpenTK.Vector3(gameEngine.GetPosition(Id).X, x, gameEngine.GetPosition(Id).Z), Id);

    }
}