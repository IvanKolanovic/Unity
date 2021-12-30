using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    private void Awake() {
        
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource Footsteps_AudioSource;
    [SerializeField] private bool canPlayOnFXaudioSource = true;
    [SerializeField] private AudioSource FX_AudioSource;
    
    [Header("Other")]
    [SerializeField] private AudioClip central;
    [SerializeField] private List<AudioClip> footSteps;
    [SerializeField] private int footStepIndex = 0;

    [Header("Monsters")]
    [SerializeField] private AudioClip monsterAttack;
    [SerializeField] private AudioClip A;
    [SerializeField] private AudioClip B;
    [SerializeField] private AudioClip C;
    [SerializeField] private AudioClip D;
    [SerializeField] private AudioClip E;
    [SerializeField] private AudioClip F;
    [SerializeField] private AudioClip G;
    
    public void PlayCentral() => FX_AudioSource.PlayOneShot(central);
    public void PlayMonstersSounds(AudioClip[] sounds) {
        if (canPlayOnFXaudioSource) {
            float longestClipTime = 0f;
            for (int i = 0; i < sounds.Length; i++) {
                FX_AudioSource.PlayOneShot(sounds[i]);
                if (sounds[i].length > longestClipTime)
                {
                    longestClipTime = sounds[i].length;
                }
            }
            DisableAudioSourcePlayFor(FX_AudioSource, longestClipTime);
        }   
    }
    public void PlayMonsterAttack() => FX_AudioSource.PlayOneShot(monsterAttack);
    public void PlayFootStepSound() {
        Footsteps_AudioSource.PlayOneShot(footSteps[footStepIndex]);
        footStepIndex++;
        if (footStepIndex >= footSteps.Count) 
            footStepIndex = 0;
    }
    private void DisableAudioSourcePlayFor(AudioSource audioSource, float time) => StartCoroutine(C_DisableAudioSourcePlayFor(audioSource, time, 0.25f));
    IEnumerator C_DisableAudioSourcePlayFor(AudioSource audioSource, float time, float tailPadding) {
        if (audioSource == FX_AudioSource)
        {
            canPlayOnFXaudioSource = false;
            yield return new WaitForSeconds(time + tailPadding);
            audioSource.volume = 1;
            canPlayOnFXaudioSource = true;
        }
        yield return null;
    }
    
}
