using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fayot : MonoBehaviour
{
    public Rigidbody Projectile;
    public float TimeBetweenFire;
    public float Speed;
    public TextMesh Text;


    private float _nextFire;
    private float _xStopPos;

    private bool CanDie;
    private LevelService _Level { get { return AppSingleton.Instance.LevelService; } }
    // Start is called before the first frame update
    void Start()
    {
        Text.text = AppSingleton.Instance.LevelService.GetName();
        Text.color = AppSingleton.Instance.LevelService.NameColors[Random.Range(0, AppSingleton.Instance.LevelService.NameColors.Count)];
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1.0f);
        CanDie = false;
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
            CanDie = true;
            if (Time.time > _nextFire)
            {
                GameObject.Instantiate(Projectile.gameObject, new Vector3(transform.position.x - (transform.localScale.x), transform.position.y, transform.position.z), Projectile.transform.rotation);
                _nextFire = Time.time + TimeBetweenFire;
            }
        }
    }

    public void TookDamage()
    {
        if(CanDie)
        {
            var gainPoint = GameObject.Instantiate(_Level.GaindPointPrefab, _Level.GroundTilesHolder.transform);
            var scriptPoint = gainPoint.GetComponent<GainPoint>();
            gainPoint.transform.position = gameObject.transform.position;
            scriptPoint.Gain(_Level.ScoreOnKill);
            gameObject.SetActive(false);
            AppSingleton.Instance.SoundService.PlayExplode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
            TookDamage();
        }
    }
}
