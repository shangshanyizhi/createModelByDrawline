using System.Collections.Generic;
using UnityEngine;

public class Polygon : MeshDrawBase
{
    public List<Vector3> points = new List<Vector3>();
    public List<int> indexes = new List<int>();
    int index = 0;
    Color[] colors;

    void Start()
    {
        /*points.Add(new Vector3(0, 0, 0));
        points.Add(new Vector3(0, 1, 0));
        points.Add(new Vector3(1, 1, 0));
        points.Add(new Vector3(0.7f, 0.8f, 0));
        points.Add(new Vector3(1, 0.5f, 0));
        points.Add(new Vector3(0.7f, 0.3f, 0));
        points.Add(new Vector3(1, 0, 0));
        indexes.Add(0);
        indexes.Add(1);
        indexes.Add(2);
        indexes.Add(3);
        indexes.Add(4);
        indexes.Add(5);
        indexes.Add(6);*/
    }

    protected override void DrawMesh()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DrawPolygon();
        }
    }

    private void DrawPolygon()
    {
        mesh = new Mesh();
        mesh.name = "Polygon";
        mesh.vertices = points.ToArray();

        tris = Triangulation.WidelyTriangleIndex(new List<Vector3>(points), indexes).ToArray();

        mesh.triangles = tris;

        normals = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; ++i)
        {
            normals[i] = new Vector3(0, 0, 1);
        }

        mesh.normals = normals;

        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        targetFilter.mesh = mesh;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                var worldHitPos = hit.point;
                var localHitPos = transform.InverseTransformPoint(worldHitPos);

                points.Add(localHitPos);
                indexes.Add(index++);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.Reset();
        }
    }

    private void Reset()
    {
        points.Clear();

        targetFilter.mesh = null;
        Destroy(mesh);
    }

    private void OnGUI()
    {
        if (points.Count == 0) return;

        GUI.color = Color.red;

        for (int i = 0; i < points.Count; ++i)
        {
            var worldPos = transform.TransformPoint(points[i]);
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            var uiPos = new Vector3(screenPos.x, Camera.main.pixelHeight - screenPos.y, screenPos.z);

            GUI.Label(new Rect(uiPos, new Vector2(100, 80)), i.ToString());
        }
    }

    private void OnDrawGizmos()
    {
        if (points.Count == 0) return;

        Gizmos.color = Color.cyan;
        foreach (var pos in points)
        {
            var worldPos = transform.TransformPoint(pos);
            Gizmos.DrawWireSphere(worldPos, .2f);
        }
    }
}
