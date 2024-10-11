using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNo2Controller : MonoBehaviour
{
    const float firstGSKDelay = 0.2f;
    const float secondGSKDelay = 0.8f;
    const float knifeSpeed = 10;

    float speed;
    Transform tf;
    Rigidbody2D rb;

    [SerializeField] GameObject knifePrefab;

    [SerializeField] Transform playerTransform;
   
    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

    }


    void ShootKnifeGround()
    {
        GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        Vector2 direction = (playerTransform.position - transform.position).normalized; // 計算朝向玩家的方向
        knife.GetComponent<Rigidbody2D>().velocity = direction * knifeSpeed; // 設置飛刀速度
    }
    void ShootKnife(float angle)
    {
        GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up; 
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

        for (int i = 0;i < 5; i++)
        {
            ShootKnifeGround();
            yield return new WaitForSeconds(firstGSKDelay);
        }
    }

    void AirShootKnife()
    {
        float angleIncrease = 20f;
        float startAngle = -40f;
        for (int i = 0; i < 5; i++)
        {
            float angle = startAngle + i * angleIncrease; 
            ShootKnife(angle);
        }
    }

    void FlashSlash()
    {

    }
    private IEnumerator PerformDownSlash()
    {
        
        GameObject hitBox = Instantiate(attackHitBoxPrefab, transform.position, Quaternion.identity);
        
        hitBox.transform.localScale = new Vector3(3, 4, 1); 

       
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float moveDuration = 3f / speed; // 向玩家方向移動3格
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

            
            Vector2 dashDirection = (player.position - transform.position).normalized;
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
