using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{

    public static Transform player;
    public float fov = 130;
    public float visibilityDistance = 5;

    public float intelligenceMod = 1;

    public Vector3 eyeOffset = new Vector3(0, 0, 0);

    public float Acceleration = 10;
    public float maxSpeed = 20;


    public float MaxMemory = 100f;
    public float memory = 0f;
    public float threshold = 75f;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        rigid = this.GetComponent<Rigidbody>();
        rigid.constraints = RigidbodyConstraints.FreezeRotation;

    }

    // Update is called once per frame
    void Update(){
        if (IsVisible())
            memory += intelligenceMod * Time.deltaTime;

        memory = Mathf.Clamp(memory, 0, MaxMemory);

        if (memory > threshold)
        {
            MoveToPlayer();
        }
        else {
            Idle();
        }

        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);

    }

    bool IsVisible() {

        float visibility = 0;
        RaycastHit hit;
        Vector3 direction = this.transform.forward + eyeOffset + (this.transform.position - player.transform.position).normalized;
        if (Vector3.SignedAngle(this.transform.forward, direction, Vector3.up) > fov) {
            return false;
        }

        Ray ray = new Ray(this.transform.forward + eyeOffset, direction);
        if (Physics.Raycast(ray, out hit, visibilityDistance))
        {
            return true;
        }
        else {
            return false;
        }

        return false;
    }

    void MoveToPlayer() {
        Vector3 moveDir = Vector3.zero;
        RaycastHit hit;
        Vector3 direction = this.transform.forward + eyeOffset + (this.transform.position - player.transform.position).normalized;

        moveDir += direction;
        
        Ray ray = new Ray(this.transform.forward + eyeOffset, direction);
        if (Physics.Raycast(ray, out hit, visibilityDistance)){
            //Uh oh there is something in front of me

            if(hit.transform.tag != "Player")
                moveDir += (Vector3.Reflect(direction, hit.normal) * hit.distance).normalized;
        }

        rigid.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
        rigid.velocity += moveDir.normalized * Acceleration;
    }

    void Idle() {
        Vector3 direction = new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1));
        rigid.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));

        rigid.velocity += direction * Acceleration;
    }

    private void OnDrawGizmos(){
        Matrix4x4 temp = Gizmos.matrix;
        Gizmos.color = Color.HSVToRGB(.5f - (memory / MaxMemory) * .5f, 1, 1);
        
        Gizmos.matrix = Matrix4x4.TRS(this.transform.forward + eyeOffset + this.transform.position, this.transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, visibilityDistance, 0, 1);
        Gizmos.matrix = temp;
    }

}
