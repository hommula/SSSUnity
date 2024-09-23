using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BossNo1Controller : MonoBehaviour
{
    enum AttackMode { groundAttackMode, airAttackMode};
    enum ActionList 
    { 
        fireBullet, 
        bumpingAttack
    }
    Dictionary<ActionList, Action> actionMap;
    [SerializeField] List<ActionList> actionQueue = new List<ActionList>();

    [SerializeField] DetectionZone wallDetectionZone;
    [SerializeField] DetectionZone CliffDetectionZone;




    Transform tf;
    Rigidbody2D rb;
    DamageSystem _damageSystem;
    EnemyStats _enemyStats;
    Vector2 targetPosition;

    [SerializeField] float moveSpeed, dashAUG;
    [SerializeField] GameObject bulletPrefab, aimingLinePrefab;
    [SerializeField] Transform playerTransform;

    [SerializeField] bool canProcessNextAction, stun;
    const float case1DIS = 5, case2DIS = 8;
    [SerializeField] TextMeshProUGUI actionText;

    public UnityEvent processActionList;

    private void Start()
    {     
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();


        targetPosition = new Vector2(0, tf.position.y);


        actionMap = new Dictionary<ActionList, Action>
        {  { ActionList.fireBullet, FireBullet },
           { ActionList.bumpingAttack, BumpingAttack}
        };

        canProcessNextAction = false;
    }



    private void Update()
    {
        //print((playerTransform.position - tf.position).magnitude);
        nextActionToText();
        if (canProcessNextAction && !stun)
        {
            bossActionexecute();
        }
        if(CliffDetectionZone.detectedColliders.Count == 0)
        {
            tf.localScale = new Vector2(tf.localScale.x * -1, tf.localScale.y);
        }

        if (Input.GetKeyDown("v"))
        {
            FireBullet();
        }
        
        if(Input.GetKeyDown("9"))
        {
            canProcessNextAction = true;
        }
        if(Input.GetKeyDown("0"))
        {
            fillActionList();
            canProcessNextAction = true;
        }
    }

    //IEnumerator ExecuteActionsIE()
    //{
    //    if (!canProcessNextAction)
    //    {
    //        StopCoroutine("ExecuteActionIE");
    //    }

    //    while (actionQueue.Count > 0)
    //    {
    //        print("yeah");
    //        ActionList currentActionList = actionQueue[0];
    //        Action currentAction = actionMap[currentActionList];
    //        currentAction.Invoke();
    //        canProcessNextAction = false;
    //        StopCoroutine(currentAction.ToString());
    //        actionQueue.RemoveAt(0);
    //    }
    //    StopCoroutine("ExecuteActionIE");
    //    yield return null;
    //}

    // void bossActionExecute()
    //{
    //    Action currentAction;
    //    for(int i = 0; i < actionQueue.Count; i++)
    //    {
    //        currentAction = actionMap[actionQueue[i]];
    //        if (canProcessNewAction)
    //        {
    //            currentAction.Invoke();
    //        }
    //    }
    //}


    void fillActionList()
    {
        float distance = (playerTransform.position - tf.position).magnitude;
        switch (distance) 
        {
            case <= case1DIS:
                actionQueue.Add(ActionList.bumpingAttack);
                break;
            case var value when value > case1DIS && value < case2DIS * 8 / 5:
                if(Random.Range(0, 10) < 5)
                {
                    actionQueue.Add(ActionList.fireBullet);
                    break;
                }
                else
                {
                    actionQueue.Add(ActionList.bumpingAttack);
                    break;
                }
                
            case >= case2DIS:
                actionQueue.Add(ActionList.fireBullet);
                break;

        }
    }    

    void bossActionexecute()
    {

        canProcessNextAction = false;
        float distance = (playerTransform.position - tf.position).magnitude;
        switch (distance)
        {
            case <= case1DIS:
                //actionQueue.Add(ActionList.bumpingAttack);

                StartCoroutine("BumpingAttackIE");

                break;
            case var value when value > case1DIS && value < case2DIS * 8 / 5:
                if (Random.Range(0, 10) < 5)
                {
                    //actionQueue.Add(ActionList.fireBullet);
                    StartCoroutine("FireBulletThreeTimes");

                    break;
                }
                else
                {
                    //actionQueue.Add(ActionList.bumpingAttack);
                    StartCoroutine("WalkIE");
                    break;
                }

            case >= case2DIS:
                //actionQueue.Add(ActionList.fireBullet);
                StartCoroutine("FireBulletThreeTimes");
                break;
        }
        
    }
    
    void nextActionToText()
    {
        float distance = (playerTransform.position - tf.position).magnitude;
        switch (distance) 
        {
            case <= case1DIS:
                actionText.text = ActionList.bumpingAttack.ToString();
                //actionQueue.Add(ActionList.dashAwayFromPlayer);
                break;
            case var value when value > case1DIS && value < case2DIS * 8/5:
                actionText.text = ActionList.fireBullet.ToString() + " or " + ActionList.bumpingAttack.ToString();
                //actionQueue.Add(ActionList.dashAttackToPlayer);
                break;
            case >= case2DIS:
                actionText.text = ActionList.fireBullet.ToString();
                //actionQueue.Add(ActionList.moveToPlayer);
                break;
        }
    }


    IEnumerator FireBulletThreeTimes()
    {
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(FireBulletIE());
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1.9f);
        canProcessNextAction = true;
    }
    IEnumerator FireBulletIE()
    {
        Vector2 spawnPosition = tf.position, targetPosition = playerTransform.position;

        GameObject aimingLine = Instantiate(aimingLinePrefab, spawnPosition, UnityEngine.Quaternion.identity);
        AimingLineController aimLineController = aimingLine.GetComponent<AimingLineController>();
        if (aimLineController != null)
        {
            aimLineController.setTarget(targetPosition);
              
        }
        
        yield return new WaitForSeconds(2.5f);
        Destroy(aimingLine);
        

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, UnityEngine.Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.SetTarget(targetPosition);
        }
        yield return new WaitForSeconds(1);
    }
    private void FireBullet()
    {
        StartCoroutine("FireBulletIE");
    }
    
    IEnumerator BumpingAttackIE()
    {
        if(Random.Range(1, 10) < 5)
        {
            tf.localScale = new Vector2(tf.localScale.x * -1, tf.localScale.y);
        }
        for (float i = 0; i < .5; i += Time.deltaTime)
        {
            tf.position = new Vector2(tf.position.x + Time.deltaTime * 16 * tf.localScale.x, tf.position.y);
            yield return null;
            if (wallDetectionZone.detectedColliders.Count > 0)
            {
                StartCoroutine("Stun");
                StopCoroutine("BumpingAttackIE");
            }
        }
        canProcessNextAction = true;
        StopCoroutine("BumpingAttackIE");

    }

    IEnumerator WalkIE()
    {
        for(float i = 0; i < 1; i += Time.deltaTime)
        {
            tf.position = new Vector2(tf.position.x + Time.deltaTime * 4 * -tf.localScale.x, tf.position.y);
            yield return null;
        }
        canProcessNextAction = true;
        StopCoroutine("WalkIE");
    }
    private void BumpingAttack()
    {
        StartCoroutine("BumpingAttackIE");
    }

    IEnumerator Stun()
    {
        stun = true;
        yield return new WaitForSeconds(3);
        stun = false;
        canProcessNextAction = true;
        StopCoroutine("Stun");
        
    }
}


