using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    private float currentSpeed = 0f;
    private float speedSmoothVelocity = 0f;
    private float speedSmoothTime = 0.1f;
    private float rotationSpeed = 0.07f;
    private float gravity = 3f;

    public bool toggle = true;

    private Transform mainCameraTransform = null;

    private CharacterController controller = null;
    public Animator anim;

    public Vector3 gameStartVector;
    public PlayerPosVector startingPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        mainCameraTransform = Camera.main.transform;
        transform.position = gameStartVector;

        if (startingPosition.sceneTransitions > 0)
        {
            transform.position = startingPosition.initialValue;
        }
    }

    void Update()
    {
        if (toggle)
        {
            Move();
        }
    }

    private void Move()
    {
        // Vector2 holds x and y values; Vector3 holds x, y, z values.
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 forward = mainCameraTransform.forward;
        Vector3 right = mainCameraTransform.right;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = (forward * movementInput.y + right * movementInput.x).normalized;
        Vector3 gravityVector = Vector3.zero;

        if(!controller.isGrounded)
        {
            gravityVector.y -= gravity; //lower the character if airborn to the ground
        }

        if (desiredMoveDirection != Vector3.zero)
        {
            //Rotates character to face direction.
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = 6f;
                anim.SetInteger("condition", 2);
            }
            else
            {
                anim.SetInteger("condition", 1);
                movementSpeed = 2f;
            }
        }
        else
        {
            anim.SetInteger("condition", 0);
            movementSpeed = 2f;
        }

        float targetSpeed = movementSpeed * movementInput.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        controller.Move(desiredMoveDirection * currentSpeed * Time.deltaTime);

        controller.Move(gravityVector * Time.deltaTime);

    }
}
