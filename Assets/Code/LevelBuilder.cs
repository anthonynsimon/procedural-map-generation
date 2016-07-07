using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour
{

    [SerializeField]
    int width;
    [SerializeField]
    int height;

    [Range(0f, 1f)]
    [SerializeField]
    float fill;

    [Range(0, 4)]
    [SerializeField]
    int smoothSteps;

    [SerializeField]
    int seed;

    [SerializeField]
    bool useRandomSeed = false;

    int[,] voxelsMap;

    public void GenerateMap()
    {
        ConfigureSeed();
        voxelsMap = MapGenerator.GenerateMap(seed, width, height, fill);
        MapGenerator.SmoothMap(voxelsMap, smoothSteps);
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

    void OnDrawGizmos()
    {
        if (ValueChanged())
        {
            GenerateMap();
        }
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
            Gizmos.color = Color.yellow;
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

    float lastfill;
    int lastSmoothSteps;
    int lastWidth;
    int lastHeight;
    bool ValueChanged()
    {
        bool changed = false;
        
        if (lastWidth != width)
        {
            lastWidth = width;
            changed = true;
        }
        if (lastHeight != height)
        {
            lastHeight = height;
            changed = true;
        }
        if (lastSmoothSteps != smoothSteps)
        {
            lastSmoothSteps = smoothSteps;
            changed = true;
        }
        if (lastfill != fill)
        {
            lastfill = fill;
            changed = true;
        }

        return changed;
    }
}
