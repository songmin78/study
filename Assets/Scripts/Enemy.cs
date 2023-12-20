using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] LayerMask ground;

    Rigidbody2D rigid;
    [SerializeField] Collider2D trigger;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moving();
    }

    private void moving()
    {
        rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
    }

    private void FixedUpdate()
    {
        if (trigger.IsTouchingLayers(ground) == false)
        {
            turn();
        }

    }

    private void turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        moveSpeed *= -1;
    }
}
