using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90.0f;
    public int rayCount = 2;
    public float startingAngle = 0.0f;
    public float viewDistance = 50.0f;
    private Vector3 origin;

    private Mesh mesh;
    [SerializeField] private LayerMask layerMask;


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate() {
        GenerateMesh();
    }

    private void GenerateMesh() {
        float angle = startingAngle;
        float angleIncrease = fov/rayCount;
        
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        //Raycount + origin + 0 ray
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if(raycastHit.collider == null) {
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            } else {
                vertex = raycastHit.point;
            }
            vertices[vertexIndex] = vertex;
            
            if(i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex ++; 
            angle -= angleIncrease;

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle*(Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    private float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0) n += 360;

        return n;
    }

    public void SetFov(float fov) {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance) {
        this.viewDistance = viewDistance;
    } 

    public void Toggle(bool tog) {
        GetComponent<MeshRenderer>().enabled = tog;
    }
}
