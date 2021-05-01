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
    public float DepthValue; // The distance for each "depth" count
    public int DepthCount;

    public GameObject Player;
    private int CurrentPlayerDepth;

    private List<GameObject> _currentGroundTiles;
    public int TileCount;

    void Start()
    {
        _currentGroundTiles = new List<GameObject>();

        for (int i = 0; i < TileCount; ++i)
        {
            float f = (float)(i - (TileCount / 2));
            var tileObj = GameObject.Instantiate(GroundTilePrefab, GroundTilesHolder.transform);
            var pos = tileObj.transform.position;
            tileObj.transform.position = new Vector3(f, pos.y, pos.z);
            float color = Mathf.Lerp(0.5f, 0.7f, Mathf.Repeat((float)i, 2.0f)); ;
            tileObj.gameObject.GetComponent<MeshRenderer>().material.color = new Color(color, color, color);
            _currentGroundTiles.Add(tileObj);
        }
    }

    private void _handleTiles()
    {
        foreach (var go in _currentGroundTiles)
        {
            if (go.transform.position.x < -(TileCount/2)) // if a tile is too left
            {
                _currentGroundTiles.Remove(go);
                go.transform.position = _currentGroundTiles.Last().transform.position + Vector3.right;
                _currentGroundTiles.Add(go);
                break;
            }
        }
    }

    void Update()
    {
        _handleTiles();
        GroundTilesHolder.transform.position += Vector3.left * Time.deltaTime * Speed;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentPlayerDepth--;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentPlayerDepth++;
        }
        var res = CurrentPlayerDepth * DepthValue;
        var pos = Player.transform.position;
        Player.transform.position = new Vector3(pos.x, pos.y, res);
    }
}
