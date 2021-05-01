using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public MeshRenderer[] BodyParts;
    public float BlinkSpeed = .05f;
    public float InvicibleTime = 2f;
    public int LifeNumber = 3;

    private bool CanDie;

    // Start is called before the first frame update
    void Start()
    {
        CanDie = true;
        if (LifeNumber < 1) LifeNumber = 1;
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
            LifeNumber--;
            if (LifeNumber < 1) gameObject.SetActive(false); // TODO : make better game over
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

    public void MoveAt(float z)
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, z);
    }
}
