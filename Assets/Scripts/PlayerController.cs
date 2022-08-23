using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float speed = 4;
    float rotSpeed = 80;
    float gravity = 8;
    float rot = 0f;

    Vector3 moveDir = Vector3.zero;

    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.W)) // Player moves forward
            {
                anim.SetInteger("condition", 1);
                Debug.Log(anim.GetInteger("condition"));
                moveDir = new Vector3(0, 0, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir); // Moves character in direction depending on orientation, AD keybutton
            }
            if (Input.GetKeyUp(KeyCode.W)) // Stops player from moving
            {
                anim.SetInteger("condition", 0);
                moveDir = new Vector3(0, 0, 0);
            }
        }

        //Rotating the character
        rot += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);

        moveDir.y -= gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime);

    }

}
