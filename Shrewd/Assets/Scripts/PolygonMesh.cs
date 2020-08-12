using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PolygonMesh : MonoBehaviour
{
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh = new Mesh();
        GenerateVertices(8);
    }

    void GenerateVertices(int edgeCount)
    {
        mesh.Clear();

        var vertices = new Vector3[edgeCount];
        var triangles = new int[(edgeCount - 2) * 3];

        for (var i = 0; i < edgeCount; i++)
        {
            var angle = 360.0f / edgeCount * i;
            vertices[i] = Quaternion.Euler(0, 0, angle) * Vector3.right;
        }

        for (var i = 0; i < edgeCount - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
