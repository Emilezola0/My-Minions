using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTargeting : MonoBehaviour
{
    [SerializeField] AIDestinationSetter aIDestinationSetter;

    [Space(5)]
    [Header("<b>Choose Target with :</b>")]
    [Space(2)]

    [SerializeField] private string[] targetTags; // Add your desired tags to this array
    [SerializeField] private LayerMask targetLayer; // Change this to your desired layer
    [SerializeField] Transform territory; // Reference to the territory transform
    [SerializeField] private float goldAmount;

    private void Update()
    {
        // If the minion has gold, return to territory
        if (HasGold())
        {
            ReturnToTerritory();
            // Optionally, you can add other logic here based on having gold
        }
        else
        {
            // If the minion doesn't have gold, find the nearest target as before
            FindNearestTarget();
        }
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

    public void ReturnToTerritory()
    {
        // Check if the current target is not the territory
        if (aIDestinationSetter.target != territory)
        {
            // Set the destination to the territory's position
            aIDestinationSetter.target = territory;
        }
    }

    public bool HasGold()
    {
        // Implement the logic to check if the minion has gold
        // For example, return true if goldAmount is greater than 0
        return goldAmount > 0;
    }

    public float GetGoldAmount()
    {
        // Return the gold amount
        return goldAmount;
    }

    public void RemoveGold()
    {
        // Implement the logic to remove gold from the minion
        // For example, set goldAmount to 0
        goldAmount = 0;
    }

    public void ReceiveGold(float amount)
    {
        // Implement the logic to receive gold and add it to the current goldAmount
        goldAmount += amount;
    }

    public void ReturnToTerritoryWithGold()
    {
        // Check if the current target is not the territory
        if (aIDestinationSetter.target != territory)
        {
            // Set the destination to the territory's position
            aIDestinationSetter.target = territory;
        }

        // Add the logic here to handle other aspects of returning to territory with gold
        // For example, you might want to perform specific actions when returning to territory with gold
    }

    public void SetTarget(Transform target)
    {
        territory = target;
    }
}
