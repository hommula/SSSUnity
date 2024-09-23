using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ParryCenterPoint : MonoBehaviour
{
    [SerializeField]Transform obToFollow;
    [SerializeField]UnityEvent startParry;
    [SerializeField]UnityEvent endParry;
    bool isParry = false;
    float parryDuration = 0.3f;
    [SerializeField] GameObject range;
    private void Start()
    {
    }
    private void Update()
    {
        if(!PauseMenu.GameIsPaused)
        {
            followObject();
            if (!isParry) {rotateSelf();};
        }
        if (Input.GetMouseButtonDown(1) && !isParry)
        {
            StartCoroutine(parry());
        }
    }


    private void followObject()
    {
        transform.position = obToFollow.position;
    }
    private void rotateSelf()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    private IEnumerator parry()
    {
        isParry = true;
        startParry?.Invoke();
        Collider2D shieldCollider = GetComponent<Collider2D>();
        shieldCollider.enabled = true;
        yield return new WaitForSeconds(parryDuration);
        shieldCollider.enabled = false;
        endParry?.Invoke();
        isParry = false;
    }

    public void showSprite()
    {
        SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
        rangeSprite.enabled = true;
    }
    public void stopParry()
    {
        SpriteRenderer rangeSprite = range.GetComponent<SpriteRenderer>();
        rangeSprite.enabled = false;
    }
}
