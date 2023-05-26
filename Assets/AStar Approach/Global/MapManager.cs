using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }


    [SerializeField]
    private int rows = 10;
    [SerializeField]
    private int columns = 10;
    [SerializeField]
    private int scale = 1;

    public GameObject tilePrefab;
    public GameObject overlayPrefab;

    public Dictionary<Vector3Int, OverlayTile> map;


    private void Start()
    {
        
    }

    void Awake()
    {
        map = new Dictionary<Vector3Int, OverlayTile>();
        if (_instance != null && _instance==this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            GenerateTiles();
        }
    }


    void Update()
    {

    }

    void GenerateTiles()
    {
        for (int z = 0; z<columns; z++)
        {
            for (int x = 0; x<rows; x++)
            {

                Vector3 tilePos = new Vector3(x*scale, 0f, z*scale);
                Vector3 overlayPos = new Vector3(x*scale, 0.55f, z*scale);
                Vector3Int tileKey = new Vector3Int(x*scale, 0, z*scale);

                if (!map.ContainsKey(tileKey))
                {

                    var tile = Instantiate(tilePrefab, tilePos, Quaternion.identity);
                    var overlay = Instantiate(overlayPrefab, overlayPos, Quaternion.identity);

                    // make children of GameManager
                    tile.transform.parent = transform;
                    overlay.transform.parent = transform;

                    // assigning map values.
                    overlay.GetComponent<OverlayTile>().gridLocation = tileKey;
                    map.Add(tileKey, overlay.GetComponent<OverlayTile>());

                    // hiding overlayTiles
                    overlay.GetComponent<OverlayTile>().HideOverlay();
                }


            }
        }
    }
}
