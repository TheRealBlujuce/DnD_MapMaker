using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CompositeCollider2D), typeof(ShadowCaster2D))]
public class ShadowCasterGenerator : MonoBehaviour
{
    void Start()
    {
        CompositeCollider2D collider = GetComponent<CompositeCollider2D>();
        if (collider == null) return;

        GameObject shadowCasterGO = new GameObject("Generated ShadowCaster");
        shadowCasterGO.transform.SetParent(transform, false);
        shadowCasterGO.layer = gameObject.layer;

        var meshFilter = shadowCasterGO.AddComponent<MeshFilter>();
        var meshRenderer = shadowCasterGO.AddComponent<MeshRenderer>();
        var shadowCaster = shadowCasterGO.AddComponent<ShadowCaster2D>();

        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        meshRenderer.receiveShadows = false;
        meshRenderer.enabled = false; // You don't want to see it

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new();
        List<int> triangles = new();

        int pathCount = collider.pathCount;
        for (int p = 0; p < pathCount; p++)
        {
            Vector2[] path = new Vector2[collider.GetPathPointCount(p)];
            collider.GetPath(p, path);
            int startVertIndex = vertices.Count;

            foreach (Vector2 point in path)
                vertices.Add(point);

            // Triangulate the path (basic fan triangulation)
            for (int i = 1; i < path.Length - 1; i++)
            {
                triangles.Add(startVertIndex);
                triangles.Add(startVertIndex + i);
                triangles.Add(startVertIndex + i + 1);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
