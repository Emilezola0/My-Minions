using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDTimer : MonoBehaviour
{
    private float elapsedTime; // Elapsed time since the player started

    public TMP_Text chronometerText; // Reference to the TextMeshPro component for displaying the chronometer

    void Update()
    {
        // Update the chronometer
        elapsedTime += Time.deltaTime;
        UpdateChronometerDisplay();
    }

    void UpdateChronometerDisplay()
    {
        // Update the TextMeshPro component with the elapsed time formatted as minutes:seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        string chronometerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        chronometerText.text = chronometerString;
    }
}
