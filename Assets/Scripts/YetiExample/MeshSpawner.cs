using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpawner : MonoBehaviour
{

    [SerializeField] private GameObject arPrefab;
    [SerializeField] private float spawnInterval = 10.0f;

    private Camera arCamera;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private int minVertex = 250; //Min vertices que debe tener bloque malla para crear objetos

    private float timeSinceLastSpawn = 11.0f;
    private float normalHorizontalTolerance = 0.9f; //Validar si es superficie horizontal

    private float minDisYOrigin = 0.15f; //Distancia min vertice a centro de malla

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        arCamera = GameManager.Instance.ARCamera;
    }

    void Update()
    {
        int vertexCount = meshFilter.sharedMesh.vertexCount;
        if(vertexCount < minVertex)
        {
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= spawnInterval && IsVisibleToCamera())
        {
            TryToSpawnARObject();
        }
    }

    private bool IsVisibleToCamera()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(arCamera);

        //Saber si limites caja objeto están dentro o intersectan planos frustrum
        return GeometryUtility.TestPlanesAABB(planes, meshRenderer.bounds);
    }

    private void TryToSpawnARObject()
    {
        Mesh mesh = meshFilter.sharedMesh;

        int indexVertex = Random.Range(0, mesh.vertexCount);

        Vector3 normal = mesh.normals[indexVertex];

        bool isHorizontal = normal.y > normalHorizontalTolerance;

        Vector3 vertexPosition = mesh.vertices[indexVertex];
        Vector3 globalPosition = transform.TransformPoint(vertexPosition);

        bool isCloseToYOrigin = globalPosition.y < minDisYOrigin;

        if(isHorizontal && isCloseToYOrigin)
        {
            Vector3 randomYRot = Vector3.up * Random.Range(0, 360);

            GameObject arObject = Instantiate(arPrefab, globalPosition, Quaternion.Euler(randomYRot), transform);
            timeSinceLastSpawn = 0;
        }
    }
}
