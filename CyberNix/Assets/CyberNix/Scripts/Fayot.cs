using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fayot : MonoBehaviour
{
    public Rigidbody Projectile;
    public float TimeBetweenFire;
    public float Speed;

    private float _nextFire;
    private float _xStopPos;
    // Start is called before the first frame update
    void Start()
    {
        _nextFire = 0;
        _xStopPos = Random.Range(AppSingleton.Instance.LevelService.MobsSpawnXMin, AppSingleton.Instance.LevelService.MobsSpawnXMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > _xStopPos)
        {
            transform.position = new Vector3(transform.position.x - Speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            if (Time.time > _nextFire)
            {
                GameObject.Instantiate(Projectile.gameObject, new Vector3(transform.position.x - (transform.localScale.x), transform.position.y, transform.position.z), Projectile.transform.rotation);
                _nextFire = Time.time + TimeBetweenFire;
            }
        }
    }
}
