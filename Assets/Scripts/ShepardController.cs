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
        //float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(positionOnScreen.y - mousePos.y, positionOnScreen.x - mousePos.x) * Mathf.Rad2Deg;
        //transform.Rotate(0, angle * rotationSpeed * Time.deltaTime, 0);
        //transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance = 0.0f;
        if(playerPlane.Raycast(ray, out hitDistance))
        {
            Vector3 targetPoint = ray.GetPoint(hitDistance);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
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
