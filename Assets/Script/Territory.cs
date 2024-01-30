using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    [Header ("<b>Player Parameter</b>")]
    [Space (4)]
    public float vitesseDeplacement;
    public float recoveryTime = 1.0f;

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
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = territoryCircle.GetComponent<LineRenderer>();
        
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
                }
            }
            // If you cannot summon, count the time since the last minion.
            else
            {
                // The time since the last minion has elapsed, reset the ability to invoke
                if (timeSinceMouseUp > recoveryTime)
                {
                    canSpawn = true;
                }
                else
                {
                    timeSinceMouseUp += Time.deltaTime;
                }
            }
        }
    }
    private void OnMouseDown()
    {
        PlaySoundOneShot(takingTerritory);

        // Make it impossible to spawn on mouse drag
        canSpawn = false;

        // Calculate Spawn Radius
        CalculateSpawnRadius();
    }

    private void OnMouseUp()
    {
        PlaySoundOneShot(territoryDeployement);

        // Activate Spawning
        if (!isMouseDragging)
        {
            canSpawn = true; // Can Spawn Troop

            timeSinceMouseUp = 0f; // Reset time since MouseUp
        }

        isMouseDragging = false; // Reset isMouseDragging to false after MouseUp

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
        transform.position = Vector3.Lerp(transform.position, worldMousePosition_, vitesseDeplacement * Time.deltaTime);
        
        // Debug.Log("Position de la souris : " + worldMousePosition);
    }
    void PlaySoundOneShot(AudioClip sound)
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
    void CalculateSpawnRadius()
    {
        // Calculate the spawnRadius depending of the size of the circle
        float circleRadius = lineRenderer.startWidth / 2f;
        spawnRadius = circleRadius;
    }

    void InstantiateTroop(GameObject troop)
    {
        // Get a random point inside the line renderer
        Vector3 randomPoint_ = GetRandomPointOnLine(lineRenderer, Random.Range(innerRadius, 1f - innerRadius));

        // Ajust Position by adding the inner plage
        Vector3 spawnPosition_ = randomPoint_ + (Vector3)(Random.insideUnitCircle * spawnRadius);

        // We will instantiate Troop each time the slider end getting the range of the territory
        Instantiate(troop, spawnPosition_, Quaternion.identity);
    }

    Vector3 GetRandomPointOnLine(LineRenderer line, float t)
    {
        Vector3 lineStart = line.GetPosition(0);
        Vector3 lineEnd = line.GetPosition(line.positionCount - 1);

        float lineWidth = line.startWidth;
        float randomOffset = Random.Range(-lineWidth / 2f, lineWidth / 2f);

        // Adjust t based on inner radius
        float adjustedT = Mathf.Clamp01(t * (1.0f - 2.0f * innerRadius));

        // Get the random point on the adjusted line
        Vector3 randomPointOnLine = Vector3.Lerp(lineStart, lineEnd, adjustedT);

        // Rejection sampling to get a point within the circle
        Vector2 randomPoint;
        do
        {
            float angle = Random.Range(0f, 360f);
            float radius = innerRadius + randomOffset;
            randomPoint = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
        } while (Vector2.Distance(randomPoint, Vector2.zero) > innerRadius);

        return randomPointOnLine + new Vector3(randomPoint.x, randomPoint.y, 0f);
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
}
