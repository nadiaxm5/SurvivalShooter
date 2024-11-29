using Niantic.Lightship.AR.NavigationMesh;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyDamage;
    private GameObject player;
    private AnimationController animationController;

    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        player = GameObject.FindWithTag("Player");
    }

    public virtual void EnemyAttack()
    {
        //Hurt the player
        animationController.ChangeToAnimation("isAttacking");
        player.GetComponent<PlayerCombat>().PlayerTakeDamage(enemyDamage);
    }

    public virtual void EnemyStopAttack()
    {
        animationController.ChangeToAnimation("isWalking");
    }

    public virtual void EnemyTakeDamage(float damage)
    {
        animationController.ChangeToAnimation("isHurted");
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            EnemyDie();
        }
    }

    protected virtual void EnemyDie()
    {
        Destroy(gameObject);
    }
}
