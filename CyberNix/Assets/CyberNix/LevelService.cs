using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VectorSwizzle;

public class LevelService : MonoBehaviour
{
    public GameObject GroundTilesHolder;
    public GameObject GroundTilePrefab;

    public GameObject LevelInstances; // holds object seen in the camera frame
    public float Speed;
    public float JumpHeight;
    public float DepthValue; // The distance for each "depth" count
    public float ObstacleSpawnChance = 0.15f;
    public int DepthCount;
    public List<GameObject> Prefabs;

    public PlayerScript Player;
    private int CurrentPlayerDepth;

    private List<GameObject> _currentGroundTiles;
    private List<GameObject> _landscapeObjects;
    public GameObject CandlePrefab;
    public List<GameObject> CloseBuildingPrefabs;
    public int TileCount;

    void Start()
    {
        _landscapeObjects = new List<GameObject>();
        _currentGroundTiles = new List<GameObject>();

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

    private void _handleTiles()
    {
        foreach (var go in _currentGroundTiles)
        {
            if (go.transform.position.x < -(TileCount / 2)) // if a tile is too left
            {
                _currentGroundTiles.Remove(go);
                go.transform.position = _currentGroundTiles.Last().transform.position + Vector3.right;
                foreach (Transform child in go.transform) // clean child of actual tile
                {
                    GameObject.Destroy(child.gameObject);
                }
                if (Random.Range(0f, 1f) < ObstacleSpawnChance)
                {
                    if (Prefabs.Count > 0)
                    {
                        var futurePrefab = GameObject.Instantiate(Prefabs[Random.Range(0, Prefabs.Count)], go.transform);
                        futurePrefab.transform.position = new Vector3(futurePrefab.transform.position.x, futurePrefab.transform.position.y, Random.Range(0, DepthCount) * DepthValue);
                    }
                }
                _currentGroundTiles.Add(go);
                break;
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
            if (go.transform.position.x < -(TileCount / 2)) // if is too left)
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
        _handleTiles();
        GroundTilesHolder.transform.position += Vector3.left * Time.deltaTime * Speed;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(CurrentPlayerDepth > 0) CurrentPlayerDepth--;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(CurrentPlayerDepth < DepthCount - 1) CurrentPlayerDepth++;
        }
        if (Input.GetKeyDown(KeyCode.Space) && Player.transform.position.y < .66f)
        {
            Player.Jump(JumpHeight);
        }
        Debug.Log(CurrentPlayerDepth);
        var res = CurrentPlayerDepth * DepthValue;
        var pos = Player.transform.position;
        Player.transform.position = new Vector3(pos.x, pos.y, res);

        // BackgroundPlanesScroll
        if (BackgroundPlanes != null)
        {
            float fplane = 1.0f;
            foreach (var back in BackgroundPlanes)
            {
                back.GetComponent<MeshRenderer>().material.SetVector("_MainTex_ST", new Vector4(2.0f, 1.0f, Time.realtimeSinceStartup * Speed*0.025f*fplane));
                fplane += 1.0f;
            }
        }
    }
}
