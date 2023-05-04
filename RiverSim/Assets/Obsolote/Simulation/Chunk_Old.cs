using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk_Old : MonoBehaviour
{
    Cell[] cells;

    TerrainMesh surfaceMesh;

    WaterMesh waterMesh;
    

    void Awake()
    {
        surfaceMesh = GetComponentInChildren<TerrainMesh>();

        waterMesh = GetComponentInChildren<WaterMesh>();

        cells = new Cell[Metrics_Old.ChunkSizeX * Metrics_Old.ChunkSizeZ];        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        surfaceMesh.Triangulate(cells);
        waterMesh.Triangulate(cells);
        enabled = false;
    }

    public void AddCell(int index, Cell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
    }

    public void Refresh()
    {
        enabled = true;        
    }
}
