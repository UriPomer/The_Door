using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private PlayerControl playerControl;
    Vector2 movement;
    Rigidbody2D rb;
    public Vector2 direction=new Vector2(0,1);

    private void Awake()
    {
        playerControl=new PlayerControl();
        rb=GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PlayerInput();
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }





    private void OnEnable()
    {
        playerControl.Enable();
    }
    private void OnDisable()
    {
        playerControl.Disable();
    }
    void PlayerInput()
    {
        movement=playerControl.MoveControl.Move.ReadValue<Vector2>();
        if (!movement.Equals(Vector2.zero)) direction = movement;
        

        //Debug.Log(movement.x);
    }
    private void Move()
    {
        rb.MovePosition(rb.position+movement*(moveSpeed*Time.fixedDeltaTime));
       // Debug.Log(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
    void Rotate()
    {
        transform.rotation = Quaternion.Euler(0,0, VectorAngle(new Vector2(0, 1), direction));

    }
    float VectorAngle(Vector2 from, Vector2 to)
    {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? angle : -angle;
    }

}
