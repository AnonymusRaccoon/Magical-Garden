using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject cam;
    public string Horizontal;
    public string Vertical;
    public KeyCode JumpKey = KeyCode.None;
    public Vector2 offset;

    [Space]
    [Space]
    [Header("Movement settings")]
    [SerializeField] private int speed = 10;
    [SerializeField] private int airSpeed = 7;
    [SerializeField] private int jumpForce = 8;
    [SerializeField] private int smallJump = 4;
    [SerializeField] private float gravity = -10;
    private float jumpDirection;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void LateUpdate()
    {
        cam.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, -17);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        if (Horizontal == null || JumpKey == KeyCode.None)
            return;


        float horizontalVel = Input.GetAxis(Horizontal);

        //Make player move
        rb.AddTorque(horizontalVel * -1 * speed - rb.GetPointVelocity(rb.position).x * -1);

        //Make user jump
        if (Input.GetKey(JumpKey) && isGrounded)
        {
            jumpDirection = Input.GetAxis(Horizontal);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //Make a small jump if user drop the button
        if (Input.GetKeyUp(JumpKey) && !isGrounded && rb.velocity.y > smallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, smallJump);
        }
    }
}
