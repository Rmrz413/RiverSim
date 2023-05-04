using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWMESHSIDE : MonoBehaviour
{
    Mesh mesh;
    public NEWMESH eh;
    float[,] heights;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;

    public int size = 8;

    // Start is called before the first frame update
    void Start()
    {
        heights = eh.heights;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

    // Update is called once per frame
    void CreateMesh()
    {
        vertices = new Vector3[(size + 1) * 4 * 2];
        uvs = new Vector2[vertices.Length];
        int i = -1;

        for (int z = 0; z < size; z++) // (size) * 2
        {
            vertices[++i] = new Vector3(0, heights[z, 0], z);
            vertices[++i] = new Vector3(0, 0, z);
            uvs[i].x = (float)size;
            uvs[i].y = (float)z / size;
        }
        for (int x = 0; x < size; x++) // (size) * 2
        {
            vertices[++i] = new Vector3(x, heights[size, x], size);
            vertices[++i] = new Vector3(x, 0, size);
            uvs[i].x = (float)x / size;
            uvs[i].y = (float)size;
        }
        for (int z = size; z >= 0; z--) // (size - 1) * 2
        {
            vertices[++i] = new Vector3(size, heights[z, size], z);
            vertices[++i] = new Vector3(size, 0, z);
            uvs[i].x = (float)size;
            uvs[i].y = (float)z / size;
        }
        for (int x = size - 1; x >= 0; x--) // (size - 1) * 2
        {
            vertices[++i] = new Vector3(x, heights[0, x], 0);
            vertices[++i] = new Vector3(x, 0, 0);
            uvs[i].x = (float)x / size;
            uvs[i].y = (float)size;
        }

        Debug.Log("Size of vertices: " + vertices.Length + " Number of vertices: " + i);

        triangles = new int[size * 4 * 2 * 6];
        int tris = 0, vert;
        for (vert = 0; vert <= vertices.Length - 3; vert+=2)
        {
            if (tris > triangles.Length -1 )
            {
                Debug.Log(tris);
            }
            triangles[tris + 0] = vert;
            triangles[tris + 1] = vert + 1;
            triangles[tris + 2] = vert + 3;
            triangles[tris + 3] = vert;
            triangles[tris + 4] = vert + 3;
            triangles[tris + 5] = vert + 2;

            tris += 6;  

            Debug.Log(vert);
        }
        triangles[tris + 0] = vertices.Length - 2;
        triangles[tris + 1] = vertices.Length - 1;
        triangles[tris + 2] = 0;
        triangles[tris + 3] = vertices.Length - 2;
        triangles[tris + 4] = 1;
        triangles[tris + 5] = 0;
        
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
