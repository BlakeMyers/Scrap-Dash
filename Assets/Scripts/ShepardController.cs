using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepardController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float rotationSpeed = 250.0f;
    private float gravity = 20.0f;
    private CharacterController characterController;
    public PlayerStats stats;
    private Vector3 movementDirection;
    [SerializeField]
    private float jump;
    public Rigidbody rigidBody;
    public Collider pickUpCollider;
    public bool isWeaponEquiped = false;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        if(!this.photonView.IsMine)
        {
            GetComponent<CharacterController>().enabled = false;
            this.enabled = false;
            return;
        }
        movementSpeed = stats.speed;
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
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        if(groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            //characterController.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (characterController.isGrounded)
        {
            movementDirection = (Vector3.forward * vertical) + (Vector3.right* horizontal);
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
        weapon.GetComponent<GunController>().GunDropped();
        isWeaponEquiped = false;
    }
    public void EquipWeapon(Transform weaponTransform)
    {
        weaponTransform.rotation = transform.rotation;
        weaponTransform.SetParent(transform.GetChild(0));
        weaponTransform.position = transform.GetChild(0).position;
        weaponTransform.GetComponent<Rigidbody>().isKinematic = true;
        weaponTransform.GetComponent<GunController>().GunEquipped();
        isWeaponEquiped = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log("Weapon Detected");
            if (isWeaponEquiped == false)
            {
                other.gameObject.GetComponent<PhotonView>().RequestOwnership();
                EquipWeapon(other.GetComponentInParent<Transform>());
            }
        }
        if (other.tag == "Bullet")
        {
            this.GetComponent<PlayerStats>().TakeDamage(10f);
        }
        if (other.tag == "Enemy")
        {
            this.GetComponent<PlayerStats>().TakeDamage(20f);
        }
    }
}
