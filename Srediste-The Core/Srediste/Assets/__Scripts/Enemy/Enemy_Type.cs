using UnityEngine;

[CreateAssetMenu(fileName = "Enemy_1", menuName = "Enemies/Enemy", order = 0)]
public class Enemy_Type : ScriptableObject {
   public AudioClip sound;
   /// <summary>
   /// A => 0
   /// B => 0
   /// C => -9
   /// D =>  -1
   /// E => +1
   /// F => +3
   /// G => 0
   /// </summary>
   [Range(-1, 2)] 
   public int strength;
}
