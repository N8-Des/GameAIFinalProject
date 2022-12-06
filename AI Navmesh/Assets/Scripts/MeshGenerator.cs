using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class MeshGenerator : MonoBehaviour
{

    public Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] UVs;
    //YES I GET TO USE KINDA SHADERS
    Color[] colors;
    public int sizeX = 40;
    public int sizeZ = 40;
    public float noiseAmplitude = 2f;
    public float noiseFrequency = 0.3f;
    //silly soltion to allow me to inverse lerp. 
    float minTerrHeight;
    float maxTerrHeight;
    public Gradient heightmap;
    public GameObject wallObject;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //create vertices size based on size of mesh
        vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];
        //assign vertices to point
        float randXOffset = UnityEngine.Random.Range(-500.0f, 500.0f);
        float randYOffset = UnityEngine.Random.Range(-500.0f, 500.0f);
        int index = 0;
        for (int i = 0; i <= sizeZ; i++)
        {
            for (int j = 0; j <= sizeX; j++)
            {
                float yOffset = (Mathf.PerlinNoise((float)(j + randXOffset) * noiseFrequency, (float)(i  + randXOffset)* noiseFrequency) * noiseAmplitude);
                vertices[index] = new Vector3(j, yOffset, i);
                //classic get min and max values
                if (yOffset > maxTerrHeight)
                {
                    maxTerrHeight = yOffset;
                }
                if (yOffset < minTerrHeight)
                {
                    minTerrHeight = yOffset;
                }
                index++;
            }
        }

        //this is the sucky part, I need to create a bunch of tris.
        triangles = new int[sizeX * sizeZ * 6];

        int vert = 0;
        int tris = 0;
        //double for loop again. This time with some fancy workarounds to work with anything. 
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + sizeX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + sizeX + 1;
                triangles[tris + 5] = vert + sizeX + 2;
                vert++;
                //six verts per "square" so we add 6 here.
                tris += 6;
            }
            vert++;
        }

        //I am lerping the color based on the Y-position of the vertex. Sound familiar? ;)
        //Unity has some built-in gradient functions so its much easier. 
        colors = new Color[vertices.Length];
        index = 0;
        for (int i = 0; i <= sizeZ; i++)
        {
            for (int j = 0; j <= sizeX; j++)
            {
                //I CAN USE VERTEX COLORS THIS IS BASICALLY BLENDER LETS GOOO 
                //gradient.evaulaute essentially does that fancy thing I did in my noise generation code. turns a value into a color based on the gradient map. 

                float h = vertices[index].y;
                //here's the inverse lerp I was talking about.
                h = Mathf.InverseLerp(minTerrHeight, maxTerrHeight, vertices[index].y);
                colors[index] = heightmap.Evaluate(h);
                index++;
            }
        }
        index = 0;
        //generate UVs because its cool 
        UVs = new Vector2[vertices.Length];
        for (int i = 0; i <= sizeZ; i++)
        {
            for (int j = 0; j <= sizeX; j++)
            {
                //I CAN USE VERTEX COLORS THIS IS BASICALLY BLENDER LETS GOOO 
                //gradient.evaulaute essentially does that fancy thing I did in my noise generation code. turns a value into a color based on the gradient map. 

                UVs[index] = new Vector2((float)j / sizeX, (float)i / sizeZ);
                index++;
            }
        }
        //generate walls on point.
        for (int r = 0; r < 11; r++)
        {
            int randIndex = UnityEngine.Random.Range(0, vertices.Length - 1);
            float randRot = UnityEngine.Random.Range(-360f, 360f);
            Vector3 randomRotation = new Vector3(0, randRot, 0);
            Instantiate(wallObject, vertices[randIndex], Quaternion.Euler(randomRotation));
        }
        //generate mesh and mesh collider
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.uv = UVs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        NavMeshSurface surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
