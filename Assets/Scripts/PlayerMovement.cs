using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public string Horizontal;
    public string Vertical;
    public KeyCode JumpKey = KeyCode.None;

    [Space]
    [Space]
    [Header("Movement settings")]
    [SerializeField] private int speed = 10;
    [SerializeField] private int airSpeed = 7;
    [SerializeField] private int jumpForce = 8;
    [SerializeField] private int smallJump = 4;
    [SerializeField] private float gravity = -10;
    [SerializeField] private int wallJump = 4;
    [SerializeField] private int wallJumpPush = 10;
    [SerializeField] private int smallJumpPush = 5;
    private bool groundedLastFrame = false;
    private int wallDirection;
    private float jumpDirection;
    [HideInInspector] public float velocity = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsGrounded()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.1f)
        {
            if (groundedLastFrame)
            {
                return true;
            }
            else
            {
                groundedLastFrame = true;
                return false;
            }
        }
        groundedLastFrame = false;
        return false;
    }

    private bool IsSliding()
    {
        List<RaycastHit2D> walls = Physics2D.RaycastAll(rb.position, Vector2.left, GetComponent<Renderer>().bounds.size.x / 2 + .1f).ToList();
        walls.AddRange(Physics2D.RaycastAll(rb.position, Vector2.right, 5.8f));
        walls.RemoveAll(x => x.collider.tag == "Player");

        if (walls.Count > 0)
        {
            if (walls[0].transform.position.x > rb.position.x && Input.GetAxis(Horizontal) > 0)
            {
                wallDirection = 1;
                return true;
            }
            else if (walls[0].transform.position.x < rb.position.x && Input.GetAxis(Horizontal) < 0)
            {
                wallDirection = -1;
                return true;
            }
        }
        return false;
    }

    private bool AirControl(bool isGrounded)
    {
        if (!isGrounded)
        {
            if (jumpDirection > 0)
            {
                if (Input.GetAxis(Horizontal) > 0)
                    return false;
                else
                    return true;
            }
            else
            {
                if (Input.GetAxis(Horizontal) > 0)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private void FixedUpdate()
    {
        if (Horizontal == null || JumpKey == KeyCode.None)
            return;
        bool isGrounded = IsGrounded();
        bool isSliding = IsSliding();
        bool airControl = AirControl(isGrounded);

        if (-1 < velocity && velocity < 1)
            velocity = 0;

        if (velocity > 0)
            velocity -= 0.5f;
        else if (velocity < 0)
            velocity += 0.5f;

        float horizontalVel = Input.GetAxis(Horizontal);

        //Make player move
        if (!isSliding || (isSliding && Mathf.Sign(wallDirection) != Mathf.Sign(Input.GetAxis(Horizontal))))
        {
            rb.AddForce(new Vector2(horizontalVel * (airControl ? airSpeed : speed) - (rb.velocity.x - velocity), 0), ForceMode2D.Impulse);
        }

        //Make user jump
        if (Input.GetKey(JumpKey) && isGrounded)
        {
            jumpDirection = Input.GetAxis(Horizontal);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        //Make a small jump if user drop the button
        if (Input.GetKeyUp(JumpKey) && !isGrounded && rb.velocity.y > smallJump)
        {
            //if (Mathf.Abs(rb.velocity.x) > smallJumpPush)
            //{
            //    velocity = smallJumpPush * jumpDirection;
            //    rb.velocity = new Vector2(rb.velocity.x - (wallJumpPush - smallJumpPush) * jumpDirection, rb.velocity.y);
            //}
            //else
                rb.velocity = new Vector2(rb.velocity.x, smallJump);
        }

        //Apply more gravity
        if (rb.velocity.y < 1 && !isSliding)
            rb.AddForce(new Vector3(0, gravity, 0), ForceMode2D.Force);

        //Wall Slide
        if (isSliding && !isGrounded)
            rb.AddForce(new Vector3(0, Mathf.Abs(rb.velocity.y) + gravity / 2, 0), ForceMode2D.Force);
    }
}
