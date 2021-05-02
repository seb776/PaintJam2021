using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fayot : MonoBehaviour
{
    public Rigidbody Projectile;
    public float TimeBetweenFire;
    public float BeginFireAfter;

    private float _nextFire;
    // Start is called before the first frame update
    void Start()
    {
        _nextFire = Time.time + BeginFireAfter;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _nextFire)
        {
            GameObject.Instantiate(Projectile.gameObject, new Vector3(transform.position.x - (transform.localScale.x), transform.position.y, transform.position.z), Projectile.transform.rotation);
            _nextFire = Time.time + TimeBetweenFire;
        }
    }
}
