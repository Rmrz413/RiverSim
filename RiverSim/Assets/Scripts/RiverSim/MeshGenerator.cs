using UnityEngine;

public static class MeshGenerator
{
    public static Mesh CreatePlane(Vector3 sizeX, Vector3 sizeZ, int vertsX, int vertsZ, Vector2 uvStart, Vector2 uvEnd)
    {
        var vertices = new Vector3[vertsX * vertsZ];
            var normals = new Vector3[vertices.Length];
            var uvs = new Vector2[vertices.Length];
            var triangles = new int[(vertsX - 1) * (vertsZ - 1) * 2 * 3];
            var normal = Vector3.Cross(sizeZ, sizeX).normalized;

            for (var i = 0; i < vertices.Length; i++)
            {
                //local coordinates on the plane
                var x = i / vertsZ;
                var y = i % vertsZ;
                var localX = x / (vertsX - 1f);
                var localY = y / (vertsZ - 1f);

                vertices[i] = localX * sizeX + localY * sizeZ;
                normals[i] = normal;
                uvs[i].x = Mathf.Lerp(uvStart.x, uvEnd.x, localX);
                uvs[i].y = Mathf.Lerp(uvStart.y, uvEnd.y, localY);
            }

            var vertexIndex = 0;
            for (var x = 0; x < vertsX - 1; x++)
            {
                for (var z = 0; z < vertsZ - 1; z++)
                {
                    triangles[vertexIndex++] = (x + 0) * vertsZ + (z + 0);
                    triangles[vertexIndex++] = (x + 1) * vertsZ + (z + 1);
                    triangles[vertexIndex++] = (x + 1) * vertsZ + (z + 0);

                    triangles[vertexIndex++] = (x + 0) * vertsZ + (z + 0);
                    triangles[vertexIndex++] = (x + 0) * vertsZ + (z + 1);
                    triangles[vertexIndex++] = (x + 1) * vertsZ + (z + 1);
                }
            }

            var mesh = new Mesh()
            {                
                vertices = vertices,
                normals = normals,
                uv = uvs,
                triangles = triangles
            };

            mesh.RecalculateBounds();
            return mesh;
    }
        
}
