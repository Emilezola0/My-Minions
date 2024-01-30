using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public float circleScale;
    private LineRenderer lineRenderer;

    // Ref Of Territory
    public GameObject territory;

    // Start is called before the first frame update
    void Start()
    {
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
}
