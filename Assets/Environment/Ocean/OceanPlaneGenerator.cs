using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanPlaneGenerator : MonoBehaviour
{
    
    //size of the mesh to be generated, in quads
    public int sizex = 4;
    public int sizez = 3;

    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
        UpdateMesh();
    }

    // Update is called once per frame
    void GenerateMesh()
    {
        triangles = new int[sizex * sizez * 6];
        vertices = new Vector3[(sizex+1) * (sizez+1)];

        for (int i=0, z=0; z <= sizez; z++){
            for (int x=0; x <= sizex; x++){
                vertices[i] = new Vector3(x,0,z);
                i++;
            }
        }

        int tris = 0;
        int verts = 0;

        for (int z=0; z < sizez; z++){
            for (int x=0; x < sizex; x++){
                triangles[tris+0] = verts + 0;
                triangles[tris+1] = verts + sizex + 1;
                triangles[tris+2] = verts + 1;

                triangles[tris+3] = verts + 1;
                triangles[tris+4] = verts + sizex + 1;
                triangles[tris+5] = verts + sizex + 2;

                verts++;
                tris += 6;
            }
            verts++;
        }

    }

    void UpdateMesh(){
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
    }
}
