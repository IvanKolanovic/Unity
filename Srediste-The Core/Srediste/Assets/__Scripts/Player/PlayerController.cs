using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    public bool isFingerOnPlayer;
    [SerializeField] private Battery battery;
    [SerializeField] private PlayerMovement playerMovement;
    private float spriteDissolve;
    public Material spriteMaterial;
    private Vector3 lastPosition;
    
    
    
    public static PlayerController Instance;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
    }

    private void Start() {
        lastPosition = transform.position;
    }

    public void Death() => StartCoroutine(C_Death());

    IEnumerator C_Death() {
        //SoundManager.Instance.PlayMonsterAttack();
        playerMovement.StopWalking();
        playerMovement.canMove = false;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(C_DeathSpriteChange());
        yield return new WaitForSeconds(0.3f);
        transform.position = lastPosition;
        playerMovement.canMove = true;
        battery.FillBatteryToMax();
        UI_Manager.Instance.DeathPanel();
    }

    public IEnumerator C_DeathSpriteChange()
    {
        float timeElapsed = 0f;
        float timeToDie = 1f;
        float startvalue = 0f;
        float endSpriteDissolveValue = 0.6f;
        bool fadingStarted = false;
        while (timeElapsed < timeToDie)
        {
            spriteDissolve = Mathf.Lerp(startvalue, endSpriteDissolveValue, timeElapsed / timeToDie);
            spriteMaterial.SetFloat("_Dissolve", spriteDissolve);
            timeElapsed += Time.deltaTime;
            if (timeElapsed / timeToDie >= 0.7f && !fadingStarted)
            {
                UI_Manager.Instance.FadeOut(0.3f);
                fadingStarted = true;
            }
            yield return null;
        }
        spriteMaterial.SetFloat("_Dissolve", endSpriteDissolveValue);
    }
    //public void SetHeartRate(int heartRate) => heartbeat.HeartRate = heartRate;
   
    
    /* SaveLoadSystem
    public void SetLoadStatePosition() => transform.position = LoadPosition();
      
     private void OnApplicationQuit() {
        SavePosition(transform.position);
    }

    public void SavePosition(Vector3 pos) {
        PlayerPrefs.SetFloat("posX", pos.x);
        PlayerPrefs.SetFloat("posY", pos.y);
        PlayerPrefs.SetFloat("posZ", pos.z);
    }

    private Vector3 LoadPosition() {
        if (PlayerPrefs.HasKey("posX")) {
            Vector3 newPos;
            newPos.x = PlayerPrefs.GetFloat("posX");
            newPos.y = PlayerPrefs.GetFloat("posY");
            newPos.z = PlayerPrefs.GetFloat("posZ");
            return newPos;
        }
        //Return startPoint
        return new Vector3(-34.24f, -3.9f, 0f);
    }
     */
    
}
