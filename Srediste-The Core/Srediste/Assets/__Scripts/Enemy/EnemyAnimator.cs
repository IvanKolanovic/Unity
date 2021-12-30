using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyAnimator : MonoBehaviour {

    [SerializeField] private float animationDuration = 1.2f;
    [Range(0.5f, 1f)]
    [SerializeField] private float lowEndScale = 0.85f;
    [SerializeField] private Ease easeType = Ease.Linear;
    
    
    IEnumerator Start() {
        float fullSizeY = transform.localScale.y;
        float lowSizeY = fullSizeY * lowEndScale;
        yield return new WaitForSeconds(Random.Range(0, 2f));
        transform.DOScaleY(lowSizeY, animationDuration).SetLoops(-1, LoopType.Yoyo).SetEase(easeType);
    }
}
