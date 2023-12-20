using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public enum hitType
    {
        WallCheck,
        HitCheck,
    }

    Rigidbody2D rigid;
    Vector3 moveDir;//default
    BoxCollider2D boxCollider2D;
    Animator anim;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] bool isGround = false;//false 공중에 떠있는 상태, true 땅에 붙어있는 상태

    private bool isJump = false;
    private float verticalVelocity;//수직으로 받는 힘
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpForce = 5f;
    private Camera mainCam;

    [Header("투척무기")]
    Transform trsHand;
    bool isPlayerLookAtRightDirection;
    [SerializeField] GameObject objSword;
    Transform trsSword;
    [SerializeField] Transform trsObjDynamic;
    [SerializeField] float throwlimit = 0.3f;
    float throwTimer = 0.0f;

    bool GamePause = false;

    [Header("대쉬")]
    private bool dash = false;
    private float dashTimer = 0.0f;
    private TrailRenderer dashEffect;
    [SerializeField] private float dashLimit = 0.2f;

    [Header("벽점프")]
    private bool wallStep = false;
    private bool doWallStep = false;
    private bool doWallStepTimer = false;
    private float wallStepTimer = 0.0f;
    [SerializeField] private float wallStepTime = 0.3f;

    private void OnDrawGizmos()
    {
        if (boxCollider2D != null)
        {
            Gizmos.color = Color.red;
            Vector3 pos = boxCollider2D.bounds.center - new Vector3(0, 0.1f, 0);
            Gizmos.DrawWireCube(pos, boxCollider2D.bounds.size);
        }
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        trsHand = transform.Find("Hand");
        trsSword = trsHand.GetChild(0);
        dashEffect = GetComponent<TrailRenderer>();
        dashEffect.enabled = false;
    }

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        checkMouse();

        checkGround();
        moving();
        //turning();

        jumping();
        checkGravity();

        checkDash();

        doAnimation();

        checkDoStepWallTimer();
    }

    private void checkMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCam.transform.position.z;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mousePos);

        //플레이어의 좌우 확인
        Vector3 distanceMouseToPlayer = mouseWorldPos - transform.position;
        if (distanceMouseToPlayer.x > 0 && transform.localScale.x != -1)
        {
            transform.localScale = new Vector3(-1f, 1, 1);
            isPlayerLookAtRightDirection = true;
        }
        else if(distanceMouseToPlayer.x < 0 && transform.localScale.x != 1)
        {
            transform.localScale = new Vector3(1f, 1, 1);
            isPlayerLookAtRightDirection = false;
        }

        //마우스 에임
        //Vector3 direction = isPlayerLookAtRightDirection == true ? Vector3.right : Vector3.left;
        Vector3 direction = Vector3.right;
        if (isPlayerLookAtRightDirection == false)
        {
            direction = Vector3.left;
        }

        float angle = Quaternion.FromToRotation(direction, distanceMouseToPlayer).eulerAngles.z;
        if (isPlayerLookAtRightDirection == true)
        {
            angle = -angle;
        }

        //trsHand.rotation = Quaternion.Euler(0, 0, angle);
        trsHand.localEulerAngles = new Vector3(trsHand.localEulerAngles.x, trsHand.localEulerAngles.y, angle);

        if (throwTimer != 0.0f)
        {
            throwTimer -= Time.deltaTime;
            if(throwTimer < 0.0f) 
            {  
                throwTimer = 0.0f; 
            }
        }

        if (GamePause == false && Input.GetKey(KeyCode.Mouse0) && throwTimer == 0.0f)
        {
            shoot();
            throwTimer = throwlimit;
        }
    }

    private void shoot()
    {
        GameObject obj = Instantiate(objSword, trsSword.position, trsSword.rotation, trsObjDynamic);
        Sword sc = obj.GetComponent<Sword>();
        //Vector2 throwForce = isPlayerLookAtRightDirection == true ? new Vector2(10.0f, 0f) : new Vector2(-10.0f, 0f);
        Vector2 throwForce = new Vector2(10.0f, 0f);
        if (isPlayerLookAtRightDirection == false)
        {
            //throwForce = new Vector2(-10.0f, 0f);
            throwForce.x = -10.0f;
        }
        sc.SetForce(trsSword.rotation * throwForce, isPlayerLookAtRightDirection);
    }

    private void checkGround()
    {
        isGround = false;
        if (verticalVelocity > 0)//5
        {
            return;
        }

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f,
            Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        if (hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if (dash == true || doWallStepTimer == true) return;

        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.y = rigid.velocity.y;
        rigid.velocity = moveDir;
    }

    //private void turning()
    //{
    //    if (moveDir.x > 0 && transform.localScale.x > -1)//오른쪽으로 이동중
    //    {
    //        Vector3 scale = transform.localScale;
    //        scale.x *= -1;
    //        transform.localScale = scale;
    //    }
    //    else if (moveDir.x < 0 && transform.localScale.x < 1)//왼쪽으로 이동중
    //    {
    //        Vector3 scale = transform.localScale;
    //        scale.x *= -1;
    //        transform.localScale = scale;
    //    }
    //}

    private void jumping()
    {
        if(isGround == false)
        {
            if(isGround == false && Input.GetKeyDown(KeyCode.Space) && wallStep == true && moveDir.x != 0.0f)//(moveDir.x <  0 || moveDir.x >0)
            {
                doWallStep = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            isJump = true;
        }

    }

    private void checkGravity()
    {
        if (dash == true) return;

        if(doWallStep == true)//벽점프를 해야할때
        {
            Vector2 dir = rigid.velocity;
            dir.x *= -1;
            rigid.velocity = dir;//튀는 방향
            verticalVelocity = jumpForce;//힘은 점프 포스의 힘으로 점프
            doWallStep = false;
            doWallStepTimer = true;
        }

        if (isGround == false)//공중에 떠 있을때
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (verticalVelocity < -10.0f)//만약에 떨어지는 속도가 -10보다 작아지면 -10으로 제한
            {
                verticalVelocity = -10f;
            }
        }
        else//땅에 붙어 있을때
        {
            verticalVelocity = 0;//0
        }

        if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void checkDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dash == false)
        {
            dash = true;

            verticalVelocity = 0f;

            Vector2 direction = new Vector2(20.0f, 0f);
            if (isPlayerLookAtRightDirection == false)
            {
                direction.x = -20.0f;
            }

            rigid.velocity = direction;

            dashEffect.enabled = true;
        }
        else if (dash == true)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashLimit)//대쉬를 종료
            {
                dashTimer = 0.0f;
                dashEffect.enabled = false;
                dashEffect.Clear();
                dash = false;
            }
        }
    }

    private void doAnimation()
    {
        anim.SetBool("IsGround", isGround);
        anim.SetInteger("Horizontal", (int)moveDir.x);

        int curHorizontal = anim.GetInteger("Horizontal");
    }

    private void checkDoStepWallTimer()
    {
        if (doWallStepTimer == true)
        {
            wallStepTimer += Time.deltaTime;//0.0f ~ 0.3f
            if (wallStepTimer >= wallStepTime)
            {
                wallStepTimer = 0.0f;
                doWallStepTimer = false;
            }
        }
    }

    public void TriggerEnter(hitType _type, Collider2D _collision)
    {
        //&& _collision.gameObject.layer == LayerMask.NameToLayer("Ground")
        if (_type == hitType.WallCheck && _collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wallStep = true;
        }
        else if(_type == hitType.HitCheck && _collision.gameObject.tag == "Item")
        {
            item sc = _collision.gameObject.GetComponent<item>();
            sc.GetItem();
        }
    }

    public void TriggerExit(hitType _type, Collider2D _collision)
    {
        if (_type == hitType.WallCheck && _collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            wallStep = false;
        }
    }
}
