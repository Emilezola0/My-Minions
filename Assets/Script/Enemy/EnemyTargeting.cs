using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    [SerializeField] AIDestinationSetter aIDestinationSetter;
    
    [Space(5)]
    [Header("<b>Choose Target with :</b>")]
    [Space(2)]

    [SerializeField] private string[] targetTags; // Add your desired tags to this array
    [SerializeField] private LayerMask targetLayer; // Change this to your desired layer

    private void Update()
    {
        FindNearestTarget();
    }

    private void FindNearestTarget()
    {
        GameObject nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (string targetTag in targetTags)
        {
            // Find all objects with the specified tag or on the specified layer
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (var target in targets)
            {
                if (target.layer == targetLayer)
                {
                    // Calculate the distance between this object and the target
                    float distance = Vector3.Distance(transform.position, target.transform.position);

                    // Check if this target is closer than the current nearest target
                    if (distance < nearestDistance)
                    {
                        nearestTarget = target;
                        nearestDistance = distance;
                    }
                }
            }
        }

        // At this point, nearestTarget will be the closest object with the specified tags and on the specified layer
        if (nearestTarget != null)
        {
            // Do something with the nearest target (e.g., set it as the destination)
            aIDestinationSetter.target = nearestTarget.transform;
            // Example: GetComponent<AIDestinationSetter>().SetTarget(nearestTarget.transform);
        }
    }
}