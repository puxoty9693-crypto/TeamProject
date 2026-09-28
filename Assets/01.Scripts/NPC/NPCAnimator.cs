using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform root;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private bool isFlipX;

    private Vector3 lastPosition;

    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (root == null) root = transform.parent;
        lastPosition = root.position;

    }

    private void Update()
    {
        Vector3 lastDir = root.position - lastPosition;

        bool isMoving = lastDir.sqrMagnitude > 0.000001f;

        animator.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
            Vector2 dir = new Vector2(lastDir.x, lastDir.y).normalized;
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);

            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                isFlipX = dir.x > 0f;

            }
            spriteRenderer.flipX = false;
            
        }
        else
        {
            spriteRenderer.flipX = isFlipX;
        }
        lastPosition = root.position;

    }

    public void SetIdleFacing(bool facing)
    {
        isFlipX = facing;
        spriteRenderer.flipX = facing;
    }
}
