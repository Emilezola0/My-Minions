using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    [Header ("<b>Player Parameter</b>")]
    [Space (4)]
    public float vitesseDeplacement;

    [Space(10)]
    [Header("<b>Invoke</b>")]
    [Space(4)]
    public GameObject minions;

    [Space(10)]
    [Header("<b>Sound</b>")]
    [Space(4)]
    public AudioClip takingTerritory;
    public AudioClip territoryDeployement;
    public AudioClip territoryUnlayable;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
          audioSource = GetComponent<AudioSource>();  
    }
    private void OnMouseDown()
    {
        PlaySoundOneShot(takingTerritory);
 
    }

    private void OnMouseUp()
    {
        PlaySoundOneShot(territoryDeployement);
    }
    private void OnMouseDrag()
    {
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
    // Update is called once per frame
    void Update()
    {
     
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

    void InstantiateTroop(GameObject troop)
    {
        // We will instantiate Troop each time the slider end getting the range of the territory
        Instantiate(troop, transform.position, Quaternion.identity);
    }
}
