using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{    
    public Chunk_Old chunk;    
    float terrainHeight = int.MinValue;
    float waterHeight = int.MinValue;
    bool hasWater = false;

    [SerializeField]
    Cell[] neighbors;

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public float TerrainHeight
    {
        get
        {
            return terrainHeight;
        }
        set
        {
            if (terrainHeight == value)
            {
                return;
            }
            
            terrainHeight = value;            
            Vector3 position = transform.localPosition;
            position.y = value;
            transform.localPosition = position;
        }
    }

    public float WaterHeight
    {
        get
        {
            return waterHeight;
        }
        set
        {
            if (waterHeight == value)
            {
                return;
            }

            HasWater = value > 0.0001f ? true : false;            
            waterHeight = value;
        }
    }

    public float Elevation
    {
        get
        {
            return terrainHeight + waterHeight;
        }        
    }    

    public bool HasWater
    {
        get
        { 
            return hasWater;
        }
        private set
        {
            if (hasWater == value)
            {
                return;
            }

            hasWater = value;
        }
    }

    public Cell GetNeighbor(Direction direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(Direction direction, Cell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++)
            {
                Cell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk)
                {
                    neighbor.chunk.Refresh();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
