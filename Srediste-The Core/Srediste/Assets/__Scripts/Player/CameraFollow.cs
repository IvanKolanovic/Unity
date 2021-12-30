using System;
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    
    
    public static CameraFollow Instance;
    private void Awake() {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
        
    }
    
    private void LateUpdate() {
        UpdatePosition(target.position);
    }

    public void UpdatePosition(Vector3 newTargetPos) => transform.position = newTargetPos + offset;

}