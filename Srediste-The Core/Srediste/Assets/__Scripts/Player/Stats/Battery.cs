using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Battery : MonoBehaviour
{
    private const int MIN_BATTERY_LEVEL = 0;
    private const int MAX_BATTERY_LEVEL = 3;
    public static Battery Instance;
    
    [SerializeField] private int batteryLevel = MAX_BATTERY_LEVEL;
    public Light2D batteryLight;

    public int BatteryLevel {
        get => batteryLevel;
        set {
            if (value >= MAX_BATTERY_LEVEL) return;
            
            if (value <= MIN_BATTERY_LEVEL + 1) {
                PlayerController.Instance.Death();
            }
            else {
                batteryLevel = value;
                RefreshLightIntensity();
            }
        }
    }
    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start() => RefreshLightIntensity();
    public void FillBattery() => BatteryLevel++;
    public void UseBattery() => BatteryLevel--;
    public void FillBatteryToMax() => BatteryLevel = MAX_BATTERY_LEVEL;
    private void RefreshLightIntensity() => TweenLightIntensity();

    private void TweenLightIntensity() {
        DOTween.To(() => batteryLight.intensity, 
                x => batteryLight.intensity = x, 
                batteryLevel - 1, 
                1.3f)
                .SetEase(Ease.InOutBounce);
    }
}
