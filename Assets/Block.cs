using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    public string ID { get; private set; }
    public Vector3Int LocalPos;
    public Vector3Int LocalOrientation { get; private set; } //set to 0 1 2 3 in each pos
    public Vector3Int Dimensions { get; private set; }
    public Grid ParentGrid { get; private set; }
    
    // Start is called before the first frame update

    void Start()
    {
        ID = name;
        transform.Rotate(LocalOrientation * 90);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetParentGrid(Grid target)
    {
        if (ParentGrid == null)
        {
            ParentGrid = target;
            return true;
        }

        return false;
    }

    public void SetLocalOrientation(Vector3Int input)
    {
        Vector3Int output = new Vector3Int();
        output.x = Math.Abs(input.x) % 4;
        output.y = Math.Abs(input.y) % 4;
        output.z = Math.Abs(input.z) % 4;
        LocalOrientation = output;
    }

    public void SetDimensions(Vector3Int input)
    {
        Vector3Int output = new Vector3Int();
        output.x = (int) Mathf.Clamp(input.x, 1, Mathf.Infinity);
        output.y = (int) Mathf.Clamp(input.y, 1, Mathf.Infinity);
        output.z = (int) Mathf.Clamp(input.z, 1, Mathf.Infinity);
        Dimensions = output;
    }
}
