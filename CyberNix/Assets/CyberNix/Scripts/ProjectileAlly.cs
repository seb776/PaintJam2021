using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAlly : MonoBehaviour
{
    public Rigidbody Rigidbody;
    public float Speed;
    public float SpeedRotation;
    public GameObject Texture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > AppSingleton.Instance.LevelService.XLimit() * -1)
        {
            Destroy(gameObject);
        }
        Rigidbody.velocity = new Vector3(Speed, Random.Range(-1f, 1f), Rigidbody.velocity.z);
        // TODO projectile rotation:
        // Texture.transform.localRotation = Quaternion.Euler(Texture.transform.localRotation.x, Texture.transform.localRotation.y, Texture.transform.localRotation.z + SpeedRotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        foreach(GameObject mob in AppSingleton.Instance.LevelService.GetMobsAlive() )
        {
            if(mob.GetInstanceID() == collision.gameObject.GetInstanceID())
            {
                mob.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
