using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    [Header ("<b>Player Parameter</b>")]
    [Space (4)]
    [SerializeField] float moveSpeed;
    public float recoveryTime = 1.0f;
    [SerializeField] float health, maxHealth = 3f;

    [Space(10)]
    [Header("<b>UI</b>")]
    [Space(4)]
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] FloatingProductionBar productionBar;
    [SerializeField] FloatingRecoveryBar recoveryBar;

    [Space(10)]
    [Header("<b>Invoke</b>")]
    [Space(4)]
    public GameObject minions;
    public float spawnInterval = 5.0f;

    [Space(10)]
    [Header("<b>Game Object</b>")]
    [Space(4)]
    public GameObject territoryCircle;
    [Tooltip("This is the inner radius, Game Object will not spawn inside the center of the spawnRadius")]
    public float innerRadius = 1.0f;

    [Space(10)]
    [Header("<b>Sound</b>")]
    [Space(4)]
    public AudioClip takingTerritory;
    public AudioClip territoryDeployement;
    public AudioClip territoryUnlayable;

    // Private Audio
    private AudioSource audioSource;
    // Line Renderer
    private float spawnRadius;
    private LineRenderer lineRenderer;
    // Spawn & Mouse
    private bool canSpawn = true;
    private bool isMouseDragging = false;
    // When you MouseUp we put a time to make the animation
    private float timeSinceMouseUp;
    // Time
    private float currentTime;
    // Start is called before the first frame update

    //get last position
    private Vector3 lastPosition;

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
        if(isMouseDragging == false)
        {
            if (canSpawn == true)
            {
                timeSinceMouseUp = 0;
                // If the invocation time has elapsed
                if (currentTime <= 0)
                {
                    currentTime = spawnInterval;
                    // Invoke Minions
                    InstantiateTroop(minions);
                    
                }
                else
                {
                    // Decrement invocation time
                    currentTime -= Time.deltaTime;
                    productionBar.UpdateProductionBar(currentTime, spawnInterval);
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
    
    private void CalculateSpawnRadius()
    {
        // Calculate the spawnRadius depending of the size of the circle
        float circleRadius = lineRenderer.startWidth / 2f;
        spawnRadius = circleRadius;
    }

    private void InstantiateTroop(GameObject troop)
    {
        // Get a random angle within the circular sector
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the spawn position based on the random angle, inner, and outer radii
        Vector3 spawnPosition_ = transform.position + Quaternion.Euler(0f, 0f, randomAngle) * Vector3.right * Random.Range(innerRadius, spawnRadius);

        // We will instantiate Troop each time the slider ends, getting the range of the territory
        Instantiate(troop, spawnPosition_, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        DrawInnerRadiusGizmo();
    }

    private void DrawInnerRadiusGizmo()
    {
        Gizmos.color = Color.red; // You can choose any color
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }

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
}
