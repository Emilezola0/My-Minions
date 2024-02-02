using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [Header("<b>Gold & Sprite: </b>")]
    [Space(2)]
    public int goldAmount = 0; // Initial gold amount
    [Tooltip("Reference to the SpriteRenderer Component")]
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    [Space(10)]

    [Header("<b>Liste Of Sprite: </b>")]
    [Space(2)]
    [Tooltip("For each Sprite you need a tresholdValues if goldAmount <= tresholdValues then you take the sprite")]
    public Sprite[] sprites; // Array of sprites
    public float[] thresholdValues; // Array of corresponding threshold values

    // Start is called before the first frame update
    void Start()
    {
        UpdateSpriteBasedOnGoldAmount();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any additional logic in the Update method if needed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider has the "Minion" tag
        if (other.CompareTag("Minion"))
        {
            // Assuming minions have a script called MinionTargeting
            MinionTargeting minionTargeting = other.GetComponent<MinionTargeting>();

            // Check if the minion has gold
            if (minionTargeting != null)
            {
                // If the minion has gold, do not add more
                if (minionTargeting.HasGold())
                {
                    Debug.Log("Minion already has gold.");
                    minionTargeting.ReturnToTerritory();
                }
                else
                {
                    // If the minion doesn't have gold, add gold and notify the minion
                    float takenGold = TakeGold();
                    minionTargeting.ReceiveGold(takenGold);

                    // Return to territory if the minion has gold
                    minionTargeting.ReturnToTerritory();

                    // Now, you might want to remove the gold from the GoldManager
                }
            }
        }
    }

    float TakeGold()
    {
        // You can implement the logic to take gold from the GoldManager
        // For now, let's assume we take all the gold
        float takenGold = goldAmount;
        goldAmount = 0; // Set gold amount to 0 after taking

        // Update the sprite based on the new gold amount
        UpdateSpriteBasedOnGoldAmount();

        // Destroy the GoldManager object
        Destroy(gameObject);

        return takenGold;
    }

    void UpdateSpriteBasedOnGoldAmount()
    {
        // Ensure that the arrays have the same length
        if (sprites.Length != thresholdValues.Length)
        {
            Debug.LogError("The length of 'sprites' and 'thresholdValues' arrays must be the same.");
            return;
        }

        // Loop through the arrays to find the appropriate sprite
        for (int i = 0; i < sprites.Length; i++)
        {
            // Check if goldAmount is less than or equal to the current threshold value
            if (goldAmount <= thresholdValues[i])
            {
                // Set the sprite based on the current index
                spriteRenderer.sprite = sprites[i];
                return; // Exit the loop once a suitable sprite is found
            }
        }

        // If none of the thresholds are met, use the last sprite in the array
        spriteRenderer.sprite = sprites[sprites.Length - 1];
    }
}
