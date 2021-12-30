using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    public bool canMove = true;
    [SerializeField] private Animator anim;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 newVelocity = Vector3.zero;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool facingRight = true;

    [SerializeField] private float inputY;

    //[SerializeField] private Collider2D coll2D;
    //[SerializeField] private Vector2 groundPoint;
    //[SerializeField] private RaycastHit2D hit2D;
    public void ChangeMoveSpeed(int newMoveSpeed) => moveSpeed = newMoveSpeed;
    public float InputY => inputY;

    //testtest
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundMinPos;
    private Vector2 currentGravity = Vector2.down;
    private Vector2 groundNormal = Vector2.up;
    private ContactPoint2D[] cPoints;
    private float maxGroundedAngle = 75f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        rb.gravityScale = 0f;
    }

    public void MovePlayer(Vector2 input)
    {
        if (!canMove) return;
        StartWalking();
        HandleInput(input);
        FlipCharacter(input.x > 0);
    }


    private void HandleInput(Vector2 input)
    {
        newVelocity.x = input.x * moveSpeed;
        SetWalkSpeed(newVelocity.x / moveSpeed);
        newVelocity.y = rb.velocity.y;
        inputY = input.y;
        rb.velocity = newVelocity;
    }

    private void FlipCharacter(bool rightDirection)
    {
        if (facingRight == rightDirection) return;

        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    public void StartWalking() {
        rb.simulated = true;
        anim.SetBool("Walking", true);   
    }

    public void StopWalking() {
        rb.simulated = false;
        anim.SetBool("Walking", false); 
    } 

    public void SetWalkSpeed(float speed) => anim.SetFloat("MoveSpeed", speed);

    void OnCollisionStay2D(Collision2D ourCollision) => isGrounded = CheckGrounded(ourCollision);

    void OnCollisionExit2D(Collision2D ourCollision) => isGrounded = false;

    bool CheckGrounded(Collision2D newCol)
    {
        cPoints = new ContactPoint2D[newCol.contactCount];
        newCol.GetContacts(cPoints);
        foreach (ContactPoint2D cP in cPoints)
        {
            if (maxGroundedAngle > Vector2.Angle(cP.normal, -Physics2D.gravity.normalized))
            {
                groundNormal = cP.normal;
                return true;
            }
        }
        return false;
    }

    void ObeyGravity()
    {

        if (!isGrounded)
        {
            currentGravity = Physics2D.gravity;
            rb.gravityScale = 3f;
            
        }
        else
        {
            currentGravity = -groundNormal * Physics2D.gravity.magnitude;
            rb.gravityScale = 0f;
            Ray2D ray = new Ray2D(transform.position, Vector2.down);
            float dist = Vector2.Distance(transform.position, groundMinPos.position);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, dist);
            if (hit)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red, 0.5f);
                isGrounded = false;
            }

            Debug.DrawLine(transform.position, groundMinPos.position, Color.green, 0.5f);
        }
        rb.AddForce(currentGravity, ForceMode2D.Force);
    }

    void FixedUpdate() => ObeyGravity();

    public void DisableAnimator() => GetComponent<Animator>().enabled = false;
    public void EnableAnimator() => GetComponent<Animator>().enabled = true;
}