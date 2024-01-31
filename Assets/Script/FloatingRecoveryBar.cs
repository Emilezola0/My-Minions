using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingRecoveryBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cameraRef;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private Renderer barRenderer;

    public void UpdateRecoveryBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = cameraRef.transform.rotation;
        transform.position = target.position + offset;
    }
}
