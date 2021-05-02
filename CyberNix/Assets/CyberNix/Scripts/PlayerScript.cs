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
    public float SpeedLostOnDamage;

    public float JumpHeight;
    public float Inclinaison;
    public float HightSpeedGap;
    public float JumpHeightHightSpeed;
    public float InclinaisonHighSpeed;

    public float FireCouldown;

    public GameObject Projectile;

    private bool CanDie;
    private float _nextFireAfter;
    public bool GameOver;

    public GameObject EndGameScreen;

    // Start is called before the first frame update
    void Start()
    {
        _nextFireAfter = 0;
        GameOver = false;
        CanDie = true;
        if (LifeNumber < 1) LifeNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > AppSingleton.Instance.LevelService.GroundTheshold)
        {
            if (Rigidbody.velocity.y > 0 || Rigidbody.velocity.y < 0)
            {
                if (AppSingleton.Instance.LevelService.Speed < AppSingleton.Instance.LevelService.MaxSpeed * HightSpeedGap)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Rigidbody.velocity.y * Inclinaison);
                } else {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Rigidbody.velocity.y * InclinaisonHighSpeed);
                }
            }
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TookDamage();
    }

    public void FireAt()
    {
        if (Time.time > _nextFireAfter)
        {
            GameObject.Instantiate(Projectile.gameObject, new Vector3(transform.position.x + (transform.localScale.x / 1.9f), transform.position.y, transform.position.z), Projectile.transform.rotation);
            _nextFireAfter = Time.time + FireCouldown;
        }
    }

    public void TookDamage()
    {
        if (CanDie)
        {
            CanDie = false;
            LifeNumber--;
            AppSingleton.Instance.LevelService.Speed = AppSingleton.Instance.LevelService.Speed * (1 - SpeedLostOnDamage);
            if (LifeNumber < 1)
            {
                GameOver = true;
                gameObject.SetActive(false); // TODO : make better game over
                EndGameScreen.SetActive(true);
            }
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

    public void Jump()
    {
        if (AppSingleton.Instance.LevelService.Speed < AppSingleton.Instance.LevelService.MaxSpeed * HightSpeedGap)
        {
            Rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
        }
        else
        {
            Rigidbody.AddForce(Vector3.up * JumpHeightHightSpeed, ForceMode.VelocityChange);
        }
        
    }

    public void MoveAt(float z)
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, z);
    }
}
