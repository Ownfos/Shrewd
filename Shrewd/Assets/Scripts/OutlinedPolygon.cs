using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class OutlinedPolygon : MonoBehaviour
{
    public int edgeCount;

    private Mesh mesh;
    private Material material;

    public void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        mesh.Clear();

        // Inner border: 0 ~ edgeCount-1
        // Outer border: edgeCount ~ edgeCount * 2
        //
        // For index i, vertices[i] and vertices[i + edgeCount]
        // are same vectors with different length (the latter is longer)
        var vertices = new Vector3[edgeCount * 2];

        // For each edge, we need to create two triangles
        // (6 vertices involved) for the trapezoid shape
        var triangles = new int[edgeCount * 6];

        for (var currentIndex = 0; currentIndex < edgeCount; currentIndex++)
        {
            var angle = 360.0f / edgeCount * (currentIndex + 0.5f);
            vertices[currentIndex+edgeCount] = vertices[currentIndex] = Quaternion.Euler(0, 0, angle) * Vector3.right;
        }

        for (var currentIndex = 0; currentIndex < edgeCount; currentIndex++)
        {
            var nextIndex = (currentIndex + 1) % edgeCount;
            triangles[currentIndex * 6]     = currentIndex;
            triangles[currentIndex * 6 + 1] = currentIndex + edgeCount;
            triangles[currentIndex * 6 + 2] = nextIndex;

            triangles[currentIndex * 6 + 3] = currentIndex + edgeCount;
            triangles[currentIndex * 6 + 4] = nextIndex + edgeCount;
            triangles[currentIndex * 6 + 5] = nextIndex;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    public void UpdateInnerSize(float innerSize)
    {
        var vertices = mesh.vertices;
        for (var i = 0; i < edgeCount; i++)
        {
            vertices[i] = vertices[i].normalized * innerSize;
        }
        mesh.vertices = vertices;
    }

    public void UpdateOuterSize(float outerSize)
    {
        var vertices = mesh.vertices;
        for (var i = 0; i < edgeCount; i++)
        {
            vertices[i + edgeCount] = vertices[i + edgeCount].normalized * outerSize;
        }
        mesh.vertices = vertices;
    }

    public void UpdateAlpha(float alpha)
    {
        var color = material.color;
        color.a = alpha;
        material.color = color;
    }
}
