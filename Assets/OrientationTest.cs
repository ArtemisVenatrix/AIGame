using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationTest : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Controls")]
    public KeyCode XUp;
    public KeyCode XDown;
    public KeyCode YUp;
    public KeyCode YDown;
    public KeyCode ZUp;
    public KeyCode ZDown;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RotateXUp()
    {
        transform.Rotate(90, 0, 0);
    }
}
