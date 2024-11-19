using System.Collections;
using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;

public class EnemyFollowPlayer : MonoBehaviour
{
    private LightshipNavMeshAgent enemyAgent; // Agente de navegación del enemigo
    private GameObject player; // Referencia al player

    [SerializeField] private float updateInterval = 0.5f; // Intervalo de actualización de la posición del player

    private void Start()
    {
        // Obtiene el LightshipNavMeshAgent del enemigo
        enemyAgent = GetComponent<LightshipNavMeshAgent>();

        // Encuentra el objeto con el tag 'Player' en la escena
        player = GameObject.FindWithTag("Player");

        if (player != null && enemyAgent != null)
        {
            // Inicia la corutina para actualizar la posición del player
            StartCoroutine(UpdatePlayerDestination());
        }
        else
        {
            Debug.LogWarning("Player o LightshipNavMeshAgent no encontrados. Asegúrate de que el enemigo tiene el componente LightshipNavMeshAgent y el player tiene el tag 'Player'.");
        }
    }

    private IEnumerator UpdatePlayerDestination()
    {
        while (true)
        {
            if (player != null)
            {
                // Establece una posición cerca del player como destino
                Vector3 playerPosition = player.transform.position;
                Vector3 directionToPlayer = (playerPosition - enemyAgent.transform.position).normalized;
                Vector3 closeToPlayer = playerPosition - directionToPlayer * 0.1f;
                closeToPlayer.y = enemyAgent.transform.position.y;
                enemyAgent.SetDestination(closeToPlayer);
            }

            // Espera el intervalo antes de actualizar nuevamente
            yield return new WaitForSeconds(updateInterval);
        }
    }
}

