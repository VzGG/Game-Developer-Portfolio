using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] gameBackgroundSounds;


    private void Awake()
    {
        // Singleton - only one music manager is alive in the game
        int numberOfMusicManager = FindObjectsOfType<MusicManager>().Length;
        if (numberOfMusicManager > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(gameBackgroundSounds[0]);
        audioSource.loop = true;
    }

    public void PlayMusic(int musicIndex)
    {

        // Fade the sound by reducing the volume level, then stop the music
        // Then fade in and play the music
        StartCoroutine(FadeOutMusic(musicIndex));

    }

    IEnumerator FadeOutMusic(int musicIndex)
    {

    
        // Fade out Music
        while (audioSource.volume > 0)
        {
            yield return new WaitForSeconds(0.1f);

            // Then run the code below
            audioSource.volume -= 0.05f;
            //Debug.Log("FadeOutMUSIC!!!");
        }

        // Debug.Log("Hello");
        // Debug.Log("Isplaying music: " + audioSource.isPlaying);
        audioSource.Stop();

        // Fade in music
        audioSource.clip = gameBackgroundSounds[musicIndex];
        audioSource.Play();
        //audioSource.PlayDelayed(1f);
        while (audioSource.volume < 0.25)
        {
            yield return new WaitForSeconds(0.1f);

            audioSource.volume += 0.05f;
           // Debug.Log("Fade In Music");
        }

        

    }

    
}
