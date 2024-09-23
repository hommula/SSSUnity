using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimingLineController : MonoBehaviour
{
    Transform playerTf, tf;
    LineRenderer lineRenderer;


    Color lineColor;
    [SerializeField]float width, flickerSpeed;
    Vector2 startPoint, endPoint;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        lineRenderer = GetComponent<LineRenderer>();
        lineColor = Color.red;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        startPoint = tf.position;
    }

    // Update is called once per frame
    void Update()
    {
        print(endPoint);
        DrawLine();
        float alpha = Mathf.PingPong(Time.time * flickerSpeed, 1f);
        lineRenderer.startColor = new Color(lineColor.r, lineColor.g, lineColor.b, alpha);
        lineRenderer.endColor = new Color(lineColor.r, lineColor.g, lineColor.b, alpha);
    }
    
    void DrawLine()
    {
        


        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
        lineRenderer.SetPosition(2, startPoint + (endPoint - startPoint).normalized * 100);
    }

    public void setTarget(Vector2 target)
    {
        endPoint = target;
    }
}
