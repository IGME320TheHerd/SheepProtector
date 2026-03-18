using UnityEngine;
using UnityEngine.Audio;

public class DogBark : MonoBehaviour
{
    // References
    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private AudioResource barkSound;
    [SerializeField] private ParticleSystem barkVisual;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Bark();
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
            barkVisual.Play();
        }
    }
}
