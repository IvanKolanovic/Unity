using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaIndicator : MonoBehaviour {
    public EnemyArea enemyArea;

    private void Awake() {
        if (enemyArea == null) Debug.LogError("Enemy area not set in inspector!", this);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SoundManager.Instance.PlayMonstersSounds(enemyArea.GetSounds());
        }
    }
}
