using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cassoulax : MonoBehaviour
{
    public float probaMove;
    public float probaJump;
    public float probaFire;
    public Rigidbody Rigidbody;
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
        }
    }

    public void Jump()
    {
        Rigidbody.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
    }

    public void MoveAt(float z)
    {
        Debug.Log(z);
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, z);
    }

    public void TookDamage()
    {
        if(CanDie)
        {
            LifeNumber--;
            if(LifeNumber < 1)
            {

            }
        }
    }
}
