using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
        if(transform.position.x < AppSingleton.Instance.LevelService.XLimit())
        {
            Destroy(gameObject);
        }
        Rigidbody.velocity = new Vector3(-Speed - (AppSingleton.Instance.LevelService.Speed / 3f), Rigidbody.velocity.y, Rigidbody.velocity.z);
        // TODO projectile rotation:
        // Texture.transform.localRotation = Quaternion.Euler(Texture.transform.localRotation.x, Texture.transform.localRotation.y, Texture.transform.localRotation.z + SpeedRotation * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetInstanceID() == AppSingleton.Instance.LevelService.Player.gameObject.GetInstanceID())
        {
            AppSingleton.Instance.LevelService.Player.TookDamage();
            Destroy(gameObject);
        }
    }
}
