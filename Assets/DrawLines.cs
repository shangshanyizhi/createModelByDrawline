﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLines : MonoBehaviour {
    public GameObject drawBoard;
    public Material materialMesh;
    public Material materialLine;

    List<LineRenderer> lineRenders = new List<LineRenderer>();
    LineRenderer nowEditorLineRender;
    List<Vector3> points = new List<Vector3>();
    bool isDrawLine = false;
    bool isadjustViewer=true;
    float moudle =0;
    Vector3 dir=Vector3.zero;
	// Use this for initialization
	void Start () {
        moudle = (drawBoard.transform.position - Camera.main.transform.position).magnitude;
        es = EventSystem.current;
    }
    Ray ray;
    RaycastHit raycast;
    private EventSystem es;
    private List<RaycastResult> rr = new List<RaycastResult>();

    bool IsOverGUI(Vector2 pos)
    {
        PointerEventData ped = new PointerEventData(es);
        ped.position = pos;
        rr.Clear();
        es.RaycastAll(ped, rr);
        //for (int i = 0; i < rr.Count; i++)
        //{
        //    print(rr[i].gameObject.name);
        //}
        return rr.Count > 0;
    }
	// Update is called once per frame
	void Update () {
        if (IsOverGUI(Input.mousePosition))
        {
            return;
        }
        if (isadjustViewer&&Input.GetMouseButton(0))
        {
            float x=Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            Camera.main.transform.position += -Camera.main.transform.right * x - Camera.main.transform.up * y;
            dir = -drawBoard.transform.position + Camera.main.transform.position;
            Camera.main.transform.position = drawBoard.transform.position + Vector3.ClampMagnitude(dir, moudle);
            Camera.main.transform.LookAt(drawBoard.transform);
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out raycast))
            {
                if (raycast.collider.gameObject==drawBoard)
                {
                    if (isDrawLine)
                    {
                        points.Add(raycast.point);
                        nowEditorLineRender.positionCount =points.Count;
                        nowEditorLineRender.SetPositions(points.ToArray());
                    }
                }
            }
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartDraw();
        }
	}
    public void CompleteDrawLine()
    {
        isDrawLine = false;
        nowEditorLineRender.loop = true;
    }
    public void StartDraw()
    {

        isadjustViewer = false;
        points.Clear();
        if (nowEditorLineRender != null)
        {
            nowEditorLineRender.loop = false;
            Destroy(nowEditorLineRender.gameObject);
        }

        isDrawLine = true;
        GameObject line = new GameObject();
        line.name = "line";
        LineRenderer linerender = line.AddComponent<LineRenderer>();
        linerender.material = materialLine;
        linerender.widthCurve = AnimationCurve.Linear(0, 0.1f, 0, 0.1f);
        nowEditorLineRender = linerender;
    }
    public void AdjustViewer()
    {
        isadjustViewer = true;
    }
    GameObject noweditorModel;
    List<GameObject> Models=new List<GameObject>();
    public void CreateModel()
    {
        List<Vector3> points1 = new List<Vector3>();
        dir = -drawBoard.transform.position + Camera.main.transform.position;
        if (Vector3.Dot(dir, drawBoard.transform.right)<0)
        {
            for (int i = 0; i < points.Count; i++)
            {
                points1.Add(points[i] - drawBoard.transform.right);
            }
        }
        else
        {
            for (int i = 0; i < points.Count; i++)
            {
                points1.Add(points[i] + drawBoard.transform.right);
            }
        }
        
        GameObject obj = new GameObject();
        Models.Add(obj);
        noweditorModel = obj;
        obj.name = "model";
        MeshFilter mf = obj.AddComponent<MeshFilter>();
        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        Vector3 centerpoint = VectersAverage(points1);
        Vector3[] verticles = new Vector3[points1.Count*3];
        for (int i = 0; i < points.Count - 1; i++)
        {
            verticles[3 * i] = centerpoint;
            verticles[3 * i + 1] = points[i];
            verticles[3 * i + 2] = points[i + 1];
        }
        verticles[3 * (points.Count - 1)] = centerpoint;
        verticles[3 * (points.Count - 1) + 1] = points[points.Count - 1];
        verticles[3 * (points.Count - 1) + 2] = points[0];
        Vector3[] verticles1=points1.ToArray();
        Vector3[] verticles2 = points.ToArray();
        Vector3[] verticles3 = new Vector3[verticles1.Length+verticles2.Length];
        for (int i = 0; i < verticles1.Length; i++)
        {
            verticles3[2*i] = verticles1[i];
            verticles3[2 * i+1] = verticles2[i];
        }

        //verticles.CopyTo(verticles3, 0);
        Vector3[] verticles4 = new Vector3[verticles.Length+verticles3.Length];
        for (int i = 0; i < verticles4.Length; i++)
        {
            if (i<verticles.Length)
            {
                verticles4[i] = verticles[i];
            }
            else
            {
                verticles4[i] = verticles3[i-verticles.Length];
            }
            
        }
        int[] triangles = new int[points.Count * 3];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = i;
        }
        int[] triangles1 = new int[points.Count * 2*3];
        for (int i = 0; i < (points.Count*2- 4)/2+1; i++)
        {
            triangles1[i] = 2 * i;
            triangles1[i + 1] = 2 * i + 1;
            triangles1[i + 2] = 2 * i + 2;

            triangles1[i+3] = 2 *( i+1);
            triangles1[i + 4] = 2 * (i + 1) -1;
            triangles1[i + 5] = 2 * (i + 1) +1;
        }
        int length = triangles1.Length;
        triangles1[length - 1] = 1;
        triangles1[length - 2] = length - 1;
        triangles1[length - 3] = 0;
        triangles1[length - 4] = 0;
        triangles1[length - 5] = length - 1;
        triangles1[length - 6] = length-2;
        int[] triangles2 = new int[triangles.Length+ triangles1.Length];
        for (int i = 0; i < triangles2.Length; i++)
        {
            if (i<triangles.Length)
            {
                triangles2[i] = triangles[i];
            }
            else
            {
                triangles2[i] = triangles1[i-triangles.Length]+ triangles.Length;
            }
            
        }
        Debug.Log(verticles.Length);
        Debug.Log(verticles3.Length);
        Debug.Log(verticles4.Length);
        Debug.Log(triangles.Length);
        Debug.Log(triangles1.Length);
        Debug.Log(triangles2.Length);
        //mf.mesh.vertices = verticles4;
        //mf.mesh.triangles = triangles2;
        mf.mesh.vertices = verticles;
        mf.mesh.triangles = triangles;
        mf.mesh.RecalculateNormals();
        mr.material = materialMesh;

        StartDraw();
    }
    Vector3 VectersAverage(List<Vector3> vecs)
    {
        Vector3 average = Vector3.zero;
        for (int i = 0; i < vecs.Count; i++)
        {
            average += vecs[i];
        }
        average = average / vecs.Count;
        return average;
    }
    public void Fanzhuanfaxian()
    {
        GameObject model = noweditorModel;
        MeshFilter mf = model.GetComponent<MeshFilter>();
        int[] triangles = mf.mesh.triangles;
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = mf.mesh.triangles[triangles.Length-1-i];
        }
        mf.mesh.triangles = triangles;
        mf.mesh.RecalculateNormals();
    }
}
