using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [Header("Constants")]
    public float maxInteractDistance;

    [Space]
    
    [Header("Controls")] public KeyCode interact = KeyCode.E;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interact))
        {
            RaycastHit temp = Interact();
            Debug.Log("Collider Hit: " + temp.collider);
            Debug.Log("Distance From Camera: " + temp.distance);
            Debug.Log("Target Location: " + temp.point);
            ModifyTerrain(temp.collider, temp.point);
        }
    }

    private RaycastHit Interact()
    {
        RaycastHit temp;
        Physics.Raycast(transform.position, transform.forward, out temp, maxInteractDistance);
        return temp;
    }

    private void ModifyTerrain(Collider target, Vector3 location)
    {
        if (target is TerrainCollider)
        {
            GameObject terrainGO = target.gameObject;
            Terrain terrain = terrainGO.GetComponent<Terrain>();
            Vector3 normedPos = location - terrainGO.transform.position;
            Vector3 coords;
            coords.x = normedPos.x / terrain.terrainData.size.x;
            coords.y = normedPos.y / terrain.terrainData.size.y;
            coords.z = normedPos.z / terrain.terrainData.size.z;

            int posXInTerrain = (int) (coords.x / terrain.terrainData.heightmapWidth);
            int posYInTerrain = (int) (coords.z / terrain.terrainData.heightmapHeight);
            float[,] heights = terrain.terrainData.GetHeights(posXInTerrain, posYInTerrain, 50, 60);
            for (int i = 0; i < heights.GetLength(0); i++)
            {
                for (int j = 0; j < heights.GetLength(1); j++)
                {
                    Debug.Log(heights[i, j]);
                }
            }
            //terrain.terrainData.SetHeights((int)Math.Round(location.x),(int)Math.Round(location.y), heights);
        }
    }
}
