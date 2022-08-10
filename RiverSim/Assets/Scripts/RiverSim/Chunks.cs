using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunks : MonoBehaviour
{
    public Material[] Materials;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        var chunksSizeX = Metrics.ChunkSizeX;   
        var chunksSizeZ = Metrics.ChunkSizeZ;     
        var chunkCount = new Vector3(
        Metrics.WorldSize.x / chunksSizeX, 
        Metrics.WorldSize.y, 
        Metrics.WorldSize.z / chunksSizeZ);
        
        for (var z = 0; z < chunksSizeZ; z++)
        {
            for (var x = 0; x < chunksSizeX; x++)
            {
                var uvStart = new Vector2((float)x / chunksSizeX, (float)z / chunksSizeZ);
                var uvEnd = new Vector2((float)(x + 1) / chunksSizeX, (float)(z + 1) / chunksSizeZ);

                var chunk = new GameObject(string.Format("Chunk({0},{1})", x, z));
                chunk.transform.position = new Vector3(x * chunkCount.x, 0, z * chunkCount.z);
                chunk.transform.SetParent(transform, false);

                var chunkMeshFilter = chunk.AddComponent<MeshFilter>();
                var chunkMeshRenderer = chunk.AddComponent<MeshRenderer>();

                var chunkMesh = MeshGenerator.CreatePlane(
                Vector3.right * chunkCount.x, 
                Vector3.forward * chunkCount.z,
                32, 32,
                uvStart, uvEnd);
                chunkMesh.bounds = new Bounds(0.5f * chunkCount, chunkCount);  

                chunkMeshFilter.mesh = chunkMesh;
                chunkMeshRenderer.materials = Materials;                              
            }
        }
    }    
}
