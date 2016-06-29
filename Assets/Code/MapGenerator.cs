using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    uint width;
    [SerializeField]
    uint height;
	[SerializeField]
	float depth;

    [SerializeField]
    Transform mapRoot;

    [Range(0f, 1f)]
    [SerializeField]
    float fillPercent;

    [Range(0, 4)]
    [SerializeField]
    int smoothSteps;

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
        BuildMapGrid();
        for (int i = 0; i < smoothSteps; i++)
        {
            SmoothMap();
        }
        GenerateGameObjects(map);
    }

    void ConfigureRandomSeed()
    {
        Random.seed = useRandomSeed ? Random.Range(int.MinValue, int.MaxValue) : seed;
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int wallCount = GetSurroundingWallCount(x, y);
                if (wallCount > 4)
                {
                    map[x, y] = 1;
                }
                else if (wallCount < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    int GetSurroundingWallCount(int itemX, int itemY)
    {
        int wallCount = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int currentX = itemX + x;
                int currentY = itemY + y;

                if (currentX == itemX && currentY == itemY)
                {
                    continue;
                }
                if (currentX < 0 || currentX >= width || currentY < 0 || currentY >= height)
                {
                    wallCount += 1;
                }
                else
                {
                    wallCount += map[currentX, currentY];
                }
            }
        }
        return wallCount;
    }

    void BuildMapGrid()
    {
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = (Random.Range(0, 1f) < fillPercent) ? 1 : 0;
            }
        }
    }

    void GenerateGameObjects(int[,] map)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
				float zScale = (map[x, y] * depth) + 1;
                go.transform.position = new Vector3((-width / 2) + x + 0.5f, (-height / 2) + y + 0.5f, -zScale / 2);
                go.transform.localScale = new Vector3(1, 1, zScale);
                if (mapRoot != null)
                {
                    go.transform.SetParent(mapRoot);
                }
            }
        }
    }
}
