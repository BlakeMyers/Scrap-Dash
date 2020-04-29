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
    public Collider pickUpCollider;
    public bool isWeaponEquiped = false;
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropWeapon();
        }
        float vertical = Input.GetAxis("Vertical");
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(positionOnScreen.y - mousePos.y, positionOnScreen.x - mousePos.x) * Mathf.Rad2Deg;

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
            movementDirection = Vector3.forward * vertical;
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection *= movementSpeed;
        }

        movementDirection.y -= gravity * Time.deltaTime;
        characterController.Move(movementDirection * Time.deltaTime);
    }
    public void DropWeapon()
    {
        pickUpCollider.isTrigger = false;
        Transform weapon = transform.GetChild(0).GetChild(0);
        weapon.SetParent(null);
        Rigidbody weaponRB = weapon.GetComponent<Rigidbody>();
        weaponRB.isKinematic = false;
        weaponRB.AddForce(transform.right*150);
        pickUpCollider.isTrigger = true;
        isWeaponEquiped = false;
    }
    public void EquipWeapon(Transform weaponTransform)
    {
        weaponTransform.rotation = transform.rotation;
        weaponTransform.SetParent(transform.GetChild(0));
        weaponTransform.position = transform.GetChild(0).position;
        weaponTransform.GetComponent<Rigidbody>().isKinematic = true;
        isWeaponEquiped = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log("Weapon Detected");
            if (isWeaponEquiped == false)
            {
                EquipWeapon(other.GetComponentInParent<Transform>());
            }
        }
    }
}
