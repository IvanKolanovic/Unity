using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Image fadeImage;


    private GameObject player;
    
    
    private void Awake() {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start() {
        player = PlayerController.Instance.gameObject;
        //FadeIn(2f);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuPanel.activeInHierarchy) HideMenu();
            else ShowMenu();
        }
    }

    #region FADER

    public void FadeIn(float fadeTime = 2f) => StartCoroutine(FadeInOut(true, fadeTime));
    public void FadeOut(float fadeTime = 2f) => StartCoroutine(FadeInOut(false, fadeTime));
    public IEnumerator FadeInOut(bool fadeIn, float fadeTime) {
        float timeElapsed = 0f;
        Color oldColor = fadeIn ? Color.black : Color.clear;
        Color newColor = fadeIn ? Color.clear : Color.black; 

        while (timeElapsed < fadeTime) {
            timeElapsed += Time.deltaTime;
            fadeImage.color = Color.Lerp(oldColor, newColor, timeElapsed / fadeTime);
            yield return null;
        }
        
        fadeImage.color = newColor;

    }
    
    #endregion
    
    public void NewGame() {
        FadeIn(2f);
        HideMenu();
        PlayerController.Instance.spriteMaterial.SetFloat("_Dissolve", 0f);
    }
    //public void LoadGame() =>  player.GetComponent<PlayerController>().SetLoadStatePosition();
    public void ExitGame() => Application.Quit();
    public void ShowMenu() {
        PauseTime();
        menuPanel.SetActive(true);
    }
    public void HideMenu() {
        UnpauseTime();
        menuPanel.SetActive(false);
    }

    public void DeathPanel() {
        deathPanel.SetActive(true);
        PauseTime();
    }
    
    public void PauseTime() => Time.timeScale = 0f;
    public void UnpauseTime() => Time.timeScale = 1f;
    
    
    
    /*
    #region UI_UPDATES

    public void SetCentral(string newText) {
            central.text = String.Concat("> ", newText);
        }
    public void SetBatteryLevel(int batteryAmount) {
        StringBuilder sb = new StringBuilder(2 * batteryAmount);
        for(int i = 0; i < batteryAmount - 1; i++) sb.Append("0 ");
        sb.Append("0");
        batteryLevel.text = sb.ToString();
    }
    
    public void SetLatLong(float latitude, float longitude) {
        latLong.text = latitude + latitude > 0 ? "N\n" : "S\n";
        latLong.text = longitude + longitude > 0f ? "E" : "W";
    }
    public void SetDepth(float newDepth) {
        depth.text = newDepth + " m";
    }
    

    #endregion
    */

}
