using System;
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

    private const float Tolerance = 5.0f; // мин радиус из которого считается макс ускорение - если в него зашли, то попали в центр поля
    private float MaxAcceleration; // макс ускорение

    private void Start()
    {
        MaxAcceleration = gravitation * mass / (Tolerance * Tolerance);
    }

    private void FixedUpdate()
    {
        // сначала расчет ускорения
        var diff = center - transform.position;
        var acceleration = gravitation * mass * diff.normalized / diff.sqrMagnitude;
        // проверка на заход в центр поля
        if (acceleration.magnitude > MaxAcceleration)
        {
            acceleration = acceleration.normalized * MaxAcceleration;
        }
        // теперь апдейты
        var delta = speed * Time.deltaTime + acceleration * (Time.deltaTime * Time.deltaTime) / 2;
        transform.Translate(delta);
        speed += acceleration * Time.deltaTime;
    }

}
