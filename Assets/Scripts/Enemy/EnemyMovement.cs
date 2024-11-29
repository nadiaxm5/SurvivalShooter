using System.Collections;
using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;
using TMPro;

public class EnemyMovement : MonoBehaviour
{
    private LightshipNavMeshAgent enemyAgent;
    private EnemyBase enemyBase;
    private GameObject player;

    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float separationRadius = 0.02f;
    [SerializeField] private float separationStrength = 0.01f;
    private float distanceToPlayer = 0.05f;
    private Vector3 enemyTarget;

    private void Awake()
    {
        enemyAgent = GetComponent<LightshipNavMeshAgent>();
        enemyBase = GetComponent<EnemyBase>();
        player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
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
                enemyTarget = closeToPlayer + separationForce;

                //Set the new destination
                enemyAgent.SetDestination(enemyTarget);
            }

            float distanceToDestination = Vector3.Distance(enemyAgent.transform.position, enemyTarget);

            if (distanceToDestination <= distanceToPlayer)
            {
                enemyBase.EnemyAttack();
            }

            else
            {
                enemyBase.EnemyStopAttack();
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


