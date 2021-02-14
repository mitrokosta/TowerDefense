using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField]
    private Vector3 speed; // текущая скорость, сохраняется
    
    [SerializeField]
    private Vector3 center; // центр поля
    
    [SerializeField]
    private float gravitation; // константа гравитации
    
    [SerializeField]
    private float mass; // масса тела вокруг которого крутимся

    private const float Tolerance = 0.1f;
    
    private void FixedUpdate()
    {
        var diff = center - transform.position;
        if (diff.magnitude < Tolerance)
        {
            speed = Vector3.zero;
            transform.Translate(diff);
            return;
        }
        
        // сначала расчет ускорения
        var acceleration = gravitation * mass * diff.normalized / diff.sqrMagnitude;
        // теперь передвижение
        var delta = speed * Time.deltaTime + acceleration * (Time.deltaTime * Time.deltaTime) / 2;
        // проверим что не прошли через центр
        if (PointToSegmentDistance(center, transform.position, transform.position + delta) < Tolerance)
        {
            speed = Vector3.zero;
            acceleration = Vector3.zero;
            delta = diff;
        }
        
        transform.Translate(delta);
        speed += acceleration * Time.deltaTime;
    }

    // расстояние от точки c до отрезка ab
    private static float PointToSegmentDistance(Vector3 c, Vector3 a, Vector3 b)
    {
        var ab = b - a;
        var ac = c - a;
        var diff = Vector3.Dot(ab, ac) / ab.sqrMagnitude;
        if (diff > 1)
        {
            diff = 1;
        }
        else if (diff < 0)
        {
            diff = 0;
        }

        return (a + diff * ab - c).magnitude;
    }
}
