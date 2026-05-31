using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float     speed = 150;
    [SerializeField] private Vector3   offset;

    void FixedUpdate()
    {
        if (target == null)
        {
            Player player = FindFirstObjectByType<Player>();
            if (player != null)
            { 
                target = player.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;

        Vector3 error = targetPosition - transform.position;
        transform.position = transform.position + error * speed;
    }
}
