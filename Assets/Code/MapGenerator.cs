using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    uint width;
    [SerializeField]
    uint height;

    [SerializeField]
    Transform mapRoot;

    [Range(0f, 1f)]
    [SerializeField]
    float fillPercent;

    [SerializeField]
    int seed;

    [SerializeField]
    bool useRandomSeed = false;

    int[,] map;

    Transform thisTransform;

    void Awake()
    {
        thisTransform = this.transform;
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }
    }

    void ClearMap()
    {
        if (mapRoot != null)
        {
            for (int i = 0; i < mapRoot.childCount; i++)
            {
                GameObject.Destroy(mapRoot.GetChild(i).gameObject);
            }
        }
    }

    void GenerateMap()
    {
        ConfigureRandomSeed();
        ClearMap();
        map = BuildMapGrid();
        GenerateGameObjects(map);
    }

    void ConfigureRandomSeed()
    {
        Random.seed = useRandomSeed ? Random.Range(int.MinValue, int.MaxValue) : seed;
    }

    int[,] BuildMapGrid()
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (Random.Range(0, 1f) < fillPercent) ? 1 : 0;
            }
        }
        return map;
    }

    void GenerateGameObjects(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = new Vector3((-width / 2) + x + 0.5f, (-height / 2) + y + 0.5f, -map[x, y] / 2f);
                go.transform.localScale = new Vector3(1, 1, map[x, y] + 1);
                if (mapRoot != null)
                {
                    go.transform.SetParent(mapRoot);
                }
            }
        }
    }
}
