using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float playerHealth = 100f;

    public void PlayerTakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player took {damage} damage! Remaining health: {playerHealth}");

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        //To do
    }
}
