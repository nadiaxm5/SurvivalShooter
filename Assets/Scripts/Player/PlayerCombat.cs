using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private Ray shootRay = new Ray();                   //Ray from the end of the riffle
    private RaycastHit shootHit;                        //To know what was hit
    private LineRenderer shootLine;
    private Light shootLight;
    private Weapon activeWeapon;
    private float effectsDisplayTime = 0.05f;            //Proportion of the time the effects will display for
    private bool isShooting = false;
    private Coroutine shootingCoroutine;
    private GameObject closestEnemy;

    private void Awake()
    {
        activeWeapon = GetComponentInChildren<Weapon>(); //The weapon is a child of the player
        shootLine = activeWeapon.GetComponent<LineRenderer>();
        shootLight = activeWeapon.GetComponent<Light>();
        DisableEffects();
    }

    private void Start()
    {
        // Inicia la corutina de disparo
        if (activeWeapon != null)
        {
            StartShooting();
        }
    }

    private void Update()
    {
        closestEnemy = GetClosestEnemy();
    }

    private IEnumerator ShootingCoroutine()
    {
        while (isShooting)
        {
            Shoot();
            yield return new WaitForSeconds(activeWeapon.weaponCooldown * effectsDisplayTime);
            DisableEffects();
            yield return new WaitForSeconds(activeWeapon.weaponCooldown);
        }
    }

    private void StartShooting()
    {
        if (!isShooting) // Evita iniciar múltiples coroutines
        {
            isShooting = true;
            shootingCoroutine = StartCoroutine(ShootingCoroutine());
        }
    }

    private void StopShooting()
    {
        isShooting = false; // Detiene el disparo
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private void Shoot()
    {
        if (closestEnemy != null)
        {
            //Visual effects
            shootLight.enabled = true;
            shootLine.enabled = true;
            Vector3 laserOrigin = activeWeapon.transform.GetChild(1).position;
            shootLine.SetPosition(0, laserOrigin);

            //Raycast control
            shootRay.origin = laserOrigin;
            Vector3 enemyPosition = closestEnemy.GetComponent<Collider>().bounds.center;
            shootRay.direction = (enemyPosition - laserOrigin).normalized;


            if (Physics.Raycast(shootRay.origin, shootRay.direction, out shootHit, activeWeapon.weaponRange))
            {
                Debug.Log(shootHit.transform.name);
                if (shootHit.collider.CompareTag("Enemy"))
                {
                    shootLine.SetPosition(1, shootHit.point);
                    //The player shoots to the closest Enemy but may hit another enemy, check which one
                    EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                    Debug.Log($"I hit an enemy");

                    enemyHealth?.TakeDamage(activeWeapon.weaponDamage);
                }
            }
            else
            {
                shootLine.SetPosition(1, shootRay.origin + shootRay.direction * activeWeapon.weaponRange);
                Debug.Log("I hit nothing");
            }
        }
        else
            Debug.Log("closest enemy es null");
    }

    public void DisableEffects()
    {
        shootLine.enabled = false;
        shootLight.enabled = false;
    }

    private GameObject GetClosestEnemy()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, activeWeapon.weaponRange); //Detect colliders inside the weapon range
        Collider closestEnemyCollider = null;
        float closestDist = Mathf.Infinity;

        //Choose the closest enemy collider
        foreach (Collider collider in nearbyEnemies)
        {
            if (collider.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(transform.position, collider.transform.position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestEnemyCollider = collider;
                }
            }
        }
        if (closestEnemyCollider != null)
            return closestEnemyCollider.gameObject;

        return null;
    }
}