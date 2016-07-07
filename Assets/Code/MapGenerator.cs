using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    public static int[,] GenerateMap(int seed, int width, int height, float fill)
    {
        return BuildVoxelsGrid(seed, width, height, fill);
    }

    public static void SmoothMap(int[,] map, int smoothSteps)
    {
        for (int i = 0; i < smoothSteps; i++)
        {
            RelaxVoxelsGrid(map);
        }
    }

    static void RelaxVoxelsGrid(int[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int wallCount = GetSurroundingWallCount(x, y, width, height, map);
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

    static int GetSurroundingWallCount(int itemX, int itemY, int maxWidth, int maxHeight, int[,] map)
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
                if (currentX < 0 || currentX >= maxWidth || currentY < 0 || currentY >= maxHeight)
                {
                    wallCount += 4;
                }
                else
                {
                    wallCount += map[currentX, currentY];
                }
            }
        }
        return wallCount;
    }

    static int[,] BuildVoxelsGrid(int seed, int width, int height, float fill)
    {
        int[,] voxelsMap = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                voxelsMap[x, y] = (Random.Range(0, 1f) < fill) ? 1 : 0;
            }
        }
        return voxelsMap;
    }
}
