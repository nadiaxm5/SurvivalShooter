using System.Collections;
using UnityEngine;
using Niantic.Lightship.AR.NavigationMesh;
using TMPro;

public class EnemyMovement : MonoBehaviour
{
    private LightshipNavMeshAgent enemyAgent;
    [SerializeField] private EnemyCombat enemyCombat;
    private GameObject player;
    private AnimationController animationController;

    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private float separationRadius = 0.02f;
    [SerializeField] private float separationStrength = 0.01f;
    private float rangeToPlayer;
    private Vector3 enemyTarget;

    public float enemySpeed = 0.5f;

    private void Awake()
    {
        enemyAgent = GetComponent<LightshipNavMeshAgent>();
        enemyCombat = GetComponent<EnemyCombat>();
        player = GameObject.FindWithTag("Player");
        animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        if (player != null && enemyAgent != null)
            StartCoroutine(UpdateDestination());

        else
            Debug.LogWarning("Player or LightshipNavMeshAgent not found");

        rangeToPlayer = enemyCombat.attackRange;
    }

    //Corutine for updating the Enemy Destination 
    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            if (player != null)
            {
                //Destination near player
                Vector3 playerPosition = player.transform.position;
                Vector3 directionToPlayer = (playerPosition - enemyAgent.transform.position).normalized;
                Vector3 closeToPlayer = playerPosition - directionToPlayer * rangeToPlayer;
                closeToPlayer.y = enemyAgent.transform.position.y;

                //Adjust destination with group steering behaviour
                Vector3 separationForce = CalculateSeparationForce();
                enemyTarget = closeToPlayer + separationForce;

                //Set destination
                enemyAgent.SetDestination(enemyTarget);

                //Chack if it has to attack
                CheckIfAttack();
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

    private void CheckIfAttack()
    {
        float distanceToDestination = Vector3.Distance(enemyAgent.transform.position, enemyTarget);

        //enemyAgent.SetDestination(player.transform.position);  // Forzar destino hacia el jugador

        if (distanceToDestination <= rangeToPlayer + 0.01f)
        {
            enemyCombat?.Attack(); //Only attacks if the necessary components are not null
        }
        else
        {
            animationController?.ChangeToAnimation("isWalking");
        }
    }
}


