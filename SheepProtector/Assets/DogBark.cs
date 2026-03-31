using UnityEngine;
using UnityEngine.Audio;

public class DogBark : MonoBehaviour
{
    // References
    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private AudioResource barkSound;
    [SerializeField] private SpriteRenderer barkVisual;

    private float barkVisTimer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Bark"))
        {
            Bark();
        }

        if (barkVisTimer > 0.0f)
        {
            barkVisual.enabled = true;
            barkVisTimer -= Time.deltaTime;
        } else
        {
            barkVisual.enabled = false;
        }
    }

    private void Bark()
    {
        // Play sound 
        if (myAudioSource != null && barkSound != null)
        {
            myAudioSource.resource = barkSound;
            myAudioSource.Play();
        }

        // Show visual
        if (barkVisual != null)
        {
            barkVisTimer = 0.5f;
        }
    }
}
