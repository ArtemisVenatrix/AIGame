using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] Block DefaultGridMaterial;

    [Space]
    
    [Header("Dependencies")]
    [SerializeField] MovementController PlayerMovementController;
    public GameObject GridTemplate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNewGrid(int PlaceDistance, Vector3Int localOrientationOffset)
    {
        Vector3 offset = PlayerMovementController.gameObject.transform.forward * PlaceDistance;
        Vector3 targetLocation = offset + PlayerMovementController.gameObject.transform.position;
        GameObject temp = Instantiate(GridTemplate, transform);
        temp.transform.position = targetLocation;
        temp.transform.localEulerAngles = PlayerMovementController.transform.localEulerAngles;
        var tempGrid = temp.GetComponent<Grid>();
        temp.SetActive(true);
        tempGrid.RegisterBlock(DefaultGridMaterial, new Vector3Int(0, 0, 0), localOrientationOffset);
    }
}
