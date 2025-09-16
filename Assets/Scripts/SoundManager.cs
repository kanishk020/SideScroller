using UnityEngine;
using System;
using UnityEngine.UI;

[System.Serializable] 
public class Sound // Serializable class to hold audio info
{
    public string name;     // Identifier for the sound
    public AudioClip clip;  // The actual audio file
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // Singleton instance for global access

    private AudioSource source; // sound source to play 
    
    [SerializeField] private Sound[] sfxSounds;  // List of available sound effects

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }

        
        source = gameObject.GetComponent<AudioSource>();
    }


    
    /// <summary>
    /// public function to play sounds on basis of name string
    /// </summary>
    /// <param name="name"></param>
    
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, sound => sound.name == name);
        source.PlayOneShot(s.clip); // Plays without interrupting other SFX
    }

    
}
