using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 force;//0
    bool isRight;//false
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigid.AddForce(force,ForceMode2D.Impulse);
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, isRight == true ? -360f : 360) * Time.deltaTime);
    }

    public void SetForce(Vector2 _force, bool _isRight)
    {
        force = _force;
        isRight = _isRight;
    }
}
