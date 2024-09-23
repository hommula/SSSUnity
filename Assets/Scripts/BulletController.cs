using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifetime = 10f;

    Vector3 direction = Vector3.zero;
    Vector3 targetPos;
    Transform tf;
    Rigidbody2D rb;
    [SerializeField] float bulletSpeed;

   
    public ParticleSystem bulletExplosionEffect;

    private bool isFlying = true;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        direction = (targetPos - transform.position).normalized;
        Destroy(gameObject, lifetime);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tf.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }

    // Update is called once per frame
    void Update()
    {
        Accelerate();
    }

    private void Accelerate()
    {
        rb.velocity = direction * bulletSpeed;


    }
    public void SetTarget(Vector2 target)
    {
        targetPos = target;
    }

 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Destroy(gameObject);
        }
    }
}
