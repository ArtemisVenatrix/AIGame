using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Blocks;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class Grid : MonoBehaviour
{
    [Header("Dependencies")] 
    [SerializeField] public GridScaleController gridScaleController;
    public string MyName { get; private set; }
    
    private Dictionary<Vector3Int, Block> MyGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        MyName = name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3Int GetLocalPos(Block given)
    {
        return MyGrid.FirstOrDefault(x => x.Value.Equals(given)).Key;
    }

    public Block GetBlockByPos(Vector3Int given)
    {
        return MyGrid[given];
    }

    public bool RegisterBlock(Block block, Vector3Int localPos, Vector3Int localOrientation)
    {
        List<Vector3Int> projectedPoints = GeneratePoints(block, localPos);
        foreach (var point in projectedPoints)
        {
            if (MyGrid.ContainsKey(point))
            {
                return false;
            }
        }
        
        var temp = block.gameObject;
        GameObject tempClone = Instantiate(temp);
        Block tempCloneCore = tempClone.GetComponent<Block>();
        tempClone.transform.parent = transform;
        tempCloneCore.LocalPos = localPos;
        tempCloneCore.SetDimensions(localOrientation);
        tempCloneCore.SetParentGrid(this);
        tempCloneCore.transform.localEulerAngles = new Vector3();
        tempClone.transform.position += transform.position + (Vector3)(localPos)*gridScaleController.globalWorldSize;
        tempClone.SetActive(true);

        foreach (var point in projectedPoints)
        {
            MyGrid[point] = tempCloneCore;
        }
        
        return true;
    }

    public bool RemoveBlock(Vector3Int pos)
    {
        if(MyGrid.ContainsKey(pos))
        {
            MyGrid.Remove(pos);
            return true;
        }
        
        return false;
    }

    public bool RemoveBlock(Block block)
    {
        Vector3Int pos = MyGrid.FirstOrDefault(x => x.Value.Equals(block)).Key;
        if(MyGrid.ContainsKey(pos))
        {
            MyGrid.Remove(pos);
            return true;
        }

        return false;
    }

    public Dictionary<Vector3Int, Block> GetAdjacentBlocks(Block target)
    {
        if (!MyGrid.ContainsValue(target))
        {
            throw new Exception("Foreign Block Referenced!");
        }
        
        Dictionary<Vector3Int, Block> output = new Dictionary<Vector3Int, Block>();
        List<Vector3Int> points = GeneratePoints(target, target.LocalPos);
        foreach (var point in points)
        {
            List<Vector3Int> currentlyAdjacentPoints = GetAdjacentPoints(point);
            foreach (var adjacentPoint in currentlyAdjacentPoints)
            {
                if (!points.Contains(adjacentPoint) && MyGrid.ContainsKey(adjacentPoint) &&
                    !output.ContainsValue(MyGrid[adjacentPoint]))
                {
                    output[adjacentPoint] = MyGrid[adjacentPoint];
                }
            }
        }
        return output;
    }

    private List<Vector3Int> GeneratePoints(Block block, Vector3Int localRootPos)
    {
        List<Vector3Int> output = new List<Vector3Int>();
        Vector3Int rotatedDimensions = RotateDimensions(block.Dimensions, block.LocalOrientation);
        Debug.Log(rotatedDimensions);
        for (int i = 0; i < block.Dimensions.x; i++)
        {
            for (int j = 0; j < block.Dimensions.y; j++)
            {
                for (int k = 0; k < block.Dimensions.z; k++)
                {
                    Vector3Int currentPos = new Vector3Int();
                    currentPos.x = localRootPos.x + i;
                    currentPos.y = localRootPos.y + j;
                    currentPos.z = localRootPos.z + k;
                    output.Add(currentPos);
                }
            }
        }

        return output;
    }

    private Vector3Int RotateDimensions(Vector3Int dimensions, Vector3Int localOrientation)
    {
        PrismProjection temp = new PrismProjection(dimensions);
        temp.RotateShape(localOrientation);
        return temp.GetDimensions();
    }

    private List<Vector3Int> GetAdjacentPoints(Vector3Int origin)
    {
        List<Vector3Int> output = new List<Vector3Int>();
        output.Add(new Vector3Int(origin.x + 1, origin.y, origin.z));
        output.Add(new Vector3Int(origin.x - 1, origin.y, origin.z));
        output.Add(new Vector3Int(origin.x, origin.y + 1, origin.z));
        output.Add(new Vector3Int(origin.x, origin.y - 1, origin.z));
        output.Add(new Vector3Int(origin.x, origin.y, origin.z + 1));
        output.Add(new Vector3Int(origin.x, origin.y, origin.z - 1));
        return output;
    }

    public Vector3Int FindNearestLocalPoint(Vector3 worldPoint, Block target)
    {
        var targetPos = MyGrid.FirstOrDefault(x => x.Value.Equals(target)).Key;
        List<Vector3Int> adjacentPoints = GetAdjacentPoints(targetPos);
        foreach (var point in adjacentPoints)
        {
            if (MyGrid.ContainsKey(point))
            {
                adjacentPoints.Remove(point);
            }
        }

        float minimumDistance = float.MaxValue;
        Vector3Int closestLocalPoint = new Vector3Int();
        foreach (var point in adjacentPoints)
        {
            var adjacentWorldPoint = ConvertLocalToWorldPoint(point);

            if (Math.Min(minimumDistance, Vector3.Distance(adjacentWorldPoint, worldPoint)) != minimumDistance)
            {
                closestLocalPoint = point;
                minimumDistance = Math.Min(minimumDistance, Vector3.Distance(adjacentWorldPoint, worldPoint));
            }
        }

        return closestLocalPoint;
    }

    public Vector3 ConvertLocalToWorldPoint(Vector3Int pos)
    {
        return transform.position + (Vector3)(pos)*gridScaleController.globalWorldSize;
    }
}
