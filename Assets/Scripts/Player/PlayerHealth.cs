using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float playerHealth = 100f;

    public void PlayerTakeDamage(float damage)
    {
        //Decreases player's health by given damage and checks death
        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage. Remaining health: {playerHealth}");

        if (playerHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Debug.Log("Player has died.");
        //To do
    }
}
