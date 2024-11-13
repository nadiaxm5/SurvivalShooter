using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Spawner de enemigos

    //[SerializeField] private GameObject arPrefab;
    //[SerializeField] private float spawnInterval = 10.0f;

    //private Camera arCamera;
    //private MeshRenderer meshRenderer;
    //private MeshFilter meshFilter;

    //private float timeSinceLastSpawn = 11.0f;

    //private float minDistToTarget = 0.15f;
    //private float maxDistToTarget = 0.75f;

    //Primero hacer que spawnee 1 solo enemigo cerca del player

    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo que queremos instanciar
    [SerializeField] private float minSpawnDistance = 0.5f; // Distancia m�nima para el spawn del enemigo
    [SerializeField] private float maxSpawnDistance = 3f; // Distancia m�xima para el spawn del enemigo
    [SerializeField] private float spawnInterval = 2f; // Intervalo de tiempo entre cada aparici�n de enemigo

    private GameObject player; // Referencia al player ya colocado

    private void Start()
    {
        // Nos suscribimos al evento para que el enemigo aparezca despu�s de que el player se haya colocado
        PlacementOnMesh_Character.characterPlaced += StartSpawningEnemies;
    }

    private void OnDestroy()
    {
        // Nos desuscribimos del evento cuando el objeto se destruye para evitar errores de referencia
        PlacementOnMesh_Character.characterPlaced -= StartSpawningEnemies;
    }

    private void StartSpawningEnemies()
    {
        // Encuentra el objeto `player` en la escena
        player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Inicia la corutina para spawn continuo de enemigos cada 3 segundos
            StartCoroutine(SpawnEnemyCoroutine());
        }
        else
        {
            Debug.LogWarning("Player no encontrado. Aseg�rate de que el objeto tiene el tag 'Player'.");
        }
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            // Genera un enemigo en una posici�n cercana al player
            SpawnEnemy();

            // Espera el intervalo de tiempo antes de generar el siguiente enemigo
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        // Calcula una distancia aleatoria entre el m�nimo y el m�ximo de distancia al player
        float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);

        // Genera una posici�n aleatoria en un punto unitario en la esfera, a la distancia calculada
        Vector3 randomPoint = Random.onUnitSphere;
        randomPoint.y = 0; // Limita la direcci�n al plano horizontal

        Vector3 spawnPosition = player.transform.position + randomPoint.normalized * spawnDistance;
        spawnPosition.y = player.transform.position.y; // Ajusta la altura para que coincida con la del player

        // Instancia el enemigo en la posici�n calculada
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
