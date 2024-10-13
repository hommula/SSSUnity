using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNo2Controller : MonoBehaviour
{
    const float firstGSKDelay = 0.2f;
    const float secondGSKDelay = 0.8f;
    const float knifeSpeed = 10;
    const float dashDistance = 10;
    const float pauseBeforeDash = 1;

    float speed;
    Transform tf;
    Rigidbody2D rb;

    [SerializeField] GameObject knifePrefab, attackHitBoxPrefab;
    [SerializeField] Transform playerTransform;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            StartCoroutine(GroundShootKnife());
        }
        if (Input.GetKeyDown("2"))
        {
            StartCoroutine(AirShootKnife());
        }
        //print("this is tf" + (playerTransform.position - tf.position));

    }

    void ShootKnifeGround()
    {
        GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        knife.GetComponent<Rigidbody2D>().velocity = direction * knifeSpeed;
    }
    void ShootKnifeAir(float angle)
    {
        GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);

        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
        knife.GetComponent<Rigidbody2D>().velocity = direction * knifeSpeed;
    }

    IEnumerator GroundShootKnife()
    {
        for (int i = 0; i < 3; i++)
        {
            ShootKnifeGround();
            yield return new WaitForSeconds(firstGSKDelay);
        }

        yield return new WaitForSeconds(secondGSKDelay);

        for (int i = 0; i < 5; i++)
        {
            ShootKnifeGround();
            yield return new WaitForSeconds(firstGSKDelay);
        }
    }

    void AirKnife()
    {

    }
    IEnumerator AirShootKnife()
    {
        float angleIncrease = 20f;
        float startAngle = Vector3.Angle(tf.right, playerTransform.position - tf.position) -40f;
        for (int i = 0; i < 5; i++)
        {
            float angle = startAngle + i * angleIncrease;
            ShootKnifeAir(angle);
        }
        yield return new WaitForSeconds(.1f);
    }

    void FlashSlash()
    {

    }
    private IEnumerator PerformDownSlash()
    {

        GameObject hitBox = Instantiate(attackHitBoxPrefab, transform.position, Quaternion.identity);

        hitBox.transform.localScale = new Vector3(3, 4, 1);


        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float moveDuration = 3f / speed;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position += (Vector3)direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        Destroy(hitBox);
    }
    private IEnumerator PerformDash()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 20f, LayerMask.GetMask("Wall"));
        float distanceToWall = hit.collider != null ? hit.distance : float.MaxValue;


        if (distanceToWall < dashDistance)
        {

            float offset = dashDistance - distanceToWall;
            Vector2 newPosition = (Vector2)transform.position + Vector2.right * offset;

            transform.position = newPosition;


            yield return new WaitForSeconds(pauseBeforeDash);


            Vector2 dashDirection = (playerTransform.position - transform.position).normalized;
            float elapsedTime = 0f;

            while (elapsedTime < dashDistance / speed)
            {
                transform.position += (Vector3)dashDirection * speed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        void DownSlash()
        {

        }

        IEnumerator PerformHorizontalSlash()
        {

            GameObject hitBox = Instantiate(attackHitBoxPrefab, transform.position, Quaternion.identity);

            hitBox.transform.localScale = new Vector3(5, 4, 1);


            Vector2 direction = (playerTransform.position - transform.position).normalized;
            float moveDuration = 3f / speed;
            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                transform.position += (Vector3)direction * speed * Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            Destroy(hitBox);
        }

        void HorizontalSlash()
        {

        }

        void FrontJumpMove()
        {

        }

        void BackJumpMove()
        {

        }
    }
}
