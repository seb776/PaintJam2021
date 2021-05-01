using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public MeshRenderer[] BodyParts;
    public float BlinkSpeed = .05f;
    public float InvicibleTime = 2f;

    private bool CanDie;

    // Start is called before the first frame update
    void Start()
    {
        CanDie = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanDie)
        {
            CanDie = false;
            StartCoroutine(Invulnerability());
        }
    }

    private IEnumerator Invulnerability()
    {
        var endTime = Time.time + InvicibleTime;
        while(Time.time < endTime)
        {
            foreach(MeshRenderer bodyPart in BodyParts)
            {
                bodyPart.enabled = !bodyPart.enabled;
                yield return new WaitForSeconds(BlinkSpeed);
            }
        }
        foreach (MeshRenderer bodyPart in BodyParts)
        {
            bodyPart.enabled = true;
        }
        CanDie = true;
    }

    public void Jump(float JumpHeight)
    {
        Rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
    }
}
