using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    #region CircleParameter

    [Header("<b>Cicle Parameter : </b>")]
    [Space(2)]
    public float circleScale;

    #endregion

    #region GameObject

    [Space(10)]
    [Header("<b>Game Object : </b>")]
    [Space(2)]
    // Ref Of Territory
    public GameObject territory;

    #endregion

    #region UpgradesPublicVar

    [Space(10)]
    [Header("<b>Upgrades : </b>")] 
    [Space(2)]
    // At witch state the territory Cleanse
    public float cleanseLevel;
    public float upgradeStep; // Amount of circleScale increase per unit of gold

    #endregion

    #region Cleanse

    // Add a public array to hold the tags to include in cleansing
    [Space(2)]
    [Header("<b>Cleanse Tags : </b>")]
    [Space(2)]
    public string[] cleanseTags;

    #endregion

    // Level
    [HideInInspector] public float level = 0;

    // Line renderer ref
    private LineRenderer lineRenderer;

    // Start Circle Scale
    private float startCircleScale = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

        circleScale = startCircleScale;

        // Need Line Renderer Attached
        lineRenderer = GetComponent<LineRenderer>();

        // Circle Growth modifying the width of the Line Renderer
        lineRenderer.startWidth = lineRenderer.endWidth = circleScale;
    }

    // Update is called once per frame
    void Update()
    {
        // update territoryTransform
        Vector3 newPosition_ = territory.transform.position;
        newPosition_.z = 0;
        lineRenderer.SetPosition(0, newPosition_);
        lineRenderer.SetPosition(1, newPosition_);
    }

    // Called when a minion give to the territory center point the ressource
    public void UpgradeTerritory(float gold)
    {
        // Increase circleScale based on the amount of gold and upgrade step
        circleScale += gold * upgradeStep;

        // Update the Line Renderer width
        lineRenderer.startWidth = lineRenderer.endWidth = circleScale;

        if (circleScale >= cleanseLevel)
        {
            Cleanse();
        }
    }

    public void Cleanse()
    {
        // Get all colliders inside the circle range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleScale);

        // Iterate through the colliders and filter by tags
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has any of the specified tags
            if (System.Array.Exists(cleanseTags, tag => collider.CompareTag(tag)))
            {
                // This game object has one of the specified tags, so you can do something with it
                Debug.Log("Cleaning: " + collider.gameObject.name);
                // Remove or cleanse the game object based on your logic
                Destroy(collider.gameObject); // You can replace this with whatever action you want
            }
        }

        // Make the circle scale comming back to the starting circle scale
        circleScale = startCircleScale;
        
        // Need Line Renderer Attached
        lineRenderer = GetComponent<LineRenderer>();

        // Circle Growth modifying the width of the Line Renderer
        lineRenderer.startWidth = lineRenderer.endWidth = circleScale;

        level += 1;
    }
}
