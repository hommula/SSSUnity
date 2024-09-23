using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shield;
    public SpriteRenderer shieldRenderer;
    [SerializeField]float parryDuration = .25f;
    private bool isParrying = false;

    public void OnParry()
    {
        if (!isParrying)
        {
            isParrying = true;
            StartCoroutine(parry());
        }
    }
    private IEnumerator parry()
    {
        Collider2D shieldCollider = shield.GetComponent<Collider2D>();
        Color originalColor = shieldRenderer.color;
        if (shieldCollider != null )
        {
            shieldCollider.enabled = true;
            shieldRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(parryDuration);
        if (shieldCollider != null)
        {
            shieldCollider.enabled = false;
            shieldRenderer.color = originalColor;
            isParrying = false;
        }

        yield return new WaitForSeconds(parryDuration);
    }

}
