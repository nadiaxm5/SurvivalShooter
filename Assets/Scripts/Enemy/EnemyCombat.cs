using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackDamage = 1f;
    public float attackRange = 0.05f;
    private GameObject player;
    private AnimationController animationController;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animationController = GetComponent<AnimationController>();
    }

    public void Attack()
    {
        Debug.Log($"{gameObject.name} is attacking.");
        player.GetComponent<PlayerCombat>()?.PlayerTakeDamage(attackDamage); //Only attacks if playerCombat is not null
        animationController?.ChangeToAnimation("isAttacking");
    }
}
