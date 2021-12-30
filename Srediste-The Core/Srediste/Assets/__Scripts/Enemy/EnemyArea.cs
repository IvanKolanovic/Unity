using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyArea : MonoBehaviour {
    
    [SerializeField] private Enemy_Type[] enemyTypes;
    
    public AudioClip[] GetSounds() => enemyTypes.Select(enemy => enemy.sound).ToArray();
    public int CalculateStrength() => enemyTypes.Sum(enemy => enemy.strength);


    private void OnValidate() {
        if (enemyTypes.Length == 0) {
            Debug.LogError("Enemy types not set in inspector!", this);
            gameObject.name = "Empty EnemyArea";
            return;
        }

        if (IsAnyElementNull()) {
            Debug.LogError("Some enemy from array is not set", this);
            gameObject.name = "Some enemy null";
            return;
        }

        string newName = "";
            for (int i = 0; i < enemyTypes.Length; i++) {
                
                if (i == enemyTypes.Length - 1) newName = string.Concat(newName, enemyTypes[i].sound.name);
                else newName = string.Concat(newName, enemyTypes[i].sound.name, "+");
            }
            
            gameObject.name = newName;
        }

    private bool IsAnyElementNull() {
        for (int i = 0; i < enemyTypes.Length; i++) {
            if (enemyTypes[i] == null) return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            int enemyStrength = CalculateStrength();

            if (enemyStrength <= -5) {
                Debug.Log("Nothing");
            }
            else if (enemyStrength <= -1) {
                    Debug.Log("Battery Filled!");
                    Battery.Instance.FillBattery();
            }
            else if (enemyStrength == 0) {
                Debug.Log("Nothing");
            }
            else if (enemyStrength == 1) {
                Debug.Log("Battery Drained!");
                Battery.Instance.UseBattery();
            }
            else {
                Debug.Log("Player Died!");
                PlayerController.Instance.Death();
            }
            gameObject.SetActive(false);
        }
    }
}
