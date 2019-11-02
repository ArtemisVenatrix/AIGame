using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Controls")]
    public KeyCode AddBlockKey = KeyCode.Mouse0;
    public KeyCode RemoveBlockKey = KeyCode.Mouse1;
    public KeyCode BuildMenuToggle = KeyCode.G;
    
    [Space]
    
    [Header("Settings")]
    public int PlaceDistance;
    
    [Space]
    
    [Header("Dependencies")]
    public GameObject BuildMenu;
    [SerializeField] public MovementController PlayerMovementController;
    [SerializeField] public GridController GridControllerRef;
    
    private Block selectedBuildMaterial;
    private bool buildMenuState;
    private Vector3Int localOrientationOffset;

    
    void Start()
    {
        buildMenuState = false;
        localOrientationOffset = new Vector3Int();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(AddBlockKey))
        {
            AddBlock();
        }

        if (Input.GetKeyDown(RemoveBlockKey))
        {
            RemoveBlock();
        }
        
        if (Input.GetKeyDown(BuildMenuToggle))
        {
            ToggleBuildMenu();
        }
    }
    
    public void SetBuildMaterial(Block material)
    {
        selectedBuildMaterial = material;
    }
    
    private void ToggleBuildMenu()
    {
        buildMenuState = !buildMenuState;
        BuildMenu.SetActive(buildMenuState);
        PlayerMovementController.SetLockInput(buildMenuState);
    }

    public void CreateNewGrid()
    {
        GridControllerRef.CreateNewGrid(PlaceDistance, localOrientationOffset);
    }
    
    private RaycastHit Cast()
    {
        RaycastHit temp;
        Physics.Raycast(transform.position, transform.forward, out temp, PlaceDistance);
        return temp;
    }

    private void AddBlock()
    {
        try
        {
            var hit = Cast();
            Block target = hit.collider.gameObject.GetComponent<Block>();
            Vector3Int targetLocalPos = target.ParentGrid.FindNearestLocalPoint(hit.point, target);
            target.ParentGrid.RegisterBlock(selectedBuildMaterial, targetLocalPos, localOrientationOffset);
        }
        catch (Exception ex)
        {
            
        }
    }

    private void RemoveBlock()
    {
        var target = Cast();
    }
}
