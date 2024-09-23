using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDraw : MonoBehaviour
{
    [SerializeField] int offset = 6;
    public int minX;
    public int maxX;
    public int minY;
    public int maxY;
    public int _targetFrameRate;

    public Transform objectToFollow;
    private void Update()
    {
        minX = -(int)objectToFollow.position.x + offset;
        maxX = (int)objectToFollow.position.x + offset;
        minY = -(int)objectToFollow.position.y + offset;
        maxY = (int)objectToFollow.position.y + offset;
        Application.targetFrameRate = _targetFrameRate;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Vector3 pos0 = new Vector3();
        Vector3 pos1 = new Vector3();
        for (int i = -minX; i < maxX; i++)
        {
            pos0.x = i;
            pos0.y = -minY;
            pos1.x = i;
            pos1.y = maxY;
            Gizmos.DrawLine(
                pos0,
                pos1
            );
            /*for (int j = 0; j < 100; j++) {

            }*/
        }

        for (int i = -minY; i < maxY; i++)
        {
            pos0.x = -minX;
            pos0.y = i;
            pos1.x = maxX;
            pos1.y = i;
            Gizmos.DrawLine(
                pos0,
                pos1
            );
        }
    }
}
