using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using Noise;
using Noise.Modules.Generator;
using Noise.Modules.Operator;

/// <summary>
/// 
/// By: Charles Humphrey @NemoKrad
/// On: 25/06/2015
/// 
/// </summary>
[ExecuteInEditMode]
public class NodeScript : MonoBehaviour 
{
    public Dictionary<string, List<Type>> AvailableNodeTypes = new Dictionary<string, List<Type>>();

    public object root;

    public NodeScript()
    {
        AvailableNodeTypes.Clear();

        //AvailableNodeTypes.Add("Test", new List<Type>(new Type[]{
        //        typeof(Node)
        // }));

        AvailableNodeTypes.Add("Generator", new List<Type>(new Type[]{
                typeof(Billow),
                typeof(Checker),
                typeof(Const),
                typeof(Cylinders),
                typeof(Perlin),
                typeof(RidgedMultifractal),
                typeof(Spheres),
                typeof(Voronoi)
            }));

        AvailableNodeTypes.Add("Operator", new List<Type>(new Type[]{
                typeof(Abs),
                typeof(Add),
                typeof(Blend),
                typeof(Cache),
                typeof(Clamp),
                typeof(Curve),
                typeof(Displace),
                typeof(Exponent),
                typeof(Invert),
                typeof(Max),
                typeof(Min),
                typeof(Multiply),
                typeof(Power),
                typeof(Rotate),
                typeof(Scale),
                typeof(ScaleBias),
                typeof(Select),
                typeof(Subtract),
                typeof(Terrace),
                typeof(Translate),
                typeof(Turbulence)
            }));
    }
    
	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
    void Update()
    {
    }
}

