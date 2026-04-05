using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Animator animator;

    private Vector3 moveDirection;

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
        HandleAnimation();
    }

    void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v);

        if (inputDir.magnitude < 0.1f)
        {
            moveDirection = Vector3.zero;
            animator.SetFloat("Moving_BT", 0);
        }
        else
        {
            moveDirection = inputDir.normalized;
            animator.SetFloat("Moving_BT", 1);
        }
    }

    void HandleMovement()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void HandleAnimation()
    {
        animator.SetFloat("Speed", moveDirection.magnitude);
    }
}