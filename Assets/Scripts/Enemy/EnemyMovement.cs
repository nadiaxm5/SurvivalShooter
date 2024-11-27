using System.Collections;
using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;

public class EnemyMovement : MonoBehaviour
{
    private LightshipNavMeshAgent enemyAgent;
    private GameObject player;

    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float separationRadius = 0.02f;
    [SerializeField] private float separationStrength = 0.01f;
    private float distanceToPlayer = 0.05f;

    private void Start()
    {
        enemyAgent = GetComponent<LightshipNavMeshAgent>();
        player = GameObject.FindWithTag("Player");

        if (player != null && enemyAgent != null)
            StartCoroutine(UpdateDestination());

        else
            Debug.LogWarning("Player or LightshipNavMeshAgent not found");
    }

    //Corutine for updating the Enemy Destination 
    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            if (player != null)
            {
                //Put a position near the player as destination 
                Vector3 playerPosition = player.transform.position;
                Vector3 directionToPlayer = (playerPosition - enemyAgent.transform.position).normalized;
                Vector3 closeToPlayer = playerPosition - directionToPlayer * distanceToPlayer;
                closeToPlayer.y = enemyAgent.transform.position.y;

                //Adjust the destination with the Separation Force
                Vector3 separationForce = CalculateSeparationForce();
                Vector3 adjustedDestination = closeToPlayer + separationForce;

                //Set the new destination
                enemyAgent.SetDestination(adjustedDestination);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    //Separation Steering Behavior
    private Vector3 CalculateSeparationForce()
    {
        Vector3 separationForce = Vector3.zero;
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationRadius); //Detect colliders inside the radius

        foreach (Collider collider in nearbyEnemies)
        {
            if (collider.gameObject != this.gameObject && collider.CompareTag("Enemy")) //It is an enemy and not itself
            {
                Vector3 toNeighbor = transform.position - collider.transform.position;
                float distance = toNeighbor.magnitude; //The separation force is higher if the enemy is closer

                //Added to the player-directed destination, deviating the enemy when too close to others
                if (distance > 0)
                {
                    separationForce += toNeighbor.normalized / distance;
                }
            }
        }

        //Scale the force with the separation strenght
        return separationForce * separationStrength;
    }
}


