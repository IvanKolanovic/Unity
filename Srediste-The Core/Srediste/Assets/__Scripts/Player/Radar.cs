using System.Collections;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [Header("Circle")]
    public bool isGrowing;
    public Transform circleTransform_1;
    public Transform circleTransform_2;

    public Sprite[] spritePhases;
    
    public float startCircleSize = 1f;
    public float endCircleSize = 6f;
    public float circleGrowingTime = 2f;
    public float circleAngle = 20f;

    Camera _camera;
    float interpolation;
    string growingCoroutine;
    Material circleMaterial_1;
    Material circleMaterial_2;
    SpriteRenderer circleSpriterenderer_1;
    SpriteRenderer circleSpriterenderer_2;
    private Vector3 startRadarSizeVector;
    Vector3 endRadarSizeVector;
    float radarAngle;
    Vector3 radarDirection;
    


    private void Start() {
        circleSpriterenderer_1 = circleTransform_1.GetComponent<SpriteRenderer>();
        circleSpriterenderer_2 = circleTransform_2.GetComponent<SpriteRenderer>();
        _camera = Camera.main;
        growingCoroutine = nameof(C_StartGrowingCircle);
        circleMaterial_1 = circleSpriterenderer_1.material;
        circleMaterial_2 = circleSpriterenderer_2.material;
        circleMaterial_1.SetFloat("_Arc1", 180 - circleAngle);
        circleMaterial_1.SetFloat("_Arc2", 180 - circleAngle);
        circleMaterial_2.SetFloat("_Arc1", 180 - circleAngle);
        circleMaterial_2.SetFloat("_Arc2", 180 - circleAngle);
        startRadarSizeVector = Vector3.one * startCircleSize;
        endRadarSizeVector = Vector3.one * endCircleSize;
    }

    public void StartGrowing() => StartCoroutine(growingCoroutine);
    public void StopGrowing() => ResetRadar();
    
    
    private IEnumerator C_StartGrowingCircle() {
        isGrowing = true;
        PlayerController.Instance.isFingerOnPlayer = true;
        //PlaySoundLoad
        float timeElapsed = 0f;
        bool middleSound = false;
        circleSpriterenderer_1.enabled = true;
        circleSpriterenderer_2.enabled = true;
        
        while (PlayerController.Instance.isFingerOnPlayer)
        {
            
            timeElapsed += Time.deltaTime;
            interpolation = timeElapsed / circleGrowingTime;
            if (timeElapsed <= 1f && !middleSound)
            {
                middleSound = true;
                //PlaySoundPrepare
            }
            if (timeElapsed < circleGrowingTime)
                SetRadarTransform(Vector3.Lerp(startRadarSizeVector, endRadarSizeVector, interpolation));
            else
                SetRadarTransform(endRadarSizeVector);

            ChangeMainSprite(interpolation);
            
            yield return null;
            
        }

        yield return null;
    }
    private void SetRadarTransform(Vector3 newsize) {
        circleTransform_1.localScale = newsize;
        circleTransform_2.localScale = newsize;
        circleTransform_1.rotation = CalculateRotation();
        circleTransform_2.rotation = CalculateRotation();
    }
    private Quaternion CalculateRotation() {
        radarDirection = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        radarAngle = Mathf.Atan2(radarDirection.y, radarDirection.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(radarAngle, Vector3.forward);
        
    }
    [ContextMenu("Reset Radar")]
    private void ResetRadar() {
        isGrowing = false;
        PlayerController.Instance.isFingerOnPlayer = false;
        circleSpriterenderer_1.enabled = false;
        circleSpriterenderer_2.enabled = false;
        SetRadarTransform(startRadarSizeVector);
    }

    void ChangeMainSprite(float currInterpolation) {

        float phaseChanger = 1f / spritePhases.Length;
        int newPhaseIndex = Mathf.FloorToInt(currInterpolation / phaseChanger);
        newPhaseIndex = Mathf.Clamp(newPhaseIndex, 0, spritePhases.Length - 1);
        circleSpriterenderer_1.sprite = spritePhases[newPhaseIndex];

    }
}
