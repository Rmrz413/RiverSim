using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWMESH : MonoBehaviour
{
    Mesh mesh;
    public float[,] heights;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    public int size = 20;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        heights = new float[(size + 1), (size + 1)];
        for (int i = 0; i < heights.GetLength(0); i++)
        {
            for (int j = 0; j < heights.GetLength(1); j++)
            {
                heights[i,j] = Random.Range(1.0f, 1.5f);
            }            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

    // Update is called once per frame
    void CreateMesh()
    {
        vertices = new Vector3[(size + 1) * (size + 1)];
        uvs = new Vector2[vertices.Length]; //+4*n

        for (int i = 0, z = 0; z < size + 1; z++)
        {
            for (int x = 0; x < size + 1; x++)
            {
                vertices[i] = new Vector3(x, (heights[z, x]), z);

                uvs[i].x = (float)x / size;
                uvs[i].y = (float)z / size;

                i++;
            }
        }

        triangles = new int[size * size * 6];
        for (int vert = 0, tris = 0, z = 0; z < size; z++, vert++)
        {
            for (int x = 0; x < size; x++)
            {            
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;

                vert++;
                tris += 6;                
            }
        }
        
        // TODO: add sides
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
