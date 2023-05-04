using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public Material[] Materials;

    public LODSetting[] LODSettings;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        int chunksSizeX = Metrics.ChunkSizeX;   
        int chunksSizeZ = Metrics.ChunkSizeZ;     
        Vector3 chunkWorldSize = new Vector3(
        Metrics.MapResolution.x / chunksSizeX, 
        Metrics.MapResolution.y, 
        Metrics.MapResolution.z / chunksSizeZ);
        
        for (int z = 0; z < chunksSizeZ; z++)
        {
            for (int x = 0; x < chunksSizeX; x++)
            {
                Vector2 uvStart = new Vector2((float)x / chunksSizeX, (float)z / chunksSizeZ);
                Vector2 uvEnd = new Vector2((float)(x + 1) / chunksSizeX, (float)(z + 1) / chunksSizeZ);

                GameObject chunk = new GameObject(string.Format("Chunk({0},{1})", x, z));
                chunk.transform.position = new Vector3(x * chunkWorldSize.x, 0, z * chunkWorldSize.z);
                chunk.transform.SetParent(transform, false);

                LODGroup chunkLODGroup = chunk.AddComponent<LODGroup>();
                chunkLODGroup.fadeMode = LODFadeMode.SpeedTree;
                chunkLODGroup.animateCrossFading = false;

                List<LOD> lods = new List<LOD>();
                for (int i = 0; i < LODSettings.Length; i++)
                {
                    GameObject LODlevel = CreateLODLevel(chunkWorldSize, uvStart, uvEnd, i);
                    LODlevel.transform.SetParent(chunk.transform, false);
                    lods.Add(new LOD(LODSettings[i].RelativeHeight, new Renderer[] { LODlevel.GetComponent<Renderer>() })
                    {fadeTransitionWidth = 0.5f});                    
                }

                chunkLODGroup.SetLODs(lods.ToArray());
                chunkLODGroup.RecalculateBounds();                             
            }
        }
    }

    private GameObject CreateLODLevel(Vector3 chunkSize, Vector2 uvStart, Vector2 uvEnd, int level)
    {
        GameObject lodObject = new GameObject($"LOD{level}");
        MeshFilter lodMeshFilter = lodObject.AddComponent<MeshFilter>();
        MeshRenderer lodMeshRenderer = lodObject.AddComponent<MeshRenderer>();

        lodMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        lodMeshRenderer.allowOcclusionWhenDynamic = true;
        lodMeshRenderer.receiveShadows = true;

        Mesh mesh = MeshGenerator.CreatePlane( 
            Vector3.right * chunkSize.x, 
            Vector3.forward * chunkSize.z,
            LODSettings[level].Size, //TODO same as metrics
            LODSettings[level].Size,
            uvStart, uvEnd);
            
        mesh.bounds = new Bounds(0.5f * chunkSize, chunkSize);
        mesh.name = lodObject.name;

        lodMeshFilter.mesh = mesh;
        lodMeshRenderer.materials = Materials;
        return lodObject;
    }

    [Serializable]
    public class LODSetting
    {
        [Range(4, 256)]
        public int Size = 200;

        [Range(0, 1)]        
        public float RelativeHeight;
    }
}

// public void Generate()
//     {
//         var chunksSizeX = Metrics.ChunkSizeX;   
//         var chunksSizeZ = Metrics.ChunkSizeZ;     
//         var chunkCount = new Vector3(
//         Metrics.WorldSize.x / chunksSizeX, 
//         Metrics.WorldSize.y, 
//         Metrics.WorldSize.z / chunksSizeZ);
        
//         for (var z = 0; z < chunksSizeZ; z++)
//         {
//             for (var x = 0; x < chunksSizeX; x++)
//             {
//                 var uvStart = new Vector2((float)x / chunksSizeX, (float)z / chunksSizeZ);
//                 var uvEnd = new Vector2((float)(x + 1) / chunksSizeX, (float)(z + 1) / chunksSizeZ);

//                 var chunk = new GameObject(string.Format("Chunk({0},{1})", x, z));
//                 chunk.transform.position = new Vector3(x * chunkCount.x, 0, z * chunkCount.z);
//                 chunk.transform.SetParent(transform, false);

//                 var chunkMeshFilter = chunk.AddComponent<MeshFilter>();
//                 var chunkMeshRenderer = chunk.AddComponent<MeshRenderer>();

//                 var chunkMesh = MeshGenerator.CreatePlane(
//                 Vector3.right * chunkCount.x, 
//                 Vector3.forward * chunkCount.z,
//                 32, 32,
//                 uvStart, uvEnd);
//                 chunkMesh.bounds = new Bounds(0.5f * chunkCount, chunkCount);

//                 chunkMeshFilter.mesh = chunkMesh;
//                 chunkMeshRenderer.materials = Materials;                              
//             }
//         }
//     }
