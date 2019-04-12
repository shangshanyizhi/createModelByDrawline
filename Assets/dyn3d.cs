using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dyn3d : MonoBehaviour
{
    public Material mat;
    // Use this for initialization
    void Start()
    {

        CreateCube();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateCube()
    {

        GameObject obj = new GameObject("cube");
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        mr.material = mat;

        Vector3[] vertices = new Vector3[24];
        int[] triangles = new int[36];

        //forward
        vertices[0].Set(0.5f, -0.5f, 0.5f);
        vertices[1].Set(-0.5f, -0.5f, 0.5f);
        vertices[2].Set(0.5f, 0.5f, 0.5f);
        vertices[3].Set(-0.5f, 0.5f, 0.5f);
        //back
        vertices[4].Set(vertices[2].x, vertices[2].y, -0.5f);
        vertices[5].Set(vertices[3].x, vertices[3].y, -0.5f);
        vertices[6].Set(vertices[0].x, vertices[0].y, -0.5f);
        vertices[7].Set(vertices[1].x, vertices[1].y, -0.5f);
        //up
        vertices[8] = vertices[2];
        vertices[9] = vertices[3];
        vertices[10] = vertices[4];
        vertices[11] = vertices[5];
        //down
        vertices[12].Set(vertices[10].x, -0.5f, vertices[10].z);
        vertices[13].Set(vertices[11].x, -0.5f, vertices[11].z);
        vertices[14].Set(vertices[8].x, -0.5f, vertices[8].z);
        vertices[15].Set(vertices[9].x, -0.5f, vertices[9].z);
        //right
        vertices[16] = vertices[6];
        vertices[17] = vertices[0];
        vertices[18] = vertices[4];
        vertices[19] = vertices[2];
        //left
        vertices[20].Set(-0.5f, vertices[18].y, vertices[18].z);
        vertices[21].Set(-0.5f, vertices[19].y, vertices[19].z);
        vertices[22].Set(-0.5f, vertices[16].y, vertices[16].z);
        vertices[23].Set(-0.5f, vertices[17].y, vertices[17].z);

        int currentCount = 0;
        for (int i = 0; i < 24; i = i + 4)
        {
            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 3;
            triangles[currentCount++] = i + 1;

            triangles[currentCount++] = i;
            triangles[currentCount++] = i + 2;
            triangles[currentCount++] = i + 3;

        }

        mf.mesh.vertices = vertices;
        mf.mesh.triangles = triangles;
        mf.mesh.RecalculateNormals();

        //mf.mesh.normals = vertices;

    }
}
