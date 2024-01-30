using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public float speedGrowth = 1f;
    private LineRenderer lineRenderer;

    // Ref Of Territory
    public GameObject transformTerritory;

    // Start is called before the first frame update
    void Start()
    {
        // Need Line Renderer Attached
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Circle Growth modifying the width of the Line Renderer
        float nouvelleLargeur = lineRenderer.startWidth + (speedGrowth * Time.deltaTime);
        lineRenderer.startWidth = lineRenderer.endWidth = nouvelleLargeur;

        // update territoryTransform
        transform.position = transformTerritory.transform.position;
    }
}
