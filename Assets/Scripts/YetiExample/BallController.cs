using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //SCRIPT PARA CONTROLAR COLLIDER Y DESTRUCCIÓN PELOTAS

    [SerializeField] private float autoDestroyTime = 20.0f;
    private float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > autoDestroyTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            GameManager.Instance.UpdateScore(5.0f);
        }
    }
}