// Past code
/*
 * IEnumerator MoveToMidIE()
    {
        while (transform.position.x != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        canProcessNextAction = true;
    }
    private void MoveToMid()
    {
        StartCoroutine(MoveToMidIE());
        
    }
    IEnumerator MoveToPlayerIE()
    {
        Vector3 _vector3 = new Vector3 (playerTransform.position.x, transform.position.y);
        while (transform.position.x != _vector3.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, _vector3, moveSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        canProcessNextAction = true;
    }
    private void MoveToPlayer()
    {
        StartCoroutine(MoveToPlayerIE()); 
    }
    IEnumerator DashAwayFromPlayerIE()
    {
        Vector3 _vector3 = new Vector3(2 * tf.position.x - playerTransform.position.x, transform.position.y);
        while (transform.position.x != _vector3.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, _vector3, moveSpeed * dashAUG * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        canProcessNextAction = true;
    }
    private void DashAwayFromPlayer()
    {
        StartCoroutine(DashAwayFromPlayerIE());
    }
    IEnumerator DashAttackToPlayerIE()
    {
        Vector3 _vector3 = new Vector3(playerTransform.position.x, transform.position.y);
        while (transform.position.x != _vector3.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, _vector3, moveSpeed * dashAUG * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        canProcessNextAction = true;
    }
    private void DashAttackToPlayer()
    {
        StartCoroutine(DashAttackToPlayerIE());
    }
 * 
 */

