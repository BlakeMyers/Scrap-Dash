using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        transform.GetComponent<Rigidbody>().WakeUp();
        Invoke("HideBullet", 3f);
    }
    private void OnDisable()
    {
        transform.GetComponent<Rigidbody>().Sleep();
        CancelInvoke();
    }

    void HideBullet()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            enemy.health -= 10f;
            if (enemy.health <= 0)
            {
                Destroy(other.gameObject);
            }
            HideBullet();
        }
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStats>().TakeDamage(10f);
        }

    }
}
