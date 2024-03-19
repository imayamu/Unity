using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermanager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LayerMask blockLayer;
    public enum DIRECTION_TYPE
    {
        STOP,
        RIGHT,
        LEFT,
    }

    DIRECTION_TYPE direction = DIRECTION_TYPE.STOP;

    Rigidbody2D rigidbody2D;
    float speed;
    float jumpPower = 250;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if (x == 0)
        {
            direction = DIRECTION_TYPE.STOP;
        }
        else if (x > 0)
        {
            direction = DIRECTION_TYPE.RIGHT;
        }
        else if (x < 0)
        {
            direction = DIRECTION_TYPE.LEFT;
        }
        //jump
        if (IsGround()&&Input.GetKeyDown("space"))

        {
            //Debug.Log("Jump");
            Jump();
        }
    }
    private void FixedUpdate()
    {
        switch (direction)
        {
            case DIRECTION_TYPE.STOP:
                speed = 0;
                break;
            case DIRECTION_TYPE.RIGHT:
                speed = 3;
                transform.localScale =new Vector3(1,1,1);
                break;
            case DIRECTION_TYPE.LEFT:
                speed = -3;
                transform.localScale =new Vector3(-1,1,1);
                break;

        }
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

    }
    void Jump()
    {
        rigidbody2D.AddForce(Vector2.up*jumpPower);
    }

    bool IsGround()
    {
        //始点と終点を作成
        Vector3 leftStartPoint = transform.position - Vector3.right * 0.2f;
        Vector3 rightStartPoint = transform.position + Vector3.right * 0.2f;
        Vector3 endPoint = transform.position - Vector3.up * 0.1f;
        Debug.DrawLine(leftStartPoint, endPoint);
        Debug.DrawLine(rightStartPoint, endPoint);

        return Physics2D.Linecast(leftStartPoint, endPoint, blockLayer) || Physics2D.Linecast(rightStartPoint, endPoint, blockLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            Debug.Log("GameOver");
            gameManager.GameOver();
        }
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("クリア");
            gameManager.GameClear();
        }
        if(collision.gameObject.tag == "Item")
        {
            collision.gameObject.GetComponent<ItemManager>().GetItem();
        }
    }
}
