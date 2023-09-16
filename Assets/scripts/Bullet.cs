using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 1;
    public GameObject targetToDestroy;
    public string playerObjectName = "player";

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void Start()
    {
        targetToDestroy = GameObject.Find(playerObjectName);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == targetToDestroy)
        {
            Destroy(targetToDestroy);
        }
        Destroy(gameObject);
    }
}
