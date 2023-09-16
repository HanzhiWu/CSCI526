using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private static Vector3 playerRespawn;
    // temp
    private static bool isDead = false;

    // Start is called before the first frame update\
    private void Awake()
    {
        playerRespawn = transform.position;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            transform.position = playerRespawn;
            isDead = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Player is hit by bullet");
            isDead = true;
        }

        if (collision.gameObject.tag == "Poison")
        {
            Debug.Log("Player is hit by poison");
            isDead = true;
        }
    }
}
