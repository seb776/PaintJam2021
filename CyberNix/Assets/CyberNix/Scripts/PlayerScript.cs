using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody Rigidbody;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump(float JumpHeight)
    {
        Rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
    }
}
