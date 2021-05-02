using System.Collections.Generic;
using UnityEngine;

public class Cassoulax : MonoBehaviour
{
    public float probaMove;
    public float probaJump;
    public float probaFire;
    public Rigidbody Rigidbody;
    public List<GameObject> Projectiles;
    public float JumpHeight;
    public float CanJumpAtY;

    public float CommingSpeed;
    public float XStopPos;

    public int LifeNumber;

    private float _nextFire;
    private bool CanDie;
    private float _currentPosition;
    private LevelService _Level { get { return AppSingleton.Instance.LevelService; } }
    // Start is called before the first frame update
    void Start()
    {
        CanDie = false;
        _nextFire = 0;
        _currentPosition = transform.position.z / _Level.DepthValue;
        AppSingleton.Instance.SoundService.PlayCassoulet();
    }

    void Update()
    {
        if (transform.position.x > XStopPos)
        {
            transform.position = new Vector3(transform.position.x - CommingSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            CanDie = true;
        }
    }

    // Update is called once per frame
    public void DoAction()
    {
        if (CanDie)
        {
            if (Random.Range(0f, 1f) < probaMove)
            {
                _currentPosition += Random.Range(-1, 2);
                if (_currentPosition < 0) _currentPosition = 0;
                if (_currentPosition > _Level.DepthCount - 1) _currentPosition = _Level.DepthCount - 1;
                MoveAt(_currentPosition * _Level.DepthValue);
            }
            if(Random.Range(0f, 1f) < probaJump && transform.position.y < CanJumpAtY)
            {
                Jump();
            }
            if(Random.Range(0f, 1f) < probaFire)
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        if(Projectiles.Count > 0)
        {
            GameObject proj = Projectiles[Random.Range(0, Projectiles.Count)];
            GameObject.Instantiate(proj, new Vector3(transform.position.x - (transform.localScale.x / 2f), transform.position.y, transform.position.z), proj.transform.rotation);
        }
    }

    public void Jump()
    {
        Rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
    }

    public void MoveAt(float z)
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, z);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            TookDamage();
            Destroy(collision.gameObject);
        }
    }

    public void TookDamage()
    {
        if(CanDie)
        {
            LifeNumber--;
            if(LifeNumber < 1)
            {
                var gainPoint = GameObject.Instantiate(_Level.GaindPointPrefab, _Level.GroundTilesHolder.transform);
                var scriptPoint = gainPoint.GetComponent<GainPoint>();
                gainPoint.transform.position = gameObject.transform.position;
                scriptPoint.Gain(_Level.ScoreOnKill);
                gameObject.SetActive(false);
            }
        }
    }
}
