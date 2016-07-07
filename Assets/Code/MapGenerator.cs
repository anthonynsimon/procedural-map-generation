using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    uint width;
    [SerializeField]
    uint height;

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

    int[,] voxelsMap;

    void OnDrawGizmos()
    {
        if (voxelsMap != null)
        {
            for (int x = 0; x < voxelsMap.GetLength(0); x++)
            {
                for (int y = 0; y < voxelsMap.GetLength(1); y++)
                {
                    Vector3 position = new Vector3(x, y, 0);
                    Vector3 size = Vector3.one;

                    Gizmos.color = voxelsMap[x, y] > 0 ? Color.white : Color.black;
                    Gizmos.DrawCube(position, size / 2);
                }
            }

            Vector2[][] triangles = MeshGenerator.BuildMarchingSquares(voxelsMap);
            for (int i = 0; i < triangles.Length; i++)
            {
                Vector2[] triGroup = triangles[i];
                for (int j = 0; j + 2 < triGroup.Length; j += 3)
                {
                    Gizmos.DrawLine(triGroup[j], triGroup[j + 1]);
                    Gizmos.DrawLine(triGroup[j + 1], triGroup[j + 2]);
                    Gizmos.DrawLine(triGroup[j + 2], triGroup[j]);
                }
            }
        }
    }

    public void GenerateMap()
    {
        ConfigureSeed();
        BuildMapGrid();
        for (int i = 0; i < smoothSteps; i++)
        {
            SmoothMap();
        }
    }

    public void Meshify()
    {
        if (voxelsMap != null)
        {
            MeshGenerator.Meshify(voxelsMap);
        }
    }

    void ConfigureSeed()
    {
        if (useRandomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.seed = seed;
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
                    voxelsMap[x, y] = 1;
                }
                else if (wallCount < 4)
                {
                    voxelsMap[x, y] = 0;
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
                    wallCount += voxelsMap[currentX, currentY];
                }
            }
        }
        return wallCount;
    }

    void BuildMapGrid()
    {
        voxelsMap = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                voxelsMap[x, y] = (Random.Range(0, 1f) < fillPercent) ? 1 : 0;
            }
        }
    }
}
