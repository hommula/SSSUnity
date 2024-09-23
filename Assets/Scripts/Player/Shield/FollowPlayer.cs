using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FollowPlayer : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    [SerializeField] float parryDuration = .5f;
    public bool isParry = false;
    [SerializeField] float angle;
    public UnityEvent parry;
    [SerializeField] float angleToRotate = 0;
    [SerializeField] float index = 1;

    private void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            followPlayer();
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(parryStart());
        }
       
        
    }
    void followPlayer()
    {
        transform.position = objectToFollow.position + offset;

        if (!isParry)
        {
            if(index == 1)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - transform.position;

                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float i = Mathf.Round(angle / 45);
                Debug.Log(i);
                if (angle < -162 || angle > 162)
                {
                    angleToRotate = 180;
                }
                else if (angle > 0)
                {
                    angleToRotate = Mathf.Clamp(40 * i, 50 * i, angle);
                }
                else if (angle < 0)
                {
                    angleToRotate = -Mathf.Clamp(-40 * i, 50 * -i, -angle);
                }

                transform.rotation = Quaternion.AngleAxis((int)(angleToRotate / 45) * 45, Vector3.forward);
            }
            else if(index == 2) 
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = mousePosition - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
        }
    }

    private IEnumerator parryStart()
    {
        parry?.Invoke();
        isParry = true;
        yield return new WaitForSeconds(parryDuration);
        isParry = false;
    }
}
