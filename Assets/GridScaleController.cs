using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScaleController : MonoBehaviour
{
    public float globalWorldSize { get; private set; }
    private void Awake()
    {
        var meshBounds = gameObject.GetComponent<MeshRenderer>().bounds;
        globalWorldSize = meshBounds.size.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
