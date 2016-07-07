using UnityEngine;
using System.Collections.Generic;

public class MeshGenerator
{

    public static Vector2[][] Meshify(int[,] map)
    {
        VoxelSquare[,] voxels = Parse(map);
        return Triangulate(voxels);

        // Vector3[] vertices = GetVertices(voxels);
        // int[] triangles = GetTriangles(voxels);
        // GenerateMeshes(vertices, triangles);
    }

    static VoxelSquare[,] Parse(int[,] map)
    {
        int sizeX = map.GetLength(0) - 1;
        int sizeY = map.GetLength(1) - 1;

        VoxelSquare[,] results = new VoxelSquare[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                bool topLeft = map[x, y + 1] > 0 ? true : false;
                bool topRight = map[x + 1, y + 1] > 0 ? true : false;
                bool bottomRight = map[x + 1, y] > 0 ? true : false;
                bool bottomLeft = map[x, y] > 0 ? true : false;
                results[x, y] = new VoxelSquare(topLeft, topRight, bottomRight, bottomLeft);
            }
        }

        return results;
    }

    // static Vector3[] GetVertices(VoxelSquare[,] voxels)
    // {
    //     List<Vector3> vertices = new List<Vector3>();

    //     foreach (Vector2[] triGroup in Triangulate(voxels))
    //     {
    //         if (triGroup != null)
    //         {
    //             vertices.Add(triGroup[0]);
    //             vertices.Add(triGroup[1]);
    //             vertices.Add(triGroup[2]);
    //             if (triGroup.Length > 3)
    //             {
    //                 vertices.Add(triGroup[3]);
    //                 vertices.Add(triGroup[4]);
    //                 vertices.Add(triGroup[5]);
    //             }
    //         }
    //     }

    //     return vertices.ToArray();
    // }
    // static int[] GetTriangles(VoxelSquare[,] voxels)
    // {
    //     List<int> triangles = new List<int>();

    //     int i = 0;
    //     foreach (Vector2[] triGroup in Triangulate(voxels))
    //     {
    //         if (triGroup != null)
    //         {
    //             triangles.Add(i++);
    //             triangles.Add(i++);
    //             triangles.Add(i++);
    //             if (triGroup.Length > 3)
    //             {
    //                 triangles.Add(i++);
    //                 triangles.Add(i++);
    //                 triangles.Add(i++);
    //             }
    //         }
    //     }

    //     return triangles.ToArray();
    // }

    static Vector2[][] Triangulate(VoxelSquare[,] voxels)
    {
        List<Vector2[]> triangles = new List<Vector2[]>();
        for (int x = 0; x < voxels.GetLength(0); x++)
        {
            for (int y = 0; y < voxels.GetLength(1); y++)
            {
                VoxelSquare current = voxels[x, y];
                Vector2[] triSet;
                switch (current.Type())
                {
                    /*
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    */
                    case 0:
                        break;
                    /*
                    *   [X][X][ ][ ]
                    *   [X][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    */
                    case 1:
                        triSet = new Vector2[3];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 0.5f, y + 1);
                        triSet[2] = new Vector2(x, y + 0.5f);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][X][X]
                    *   [ ][ ][ ][X]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    */
                    case 2:
                        triSet = new Vector2[3];

                        triSet[0] = new Vector2(x + 0.5f, y + 1);
                        triSet[1] = new Vector2(x + 1, y + 1);
                        triSet[2] = new Vector2(x + 1, y + 0.5f);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    */
                    case 3:
                        triSet = new Vector2[6];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 1, y + 1);
                        triSet[2] = new Vector2(x, y + 0.5f);

                        triSet[3] = new Vector2(x, y + 0.5f);
                        triSet[4] = new Vector2(x + 1, y + 1);
                        triSet[5] = new Vector2(x + 1, y + 0.5f);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][X]
                    *   [ ][ ][X][X]
                    */
                    case 4:
                        triSet = new Vector2[3];

                        triSet[0] = new Vector2(x + 0.5f, y);
                        triSet[1] = new Vector2(x + 1, y + 0.5f);
                        triSet[2] = new Vector2(x + 1, y);

                        triangles.Add(triSet);

                        break;
                    /*
                    *   [X][X][ ][ ]
                    *   [X][X][X][ ]
                    *   [ ][X][X][X]
                    *   [ ][ ][X][X]
                    */
                    case 5:
                        triSet = new Vector2[12];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 0.5f, y + 1);
                        triSet[2] = new Vector2(x, y + 0.5f);

                        triSet[3] = new Vector2(x + 0.5f, y + 0.5f);
                        triSet[4] = new Vector2(x + 1, y + 0.5f);
                        triSet[5] = new Vector2(x + 1, y);

                        triSet[6] = new Vector2(x, y + 0.5f);
                        triSet[7] = new Vector2(x + 0.5f, y + 1);
                        triSet[8] = new Vector2(x + 0.5f, y);

                        triSet[9] = new Vector2(x + 0.5f, y + 1);
                        triSet[10] = new Vector2(x + 1, y + 0.5f);
                        triSet[11] = new Vector2(x + 0.5f, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][X][X]
                    *   [ ][ ][X][X]
                    *   [ ][ ][X][X]
                    *   [ ][ ][X][X]
                    */
                    case 6:
                        triSet = new Vector2[6];

                        triSet[0] = new Vector2(x + 0.5f, y + 1);
                        triSet[1] = new Vector2(x + 1, y);
                        triSet[2] = new Vector2(x + 0.5f, y);

                        triSet[3] = new Vector2(x + 0.5f, y + 1);
                        triSet[4] = new Vector2(x + 1, y + 1);
                        triSet[5] = new Vector2(x + 1, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    *   [ ][X][X][X]
                    *   [ ][ ][X][X]
                    */
                    case 7:
                        triSet = new Vector2[9];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 1, y + 1);
                        triSet[2] = new Vector2(x, y + 0.5f);

                        triSet[3] = new Vector2(x, y + 0.5f);
                        triSet[4] = new Vector2(x + 1, y + 1);
                        triSet[5] = new Vector2(x + 0.5f, y);

                        triSet[6] = new Vector2(x + 0.5f, y);
                        triSet[7] = new Vector2(x + 1, y + 1);
                        triSet[8] = new Vector2(x + 1, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [X][ ][ ][ ]
                    *   [X][X][ ][ ]
                    */
                    case 8:
                        triSet = new Vector2[3];

                        triSet[0] = new Vector2(x, y);
                        triSet[1] = new Vector2(x, y + 0.5f);
                        triSet[2] = new Vector2(x + 0.5f, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][ ][ ]
                    *   [X][X][ ][ ]
                    *   [X][X][ ][ ]
                    *   [X][X][ ][ ]
                    */
                    case 9:
                        triSet = new Vector2[6];

                        triSet[1] = new Vector2(x, y + 1);
                        triSet[2] = new Vector2(x + 0.5f, y + 1);
                        triSet[0] = new Vector2(x + 0.5f, y);

                        triSet[3] = new Vector2(x, y + 1);
                        triSet[4] = new Vector2(x + 0.5f, y);
                        triSet[5] = new Vector2(x, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][X][X]
                    *   [ ][X][X][X]
                    *   [X][X][X][ ]
                    *   [X][X][ ][ ]
                    */
                    case 10:
                        triSet = new Vector2[12];

                        triSet[0] = new Vector2(x, y);
                        triSet[1] = new Vector2(x, y + 0.5f);
                        triSet[2] = new Vector2(x + 0.5f, y);

                        triSet[3] = new Vector2(x + 0.5f, y + 1);
                        triSet[4] = new Vector2(x + 1, y + 1);
                        triSet[5] = new Vector2(x + 1, y + 0.5f);

                        triSet[6] = new Vector2(x, y + 0.5f);
                        triSet[7] = new Vector2(x + 0.5f, y + 1);
                        triSet[8] = new Vector2(x + 0.5f, y);

                        triSet[9] = new Vector2(x + 0.5f, y + 1);
                        triSet[10] = new Vector2(x + 1, y + 0.5f);
                        triSet[11] = new Vector2(x + 0.5f, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    *   [X][X][X][ ]
                    *   [X][X][ ][ ]
                    */
                    case 11:
                        triSet = new Vector2[9];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 1, y + 1);
                        triSet[2] = new Vector2(x, y);

                        triSet[3] = new Vector2(x + 1, y + 1);
                        triSet[4] = new Vector2(x + 0.5f, y);
                        triSet[5] = new Vector2(x, y);

                        triSet[6] = new Vector2(x + 0.5f, y);
                        triSet[7] = new Vector2(x + 1, y + 1);
                        triSet[8] = new Vector2(x + 1, y + 0.5f);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][ ][ ]
                    *   [ ][ ][ ][ ]
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    */
                    case 12:
                        triSet = new Vector2[6];

                        triSet[0] = new Vector2(x, y + 0.5f);
                        triSet[1] = new Vector2(x + 1, y + 0.5f);
                        triSet[2] = new Vector2(x, y);

                        triSet[3] = new Vector2(x + 1, y + 0.5f);
                        triSet[4] = new Vector2(x + 1, y);
                        triSet[5] = new Vector2(x, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][ ][ ]
                    *   [X][X][X][ ]
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    */
                    case 13:
                        triSet = new Vector2[9];

                        triSet[0] = new Vector2(x, y + 1);
                        triSet[1] = new Vector2(x + 1, y);
                        triSet[2] = new Vector2(x, y);

                        triSet[3] = new Vector2(x, y + 1);
                        triSet[4] = new Vector2(x + 1, y + 0.5f);
                        triSet[5] = new Vector2(x + 1, y);

                        triSet[6] = new Vector2(x, y + 1);
                        triSet[7] = new Vector2(x + 0.5f, y + 1);
                        triSet[8] = new Vector2(x + 1, y + 0.5f);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [ ][ ][X][X]
                    *   [ ][X][X][X]
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    */
                    case 14:
                        triSet = new Vector2[9];

                        triSet[0] = new Vector2(x + 1, y + 1);
                        triSet[1] = new Vector2(x + 1, y);
                        triSet[2] = new Vector2(x, y);

                        triSet[3] = new Vector2(x + 0.5f, y + 1);
                        triSet[4] = new Vector2(x + 1, y + 1);
                        triSet[5] = new Vector2(x, y);

                        triSet[6] = new Vector2(x, y + 0.5f);
                        triSet[7] = new Vector2(x + 0.5f, y + 1);
                        triSet[8] = new Vector2(x, y);

                        triangles.Add(triSet);
                        break;
                    /*
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    *   [X][X][X][X]
                    */
                    case 15:
                        triSet = new Vector2[6];

                        triSet[0] = new Vector2(x, y);
                        triSet[1] = new Vector2(x, y + 1);
                        triSet[2] = new Vector2(x + 1, y);

                        triSet[3] = new Vector2(x + 1, y + 1);
                        triSet[4] = new Vector2(x + 1, y);
                        triSet[5] = new Vector2(x, y + 1);

                        triangles.Add(triSet);
                        break;
                    default:
                        break;
                }
            }
        }

        return triangles.ToArray();
    }

    // static void GenerateMeshes(Vector3[] vertices, int[] triangles)
    // {
    //     GameObject go = new GameObject("Cave");
    //     go.AddComponent<MeshFilter>();
    //     go.AddComponent<MeshRenderer>();
    //     Mesh mesh = go.GetComponent<MeshFilter>().mesh;
    //     mesh.Clear();
    //     mesh.vertices = vertices;
    //     mesh.triangles = triangles;
    // }
}
