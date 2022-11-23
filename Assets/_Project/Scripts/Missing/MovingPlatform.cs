using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<MMPathMovementElement> PathElements;
}


public class MMPathMovementElement
{
    /// the point that make up the path the object will follow
    public Vector3 PathElementPosition;
    /// a delay (in seconds) associated to each node
    public float Delay;
}
