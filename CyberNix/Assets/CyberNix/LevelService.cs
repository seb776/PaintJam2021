using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class LevelService : MonoBehaviour
{
    public GameObject GroundTilesHolder;
    public GameObject GroundTilePrefab;

    public GameObject GaindPointPrefab;

    public GameObject LevelInstances; // holds object seen in the camera frame
    public float Speed;
    public float Acceleration;
    public float MaxSpeed;
    public float MinPitch;
    public float MaxPitch;
    public float DepthValue; // The distance for each "depth" count
    public float ObstacleSpawnChance = 0.15f;
    public int DepthCount;
    public float GroundTheshold;
    public AudioSource Music;
    public List<GameObject> Prefabs;
    public TextMeshProUGUI ScoreText;

    public PlayerScript Player;
    private int CurrentPlayerDepth;

    private List<GameObject> _currentGroundTiles;
    private List<GameObject> _landscapeObjects;
    public GameObject CandlePrefab;
    public List<GameObject> CloseBuildingPrefabs;
    public int TileCount;

    public int Score;
    public int ScoreMobsSpawn;
    public int ScoreOnKill;
    public int ScoreOnHealth;
    public int ScoreOnBoss;
    public int ScoreByTilePassed;
    public int StartPhase;
    public int WaveBeforeBoss;

    public int MobsAtSameTime;
    public int MobsTotalNumbers;
    public List<GameObject> BasicEnnemies;
    public List<GameObject> Boss;
    public float MobsSpawnXMin;
    public float MobsSpawnXMax;
    public float MobSpawnChance;

    private GameObject _waveOfThis;
    private int _mobsSpawnIn;
    private int _mobsDead;
    private int _phase; //0: Obstacle, 1: Mobs, ...
    private int _waveCount;
    private Cassoulax _actualBoss;

    private List<GameObject> _mobsAlive;

    public List<string> FirstNames;
    public List<string> LastNames;
    public List<Color> NameColors;

    public string GetName()
    {
        return $"{FirstNames[Random.Range(0, FirstNames.Count)]} {LastNames[Random.Range(0, LastNames.Count)]}".ToUpper();
    }

    void Start()
    {
        Score = 0;
        _phase = StartPhase;
        _mobsSpawnIn = ScoreMobsSpawn;
        _landscapeObjects = new List<GameObject>();
        _currentGroundTiles = new List<GameObject>();
        _mobsAlive = new List<GameObject>();
        _waveCount = 0;

        for (int i = 0; i < TileCount; ++i)
        {
            float f = (float)(i - (TileCount / 2));
            var tileObj = GameObject.Instantiate(GroundTilePrefab, GroundTilesHolder.transform);
            var pos = tileObj.transform.position;
            tileObj.transform.position = new Vector3(f, pos.y, pos.z);
            float color = Mathf.Lerp(0.5f, 0.7f, Mathf.Repeat((float)i, 2.0f));
            tileObj.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color, color, color);
            _currentGroundTiles.Add(tileObj);
        }
    }

    private void _mobsManager()
    {
        //TODO : check if mobs spawned are dead, respawn if wave not complete or turn _phase at 0 to return to obstacle mode !
        List<GameObject> shallowMobsAlive = new List<GameObject>(_mobsAlive);
        foreach(GameObject mob in shallowMobsAlive)
        {
            if(!mob.activeSelf)
            {
                _mobsAlive.Remove(mob);
                GameObject.Destroy(mob);
                Score += ScoreOnKill;
                _mobsDead++;
            }
        }
        if(_mobsDead >= MobsTotalNumbers)
        {
            _waveCount++;
            _phase = 0;
            _mobsSpawnIn = ScoreMobsSpawn;
        } else if(_mobsAlive.Count < MobsAtSameTime && Random.Range(0f, 1f) < MobSpawnChance)
        {
            _mobsAlive.Add(SpawnOne(_waveOfThis));
        }
    }

    private GameObject SpawnOne(GameObject toSpawn)
    {
        return GameObject.Instantiate(toSpawn, new Vector3(-XLimit(), toSpawn.transform.position.y, GetRandomLine()), toSpawn.transform.rotation);
    }

    private void _mobsSpawner()
    {
        if (BasicEnnemies.Count > 0) {
            _mobsDead = 0;
            _waveOfThis = BasicEnnemies[Random.Range(0, BasicEnnemies.Count)];
            GameObject mob = SpawnOne(_waveOfThis);
           _mobsAlive.Add(mob);
        }
    }

    private float GetRandomLine()
    {
        return Random.Range(0, DepthCount) * DepthValue;
    }

    public float XLimit()
    {
        return -(TileCount / 2);
    }

    public List<GameObject> GetMobsAlive()
    {
        return _mobsAlive;
    }

    private void _spawnBoss()
    {
        _waveCount = 0;
        if(Boss.Count != 0)
        {
            _actualBoss = SpawnOne(Boss[Random.Range(0, Boss.Count)]).GetComponent<Cassoulax>();
        }
    }

    private void _boss()
    {
        _actualBoss.DoAction();
        if(!_actualBoss.gameObject.activeSelf)
        {
            Destroy(_actualBoss);
            _phase = 0;
            Score += ScoreOnBoss;
        }
    }

    private void _handleTiles()
    {
        List<GameObject> tmpGround = new List<GameObject>(_currentGroundTiles);
        foreach (var go in tmpGround)
        {
            if (go.transform.position.x < -(TileCount / 2)) // if a tile is too left
            {
                _currentGroundTiles.Remove(go);
                go.transform.position = _currentGroundTiles.Last().transform.position + Vector3.right;
                foreach (Transform child in go.transform) // clean child of actual tile
                {
                    GameObject.Destroy(child.gameObject);
                }
                switch(_phase)
                {
                    case 0:
                        if (Random.Range(0f, 1f) < ObstacleSpawnChance)
                        {
                            if (Prefabs.Count > 0)
                            {
                                var futurePrefab = GameObject.Instantiate(Prefabs[Random.Range(0, Prefabs.Count)], go.transform);
                                futurePrefab.transform.position = new Vector3(futurePrefab.transform.position.x, futurePrefab.transform.position.y, GetRandomLine());
                            }
                        }
                        break;
                    case 1:
                        _mobsSpawner();
                        _phase = 2;
                        break;
                    case 2:
                        _mobsManager();
                        break;
                    case 3:
                        _spawnBoss();
                        _phase = 4;
                        break;
                    case 4:
                        _boss();
                        break;
                }
                
                _currentGroundTiles.Add(go);
                if (!Player.GameOver)
                {
                    if(_phase == 0) Score += ScoreByTilePassed;
                    if (_phase != 1 && _phase != 2 && _phase != 3 && _phase != 4)
                    {
                        _mobsSpawnIn--;
                        if (_mobsSpawnIn < 0)
                        {
                            if (_waveCount >= WaveBeforeBoss)
                            {
                                _phase = 3;
                            }
                            else
                            {
                                _phase = 1;
                            }
                        }
                    }
                    ScoreText.text = Score.ToString();
                }
            }
        }

        if (Mathf.Repeat(Time.realtimeSinceStartup * Speed, 5.0f) + Random.Range(0f, 0.5f) < 0.1)
        {
            var candle = GameObject.Instantiate(CandlePrefab, GroundTilesHolder.transform);
            candle.transform.position = new Vector3((float)(TileCount / 2), candle.transform.position.y, candle.transform.position.z);
            _landscapeObjects.Add(candle);
        }
        if ((Mathf.Repeat(Time.realtimeSinceStartup * Speed+Random.Range(0f, 0.5f), 5.0f)) < 0.005 && CloseBuildingPrefabs != null && CloseBuildingPrefabs.Count > 0)
        {

            var candle = GameObject.Instantiate(CloseBuildingPrefabs[Random.Range(0, CloseBuildingPrefabs.Count)], GroundTilesHolder.transform);
            candle.transform.position = new Vector3((float)(TileCount / 2), candle.transform.position.y, candle.transform.position.z);
            _landscapeObjects.Add(candle);
        }

        List<GameObject> toRemoveLandscape = new List<GameObject>();
        foreach (var go in _landscapeObjects)
        {
            if (go.transform.position.x < -(TileCount / 2)) // if is too left
            {
                toRemoveLandscape.Add(go);
            }
        }
        foreach (var toRem in toRemoveLandscape)
        {
            Destroy(toRem);
            _landscapeObjects.Remove(toRem);
        }
    }
    public List<GameObject> BackgroundPlanes;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AppSingleton.Instance.Menu();
        }
        _handleTiles();
        GroundTilesHolder.transform.position += Vector3.left * Time.deltaTime * Speed;
        if(Speed < MaxSpeed) Speed += Acceleration * Time.deltaTime;
        Music.pitch = Mathf.Lerp(1, MaxPitch, Speed / MaxSpeed);
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(CurrentPlayerDepth > 0) CurrentPlayerDepth--;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(CurrentPlayerDepth < DepthCount - 1) CurrentPlayerDepth++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && Player.transform.position.y < GroundTheshold)
        {
            Player.Jump();
        }
        if(Input.GetKeyDown(KeyCode.C) && !Player.GameOver)
        {
            Player.FireAt();
        }
        Player.MoveAt(CurrentPlayerDepth * DepthValue);

        // BackgroundPlanesScroll
        if (BackgroundPlanes != null)
        {
            float fplane = 1.0f;
            foreach (var back in BackgroundPlanes)
            {
                var backmat = back.GetComponent<MeshRenderer>().material;
                backmat.SetVector("_MainTex_ST", new Vector4(2.0f, 1.0f, backmat.GetVector("_MainTex_ST").z + 0.025f*Time.deltaTime*Speed*fplane));
                fplane += 1.0f;
            }
        }
    }
}
