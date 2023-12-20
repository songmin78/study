using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private Player.hitType hitType;
    //[SerializeField] LayerMask layer;
    Player player;
    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.TriggerEnter(hitType, collision);

        //int value = (int)Mathf.Log(layer, 2);
        //if(collision.gameObject.layer == layer)
        //{
        //    player.TriggerEnter(hitType, collision);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player.TriggerExit(hitType, collision);

        //if (collision.gameObject.layer == layer)
        //{
        //    player.TriggerExit(hitType, collision);
        //}
    }

    void Update()
    {
        
    }
}
