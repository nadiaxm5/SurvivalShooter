using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float force = 450.0f;
    [SerializeField] private Transform pointer;

    private Camera arCamera;

    void Start()
    {
        arCamera = GameManager.Instance.ARCamera;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Input.mousePosition : Input.GetTouch(0).position;

            Ray ray = arCamera.ScreenPointToRay(inputPosition);

            Vector3 direction;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                direction = hit.point - pointer.position;
            }
            else
            {
                direction = pointer.forward;
            }

            GameObject ball = Instantiate(ballPrefab, pointer.position, Quaternion.identity, transform);
            ball.GetComponent<Rigidbody>().AddForce(direction.normalized * force);
        }
    }
}
