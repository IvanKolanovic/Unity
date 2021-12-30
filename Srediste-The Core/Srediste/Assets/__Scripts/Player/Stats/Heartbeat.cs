using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Heartbeat : MonoBehaviour
{
    const int MIN_HEART_RATE = 40;
    const int MAX_HEART_RATE = 130;
    

    public float HeartRate
    {
        set => heartRate = Mathf.Clamp(value, MIN_HEART_RATE, MAX_HEART_RATE);
    }
    [Range(MIN_HEART_RATE, MAX_HEART_RATE)]
    public float heartRate = 40;
    
    private float beatTime = 0.5f;
    private float amplitudeScale;
    private float upperAplitude;
    private float BPM => 60f / heartRate;
    private float amplitude = 0f;
    private int positionCount = 200;
    private LineRenderer lineRenderer;
    private float timeElapsed = 0f;
    private Vector3[] points;
    private AudioSource audioSource;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        
        lineRenderer.hideFlags = HideFlags.HideInInspector;
    }

    private void Start() {
        Initialize();
    }

    private void Initialize() {
        
        points = new Vector3[positionCount];
        float startingPoint = -2f;
        float step = (Mathf.Abs(startingPoint) * 2) / 200f;
        for (int i = 0; i < points.Length; i++)
        {
            points[i].x = startingPoint + (i * step);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        
    }

    private void FixedUpdate() {
        timeElapsed += Time.fixedDeltaTime;
        if (timeElapsed >= BPM)
        {
            Beat();
            timeElapsed = 0f;
        }
        
        MovePointsToTheLeft();


    }

    void MovePointsToTheLeft() {
        points[0].y = points[1].y;
        for (int i = 1; i < points.Length - 1; i++)
        {
            points[i].y = points[i + 1].y;
        }

        points[points.Length - 1].y = amplitude;
        lineRenderer.SetPositions(points);
    }

    public void Beat() {
        StartCoroutine(BeatAmplitude());
    }

    
    IEnumerator BeatAmplitude() {
        float timeElapsed = 0f;
        upperAplitude = UnityEngine.Random.Range(1.47f, 1.53f);
        amplitudeScale = 5 + ((heartRate / 130) * 5);
        //PlaySound
        float x = timeElapsed / beatTime;
        while (timeElapsed < beatTime)
        {
            timeElapsed += Time.deltaTime;
            //Debug.Log("Coroutine running");
            x = 1f + (timeElapsed / beatTime);
            amplitude = Mathf.Pow(Mathf.Sin(x), 63) * Mathf.Sin(x + upperAplitude) * amplitudeScale;
            yield return null;
        }

        amplitude = 0f;
    }
}
