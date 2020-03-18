using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepardController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float rotationSpeed = 250.0f;
    private float gravity = 20.0f;
    private CharacterController characterController;
    private Vector3 movementDirection;
    [SerializeField]
    private float jump;
    public Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 250.0f;
        characterController = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        jump = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed *= 2.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed /= 2.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (characterController.isGrounded)
            {
                Debug.Log("Space");
                //rigidBody.AddForce(new Vector3(0, jump, 0));
                movementDirection = new Vector3(0, jump, 0);
                characterController.Move(movementDirection);
            }
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Rotate(0, horizontal * rotationSpeed * Time.deltaTime, 0);
        if (characterController.isGrounded)
        {
            //bool move = (vertical > 0) || (horizontal != 0);
            movementDirection = Vector3.forward * vertical;
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection *= movementSpeed;
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);
    }
        
}
