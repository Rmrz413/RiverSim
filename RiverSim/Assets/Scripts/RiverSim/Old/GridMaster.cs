using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main grid for the simulation
public class GridMaster : MonoBehaviour
{
    public Chunk_Old terrainChunkPrefab;    

    public Cell cellPrefab;    

    int chunkCountX, chunkCountZ;

    int cellCountX, cellCountZ;

    [SerializeField]
    int totalCells;

    Chunk_Old[] Chunks;

    Cell[] Cells;
    
    public void Initialize(int chunkCountX, int chunkCountZ)
    {        
        
        this.chunkCountX = chunkCountX;
        this.chunkCountZ = chunkCountZ;
        cellCountX = chunkCountX * Metrics_Old.ChunkSizeX;
		cellCountZ = chunkCountZ * Metrics_Old.ChunkSizeZ;
        totalCells = cellCountX * cellCountZ;        

        CreateChunks();
		CreateCells();
    }

    public Cell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        int index = Mathf.RoundToInt(position.x) + Mathf.RoundToInt(position.z) * cellCountZ;
        return Cells[index];
    }

    public Vector3 GetCellLocal(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        position.x = Mathf.RoundToInt(position.x);
        position.z = Mathf.RoundToInt(position.z);
        return position;
    }

    public void UpdateCells(Texture2D heightMap)
    {
        for (int i = 0; i < heightMap.height; i++)
        {
            for (int j = 0; j < heightMap.width; j++)
            {
                int index = i + j * cellCountZ;
                Cells[index].TerrainHeight = heightMap.GetPixel(i,j).r * Metrics_Old.Strenght;
                Cells[index].WaterHeight = heightMap.GetPixel(i,j).g * Metrics_Old.Strenght;
            }
        }
        Refresh();
    }

    void CreateChunks()
    {
		Chunks = new Chunk_Old[chunkCountX * chunkCountZ];

		for (int z = 0, i = 0; z < chunkCountZ; z++) 
        {
			for (int x = 0; x < chunkCountX; x++) 
            {
				Chunk_Old chunk = Chunks[i++] = Instantiate(terrainChunkPrefab);                
				chunk.transform.SetParent(transform);
			}
		}
	}

    void CreateCells()
	{
		Cells = new Cell[cellCountZ * cellCountX];

		for (int z = 0, i = 0; z < cellCountZ; z++)
		{
			for (int x = 0; x < cellCountX; x++)
			{                
				CreateCell(x, z, i++);
			}
		}
	}

    void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = x * (Metrics_Old.CellSize);
		position.y = 0f;
		position.z = z * (Metrics_Old.CellSize);

		Cell cell = Cells[i] = Instantiate<Cell>(cellPrefab);        
		cell.transform.localPosition = position;		

        if (x > 0)
	    {
			cell.SetNeighbor(Direction.W, Cells[i - 1]);
		}

        if (z > 0)
        {
            cell.SetNeighbor(Direction.S, Cells[i - cellCountX]);
        }

        cell.TerrainHeight = 0;
        cell.WaterHeight = 0;

        AddCellToChunk(x, z, cell);
	}

    void AddCellToChunk(int x, int z, Cell cell)
	{
		int chunkX = x / Metrics_Old.ChunkSizeX;
		int chunkZ = z / Metrics_Old.ChunkSizeZ;
		Chunk_Old chunk = Chunks[chunkX + chunkZ * chunkCountX];

		int localX = x - chunkX * Metrics_Old.ChunkSizeX;
		int localZ = z - chunkZ * Metrics_Old.ChunkSizeZ;
		chunk.AddCell(localX + localZ * Metrics_Old.ChunkSizeZ, cell);
	}    

    // void ApplyPerlin()
    // {
    //     Vector3 pos;
    //     for (int i = 0; i < Cells.Length; i++)
    //     {
    //         pos = Cells[i].transform.position;
    //         Cells[i].TerrainHeight = Mathf.Max(
    //         Mathf.PerlinNoise(
    //             Mathf.Sin((pos.x / (Metrics.chunkSizeX * chunkCountX) * perlinSmoothX)), 
    //             (pos.z / (Metrics.chunkSizeX * chunkCountZ) * perlinSmoothZ) + perlinOffset) * perlinStrenght,
    //         Mathf.PerlinNoise(
    //             (pos.x / (Metrics.chunkSizeX * chunkCountX) * perlinSmoothX) + perlinOffset, 
    //             Mathf.Cos((pos.z / (Metrics.chunkSizeX * chunkCountZ) * perlinSmoothZ)) * perlinStrenght));
            
    //         //cells[i].Elevation = Mathf.PerlinNoise(Mathf.Sin(pos.x * perlinSmoothX + perlinOffsetX), Mathf.Cos(pos.z * perlinSmoothZ + perlinOffsetZ)) * perlinStrenght;
    //         //Mathf.PerlinNoise(globPos.x * 1.3f, globPos.z * 1.3f);
    //         //Mathf.Sin(globPos.x * 0.0008f + 100), Mathf.Cos(globPos.z * 0.01f + 400)) * 150; <-- 5x5 chunk
    //     }        
    // }

    void Refresh()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Refresh();                 
        }
    }

    // private void OnGUI()
    // {
    //     if (GUI.Button(new Rect(0,0,100,50), "ApplyPerlin"))
    //         {
    //             ApplyPerlin();
    //         }
    // }
}
