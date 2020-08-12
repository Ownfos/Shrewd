using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FilledPolygon : MonoBehaviour
{
    public int edgeCount;

    private Mesh mesh;
    private Material material;

    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.Clear();

        var vertices = new Vector3[edgeCount];

        // Triangle requires 3 index, rectangle requires 4, ...
        var triangles = new int[(edgeCount - 2) * 3];

        for (var currentIndex = 0; currentIndex < edgeCount; currentIndex++)
        {
            var angle = 360.0f / edgeCount * (currentIndex + 0.5f);
            vertices[currentIndex] = Quaternion.Euler(0, 0, angle) * Vector3.right;
        }

        for (var currentIndex = 0; currentIndex < edgeCount - 2; currentIndex++)
        {
            triangles[currentIndex * 3] = 0;
            triangles[currentIndex * 3 + 1] = currentIndex + 1;
            triangles[currentIndex * 3 + 2] = currentIndex + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    public void UpdateSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    public void UpdateAlpha(float alpha)
    {
        var color = material.color;
        color.a = alpha;
        material.color = color;
    }
}
