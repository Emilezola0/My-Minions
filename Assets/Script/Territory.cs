using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    #region PlayerParameter

    [Header("Speed")]
    [Space(1)]
    [SerializeField] float moveSpeed;
    public float recoveryTime = 1.0f;
    // Add a public variable for the speed progression factor
    public float speedProgressionFactor = 0.1f;
    [Space(10)]

    #endregion

    #region HP

    [Space(4)]
    [Header("HP")]
    [Space(1)]
    [SerializeField] float health, maxHealth = 3f;
    [Space(10)]

    #endregion

    #region UI

    [Header("Sliders")]
    [Space(4)]
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] FloatingProductionBar productionBar;
    [SerializeField] FloatingRecoveryBar recoveryBar;
    [Space(10)]

    #endregion

    #region Invoke

    [Header("Minions")]
    [Space(1)]
    public GameObject minions;
    public float spawnInterval = 5.0f;
    [Space(10)]

    #endregion

    #region GameObject

    [Header("Circle Reference")]
    [Space(1)]
    public GameObject territoryCircle;
    [Tooltip("This is the inner radius, Game Object will not spawn inside the center of the spawnRadius")]
    public float innerRadius = 1.0f;
    [Space(10)]

    #endregion

    #region Sounds

    [Header("Sound")]
    [Space(1)]
    public AudioClip takingTerritory;
    public AudioClip territoryDeployement;
    public AudioClip territoryUnlayable;
    [Space(10)]

    #endregion

    #region PrivateVar

    // Private Audio
    private AudioSource audioSource;
    // Line Renderer
    [SerializeField] private float spawnRadius;
    private LineRenderer lineRenderer;
    // Spawn & Mouse
    private bool canSpawn = true;
    private bool isMouseDragging = false;
    // When you MouseUp we put a time to make the animation
    private float timeSinceMouseUp;
    // Time
    private float currentTime;
    // Start is called before the first frame update
    // Create a list of minions to know if they are inside the box when I move
    private List<MinionTargeting> minionsInside = new();
    // Circle Controller Script
    [SerializeField] CircleController circleController;

    //get last position
    private Vector3 lastPosition;

    #endregion

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        productionBar = GetComponentInChildren<FloatingProductionBar>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = territoryCircle.GetComponent<LineRenderer>();

        // Update health bar hp on start
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);

        // Set Spawning UI
        currentTime = spawnInterval;

        // Set RecoveryTime Inactive (only activate on use)
        timeSinceMouseUp = 0;
        recoveryBar.UpdateRecoveryBar(timeSinceMouseUp, recoveryTime);
        recoveryBar.gameObject.SetActive(false);

        // Calculate Spawn Radius
        CalculateSpawnRadius();

    }
    // Update is called once per frame
    void Update()
    {
        //
        if (isMouseDragging == false)
        {
            if (canSpawn == true)
            {
                timeSinceMouseUp = 0;

                // Calculate the current spawn interval based on the level and progression factor
                float currentSpawnInterval = spawnInterval - (circleController.level * speedProgressionFactor);

                // If the invocation time has elapsed
                if (currentTime <= 0)
                {
                    currentTime = currentSpawnInterval;
                    // Invoke Minions
                    InstantiateTroop(minions, transform);
                }
                else
                {
                    // Decrement invocation time
                    currentTime -= Time.deltaTime;
                    productionBar.UpdateProductionBar(currentTime, currentSpawnInterval);
                }
            }
            // If you cannot summon, count the time since the last minion.
            else
            {
                // The time since the last minion has elapsed, reset the ability to invoke
                if (timeSinceMouseUp > recoveryTime)
                {
                    canSpawn = true;
                    recoveryBar.gameObject.SetActive(false);
                }
                else
                {
                    timeSinceMouseUp += Time.deltaTime;
                    recoveryBar.UpdateRecoveryBar(timeSinceMouseUp, recoveryTime);
                }
            }
        }

        CalculateSpawnRadius();
    }

    #region MouseEvent

    private void OnMouseDown()
    {

        lastPosition = transform.position; // set lastPosition when we take the territory

        PlaySoundOneShot(takingTerritory);

        // Make it impossible to spawn on mouse drag
        canSpawn = false;

        // Reset Timer of Recovery
        timeSinceMouseUp = 0;

        // Calculate Spawn Radius
        CalculateSpawnRadius();
    }

    private void OnMouseUp()
    {
        // Check if the territory is over the "Environment" layer
        int environmentLayer = LayerMask.NameToLayer("Environment");
        float checkRadius = 0.5f; // Adjust the radius as needed

        // Check if there is an object on the "Environment" layer within the specified radius of the territory's position
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, checkRadius, 1 << environmentLayer);

        if (hitCollider != null)
        {
            // If colliding with the "Environment" layer, update the territory position to the last position
            transform.position = lastPosition;
        }

        PlaySoundOneShot(territoryDeployement);

        // Create a separate list to store minions that need to be processed
        List<MinionTargeting> minionsToProcess = new List<MinionTargeting>(minionsInside);

        // Process gold for all minions in the list
        foreach (MinionTargeting minionTargeting in minionsToProcess)
        {
            // Process gold for each minion (you can customize this)
            ProcessGold(minionTargeting.GetGoldAmount());

            // Now, you might want to remove the gold from the minion or handle it in MinionScript
            minionTargeting.RemoveGold();

            // Remove the minion from the original list
            minionsInside.Remove(minionTargeting);
        }

        // Clear the list after processing gold for all minions
        minionsToProcess.Clear();

        // Activate Spawning
        if (!isMouseDragging)
        {
            canSpawn = true; // Can Spawn Troop

            timeSinceMouseUp = 0f; // Reset time since MouseUp
        }

        isMouseDragging = false; // Reset isMouseDragging to false after MouseUp

        recoveryBar.gameObject.SetActive(true); // make recovery bar visible
    }

    private void OnMouseDrag()
    {
        isMouseDragging = true;

        // Get the Mouse Position
        Vector3 mousePosition_ = Input.mousePosition;
        mousePosition_.z = 0;

        // Then get The World Mouse Position
        Vector3 worldMousePosition_ = Camera.main.ScreenToWorldPoint(mousePosition_);
        worldMousePosition_.z = 0;

        // Assign the Position to the object and Lerp by the Vitesse Deplacement
        transform.position = Vector3.Lerp(transform.position, worldMousePosition_, moveSpeed * Time.deltaTime);

        // Debug.Log("Position de la souris : " + worldMousePosition);
    }

    #endregion

    #region Sounds
    private void PlaySoundOneShot(AudioClip sound)
    {
        if (audioSource != null)
        {
            // Assign the audio clip to the Audio Source
            audioSource.clip = sound;

            // Play Sound
            audioSource.Play();
        }
        else
        {
            Debug.Log("No AudioClip have been detected");
        }
    }

    #endregion

    private void CalculateSpawnRadius()
    {
        // Calculate the spawnRadius depending of the size of the circle
        float circleRadius = lineRenderer.startWidth / 2f;
        spawnRadius = circleRadius;
    }

    private void InstantiateTroop(GameObject troop, Transform target)
    {
        // Get a random angle within the circular sector
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the spawn position based on the random angle, inner, and outer radii
        Vector3 spawnPosition_ = transform.position + Quaternion.Euler(0f, 0f, randomAngle) * Vector3.right * Random.Range(innerRadius, spawnRadius);

        // Instantiate Troop at the calculated position
        GameObject newTroop = Instantiate(troop, spawnPosition_, Quaternion.identity);

        // Assuming the troop has a MinionTargeting component
        MinionTargeting minionTargeting = newTroop.GetComponent<MinionTargeting>();

        // Set the target for the minion
        if (minionTargeting != null)
        {
            minionTargeting.SetTarget(target);
        }
    }

    #region Draw

    private void OnDrawGizmos()
    {
        DrawInnerRadiusGizmo();
    }

    private void DrawInnerRadiusGizmo()
    {
        Gizmos.color = Color.red; // You can choose any color
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }

    #endregion

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth); // Update health bar hp
        // Check if we are dead
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Make effect of explosion like the territory is detroy
        // Spawn UI like Game Over if it's the last territory
    }

    #region Collisions

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMouseDragging == false)
        {
            // Check if the entering collider has the "Minion" tag
            if (other.CompareTag("Minion"))
            {
                // Assuming minions have a script called MinionTargeting
                MinionTargeting minionTargeting = other.GetComponent<MinionTargeting>();

                // Check if the minion has gold
                if (minionTargeting != null && minionTargeting.HasGold())
                {
                    // Do something when a minion with gold enters the territory
                    // For example, call a method to process the gold
                    ProcessGold(minionTargeting.GetGoldAmount());

                    // Now, you might want to remove the gold from the minion or handle it in MinionScript
                    minionTargeting.RemoveGold();
                }
            }
        }
        else
        {
            // Add the minion to the list of minions inside the trigger zone
            MinionTargeting minionTargeting = other.GetComponent<MinionTargeting>();
            if (minionTargeting != null && minionTargeting.HasGold())
            {
                minionsInside.Add(minionTargeting);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Minion"))
        {
            MinionTargeting minionTargeting = other.GetComponent<MinionTargeting>();
            // Remove the minion from the list when it exits the trigger zone
            minionsInside.Remove(minionTargeting);
        }
    }

    #endregion

    // ProcessGold trigger also the Upgrade Territory inside the CircleController.cs
    private void ProcessGold(float goldAmount)
    {
        // Implement the logic to process gold here
        circleController.UpgradeTerritory(goldAmount);
        Debug.Log("Number of Gold : " + goldAmount); // Debug
    }

}
