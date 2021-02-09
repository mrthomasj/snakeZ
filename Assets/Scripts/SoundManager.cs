using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip death;
    static AudioSource audioSrc;

    private void Start()
    {
        death = Resources.Load<AudioClip>("Morri");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "death":
                audioSrc.PlayOneShot(death);
                break;
        }
    }
}
