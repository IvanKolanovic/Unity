using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DirectionChanger : MonoBehaviour
{
    private enum DirectionCollider
    {
        Ignore,
        Ignore2,
        Upper,
        Center,
        Lower
    }

    [SerializeField] private EdgeCollider2D ignore;
    [SerializeField] private EdgeCollider2D ignore2;
    [SerializeField] private EdgeCollider2D upper;
    [SerializeField] private EdgeCollider2D center;
    [SerializeField] private EdgeCollider2D lower;


    private PlayerMovement playerMovement;
    private float yThreshold = 0.2f;
    private DirectionCollider directionCollider = DirectionCollider.Upper;
    private EdgeCollider2D[] colliders;

    private void Awake()
    {
        colliders = new EdgeCollider2D[5] {ignore,ignore2, upper, center, lower};
        if (colliders[3] == null) yThreshold = 0f;
    }

    private void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovement.InputY >= yThreshold)
                ChangeDirection(DirectionCollider.Upper);
            else if (Mathf.Abs(playerMovement.InputY) <= yThreshold)
                ChangeDirection(DirectionCollider.Center);
            else
                ChangeDirection(DirectionCollider.Lower);
        }
    }

    private void ActivateCollider(int index)
    {   
       // Debug.Log(index + " "+ colliders[0]);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (i >= index && colliders[i] != null)
                colliders[i].enabled = true;
            else if (colliders[i] != null)
                colliders[i].enabled = false;
        }
    }

    private void ActivateUpperCollider() => ActivateCollider(2);
    private void ActivateMiddleCollider() => ActivateCollider(3);
    private void ActivateLowerCollider() => ActivateCollider(4);

    private void ChangeDirection(DirectionCollider newDirectionCollider)
    {
        if (directionCollider == newDirectionCollider) return;
        //Debug.Log("Changing direction to: " + newDirectionCollider);
        switch (newDirectionCollider)
        {
            case DirectionCollider.Upper:
                ActivateUpperCollider();
                break;
            case DirectionCollider.Center:
                ActivateMiddleCollider();
                break;
            case DirectionCollider.Lower:
                ActivateLowerCollider();
                break;
        }

        directionCollider = newDirectionCollider;
    }
}