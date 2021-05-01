using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float Speed;
    public float Amplitude;
    public float Offset;

    private Vector3 _originalLocalPos;
    private void Start()
    {
        _originalLocalPos = this.gameObject.transform.localPosition;
    }
    void Update()
    {
        float t = Time.realtimeSinceStartup;
        this.gameObject.transform.localPosition = _originalLocalPos
            + new Vector3(Mathf.Sin(t * Speed+ Offset) + Mathf.Sin(t * Speed * 1.3f+ Offset) * 0.5f + Mathf.Sin(t * Speed * 2.3f + Offset) * 0.25f,
            Mathf.Sin(5.0f+t * Speed + Offset) + Mathf.Sin(3.0f+t * Speed * 1.3f + Offset) * 0.5f + Mathf.Sin(1.5f + t * Speed * 2.3f + Offset) * 0.25f, 
            0.0f) * Amplitude;
    }
}
