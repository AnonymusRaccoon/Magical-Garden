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
    private int wallJumpTimer = 0;
    private bool wallJumped;
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
                wallJumped = false;
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
        List<RaycastHit> raycastHits = Physics.BoxCastAll(rb.position - new Vector2(0.6f, 0), new Vector3(0.55f, 0, 1), Vector3.forward, Quaternion.identity, 1.2f).ToList();
        List<RaycastHit> walls = new List<RaycastHit>();

        foreach (RaycastHit hit in raycastHits)
        {
            walls.Add(hit);
        }

        if (walls.Count > 0)
        {
            wallJumpTimer = 10;

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
        wallJumpTimer--;
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
        print(isSliding);

        if (-1 < velocity && velocity < 1)
            velocity = 0;

        if (velocity > 0)
            velocity -= 0.5f;
        else if (velocity < 0)
            velocity += 0.5f;

        float horizontalVel = Input.GetAxis(Horizontal);

        //Change air velocity after wall jump
        if (wallJumped)
        {
            if (-0.3 < horizontalVel && horizontalVel < 0.3)
                horizontalVel = 0.6f * jumpDirection;
            if (Mathf.Sign(horizontalVel) == Mathf.Sign(horizontalVel))
                horizontalVel /= 2;
        }

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
            if (wallJumped && Mathf.Abs(rb.velocity.x) > smallJumpPush)
            {
                velocity = smallJumpPush * jumpDirection;
                rb.velocity = new Vector2(rb.velocity.x - (wallJumpPush - smallJumpPush) * jumpDirection, rb.velocity.y);
            }
            else
                rb.velocity = new Vector2(rb.velocity.x, smallJump);
        }

        //Apply more gravity
        if (rb.velocity.y < 1 && !isSliding)
            rb.AddForce(new Vector3(0, gravity, 0), ForceMode2D.Force);

        //WallSlide
        if (wallJumpTimer > 0)
        {
            //Wall Jump
            if (Input.GetKeyDown(JumpKey) && !isGrounded)
            {
                wallJumped = true;
                jumpDirection = -wallDirection;
                velocity = wallJumpPush * jumpDirection;
                rb.AddForce(new Vector3(wallJumpPush * jumpDirection, wallJump, 0), ForceMode2D.Impulse);
            }
            else if (isSliding)
                rb.AddForce(new Vector3(0, Mathf.Abs(rb.velocity.y) + gravity / 2, 0), ForceMode2D.Force);
        }
    }

    Vector3 NormaliseMovement(float x, float y)
    {
        Vector3 input = new Vector3(x, y, 0);
        input.Normalize();

        if (input.x == 0 && input.y == 0)
            input.x = 1;

        return input;
    }
}
