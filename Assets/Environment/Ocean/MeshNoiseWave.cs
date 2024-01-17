using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshNoiseWave : MonoBehaviour
{

    public Texture2D heightmap;

    public float strength = 2f;

    [Range(1f, 3f)]
    public float scale = 1f;
    
    Mesh mesh;
    Vector3 meshSize;
    int meshEdgeLength;

    Vector3[] vertices, modifiedVerts;
    

    // Start is called before the first frame update
    void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        meshSize = mesh.bounds.size;
        meshEdgeLength = Mathf.RoundToInt(Mathf.Sqrt(mesh.vertices.Length));

        vertices = (Vector3[]) mesh.vertices.Clone();

        modifiedVerts = (Vector3[]) mesh.vertices.Clone();

        RecalculateMesh();
    }

    void RecalculateMesh(){
        //mesh = GetComponent<MeshFilter>().mesh;
        mesh.vertices = modifiedVerts;
        //GetComponentInChildren<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    private bool dumb = true;
    // Update is called once per frame
    void Update()
    {
        if (dumb){
            Start();
            dumb = false;
        }
        for (int v=0; v<modifiedVerts.Length; v++){
            int x = Mathf.FloorToInt(v / meshEdgeLength / (float)meshEdgeLength * heightmap.width);
            int y = Mathf.FloorToInt(v % meshEdgeLength / (float)meshEdgeLength * heightmap.height);
            modifiedVerts[v].y = strength * heightmap.GetPixel(x,y).grayscale - strength/2;
            //Debug.Log(x.ToString() + " " + y.ToString());
        }

        RecalculateMesh();
    }
}
