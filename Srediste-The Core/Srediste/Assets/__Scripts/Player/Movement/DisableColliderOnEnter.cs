using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliderOnEnter : MonoBehaviour
{
    [SerializeField] private Collider2D colliderToDisable;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliderToDisable.enabled = false;
    }
}
