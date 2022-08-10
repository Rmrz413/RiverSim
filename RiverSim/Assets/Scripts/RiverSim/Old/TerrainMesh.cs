using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Terrain surface renderer
public class TerrainMesh : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    static List<Vector3> vertices = new List<Vector3>();
    static List<int> triangles = new List<int>();
    static List<Vector2> uvs = new List<Vector2>();    

    void Awake()
    {
        GetComponent<MeshFilter>().sharedMesh = mesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        mesh.name = "Terrain Mesh";
    }    

    // Triangulating a mesh for the cells
    public void Triangulate(Cell[] cells)
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
          Triangulate(cells[i]);
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray(); 
        //FixUVs();
        mesh.uv = uvs.ToArray();     
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    // Triangulate a single cell and the connectons with 'Up' and 'Right' neighbours on demand
    void Triangulate(Cell cell)
    {
        if (!cell.HasWater)
        {
          CalculateTop(cell);        
        }

        Cell neighbor = cell.GetNeighbor(Direction.N);
        if (neighbor && 
        !(neighbor.HasWater && neighbor.Elevation > cell.Elevation) && 
        !(cell.HasWater && neighbor.Elevation < cell.Elevation))
        {
          CalculateConnectionN(cell);
        }

        neighbor = cell.GetNeighbor(Direction.E);
        if (neighbor && 
        !(neighbor.HasWater && neighbor.Elevation > cell.Elevation) && 
        !(cell.HasWater && neighbor.Elevation < cell.Elevation))
        {
          CalculateConnectionE(cell);
        }
    }
    
    // Calculating the vertices for the top of a single cell
    void CalculateTop(Cell cell)
    {
        Vector3 v1 = new Vector3(cell.Position.x - (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v2 = new Vector3(cell.Position.x + (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v3 = new Vector3(cell.Position.x - (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z - (Metrics_Old.CellSize / 2.0f));
        Vector3 v4 = new Vector3(cell.Position.x + (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z - (Metrics_Old.CellSize / 2.0f));
        AddQuad(v1,v2,v3,v4);
    }

    // Adding a Quad (2 Triangles)
    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) 
    {
        float d = Mathf.Sqrt(Metrics_Old.ChunkSizeX * Metrics_Old.ChunkSizeZ + 1) / 2;
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex + 2);
    }  


    void FixUVs()
    {
        float size = vertices.Count / Metrics_Old.ChunkSizeX;
        for (int i = 0; i < uvs.Count; i++)
        {
          Vector2 vec = uvs[i];
          uvs[i] = new Vector2(vec.x / size, vec.y / size );
        }
    }

    // Calculating the vertices for the connection with northen neighbour
    void CalculateConnectionN(Cell cell)
    {
        Cell neighbour = cell.GetNeighbor(Direction.N);        

        Vector3 v1 = new Vector3(cell.Position.x - (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v2 = new Vector3(neighbour.Position.x - (Metrics_Old.CellSize / 2.0f), neighbour.Position.y, neighbour.Position.z - (Metrics_Old.CellSize / 2.0f));
        Vector3 v3 = new Vector3(cell.Position.x + (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v4 = new Vector3(neighbour.Position.x + (Metrics_Old.CellSize / 2.0f), neighbour.Position.y, neighbour.Position.z - (Metrics_Old.CellSize / 2.0f));

        AddQuad(v1,v2,v3,v4);
    }

    // Calculating the vertices for the connection with eastern neighbour
    void CalculateConnectionE(Cell cell)
    {
        Cell neighbour = cell.GetNeighbor(Direction.E);        

        Vector3 v1 = new Vector3(cell.Position.x + (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v2 = new Vector3(neighbour.Position.x - (Metrics_Old.CellSize / 2.0f), neighbour.Position.y, neighbour.Position.z + (Metrics_Old.CellSize / 2.0f));
        Vector3 v3 = new Vector3(cell.Position.x + (Metrics_Old.CellSize / 2.0f), cell.Position.y, cell.Position.z - (Metrics_Old.CellSize / 2.0f));
        Vector3 v4 = new Vector3(neighbour.Position.x - (Metrics_Old.CellSize / 2.0f), neighbour.Position.y, neighbour.Position.z - (Metrics_Old.CellSize / 2.0f));

        AddQuad(v1,v2,v3,v4);
    }
}
