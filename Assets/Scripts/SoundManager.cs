using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{


    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUMN = "SoundEffectsVolumn";
    public static SoundManager Instance { get; private set; }


    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volumn = 1f;

    private void Awake()
    {
        Instance = this;

        volumn = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUMN, 1f);
    }


    private void Start()
    {
        DeliveryManeger.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManeger.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }
    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }
    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }
    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {

        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }
    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliverCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliverCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliverCounter.transform.position);

    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volumn);
    } 

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footStep, position, volume);  
    }
    public void PlayCountDownSound()
    {
        PlaySound(audioClipRefsSO.warning, Vector3.zero);  
    }
    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warning, position);  
    }

    public void ChangeVolumn()
    {
        volumn += .1f;
        if (volumn > 1f)
        {
            volumn = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUMN, volumn);
        PlayerPrefs.Save();
    }

    public float GetVolumn()
    {
        return volumn;
    }
}
