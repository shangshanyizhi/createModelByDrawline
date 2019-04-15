using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeshDrawBase : MonoBehaviour
{

    protected MeshFilter targetFilter;
    protected Mesh mesh;
    protected int[] tris;
    protected Vector2[] uvs;
    protected Vector3[] normals;

    // Use this for initialization
    void Awake()
    {
        targetFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        DrawMesh();
    }

    protected abstract void DrawMesh();
}