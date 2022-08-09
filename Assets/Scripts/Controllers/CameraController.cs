using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float xOffset = 5.5f;
    [SerializeField] private float yOffset = 0.5f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.position.x + xOffset, target.position.y + yOffset, -100);
    }
}
