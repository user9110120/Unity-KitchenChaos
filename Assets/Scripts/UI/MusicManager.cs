using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private const string PLAYER_EFFRCTS_MUSIC_VOLUMN = "MusicVolumn";
    public static MusicManager Instance { get; private set; }



    private AudioSource audioSource;
    private float volumn = .3f;


    private void Awake()
    {

        Instance = this;

        audioSource = GetComponent<AudioSource>();

        volumn = PlayerPrefs.GetFloat(PLAYER_EFFRCTS_MUSIC_VOLUMN, .3f);
        audioSource.volume = volumn;
    }
    public void ChangeVolumn()
    {
        volumn += .1f;
        if (volumn > 1f)
        {
            volumn = 0f;
        }
        audioSource.volume = volumn;

        PlayerPrefs.SetFloat(PLAYER_EFFRCTS_MUSIC_VOLUMN, volumn);
        PlayerPrefs.Save();
    }

    public float GetVolumn()
    {
        return volumn;
    }
}
