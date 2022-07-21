using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveMovement : MonoBehaviour
{
    float movement;
    [SerializeField]
    float maxRunSpeed;
    [SerializeField]
    float runAccelerationTime;
    float RunAcceleration => maxRunSpeed / runAccelerationTime;
    [SerializeField]
    float runDecelerationTime;
    float RunDeceleration => maxRunSpeed / runDecelerationTime;
    [SerializeField]
    float turnAccelerationTime;
    float TurnAcceleration => maxRunSpeed / turnAccelerationTime;
    [SerializeField]
    float burstJumpSpeed;
    [SerializeField]
    float maxFallSpeed;

    Rigidbody2D rb;
    OliveAnimator animator;

    bool IsRising => rb.velocity.y > 0;
    bool IsFalling => rb.velocity.y < 0;
    bool IsRunning => rb.velocity.x != 0;
    string[] groundLayers = { "Ground", "Die", "OliveGround",  "MuddyPole" };
    bool IsGrounded {
        get {
            if (Physics2D.Raycast(transform.position, Vector2.down, 0.01f, LayerMask.GetMask(groundLayers)))
            {
                return true;
            }

            if (Physics2D.Raycast(transform.position + new Vector3(0.22f, 0f, 0f), Vector2.down, 0.01f, LayerMask.GetMask(groundLayers)))
            {
                return true;
            }

            if (Physics2D.Raycast(transform.position - new Vector3(0.22f, 0f, 0f), Vector2.down, 0.01f, LayerMask.GetMask(groundLayers)))
            {
                return true;
            }

            return false;
        }
    }
        

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<OliveAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        MaintainFall();
        Animate();
    }

    void Move ()
    {
        movement = Input.GetAxisRaw("Horizontal Olive");

        // moving
        if (movement != 0)
        {
            float acceleration = (Mathf.Sign(movement) == Mathf.Sign(rb.velocity.x)) ? RunAcceleration : TurnAcceleration;
            float acceleratedVxMagnitude = rb.velocity.x + (Mathf.Sign(movement) * acceleration * Time.deltaTime);
            float cappedVxMagnitude = Mathf.Clamp(acceleratedVxMagnitude, -maxRunSpeed, maxRunSpeed);
            rb.velocity = new Vector2(cappedVxMagnitude, rb.velocity.y);
        }
        // slowing down
        else if (Mathf.Abs(rb.velocity.x) > 0)
        {
            float acceleratedVxMagnitude = rb.velocity.x + (-Mathf.Sign(rb.velocity.x) * RunDeceleration * Time.deltaTime);
            float cappedVxMagnitude = (Mathf.Sign(rb.velocity.x) == Mathf.Sign(acceleratedVxMagnitude)) ? acceleratedVxMagnitude : 0f;
            rb.velocity = new Vector2(cappedVxMagnitude, rb.velocity.y);
        }
        // maintain stop
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        velocity = rb.velocity.x;
    }

    void Jump ()
    {
        bool grounded = IsGrounded;
        if (grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.position = new Vector2(rb.position.x, rb.position.y + 0.05f);
                rb.AddForce(new Vector2(0, burstJumpSpeed), ForceMode2D.Impulse);
            }
        }
    }

    void MaintainFall()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
    }

    void Animate ()
    {
        animator.SetDirection(movement == 0 ?  rb.velocity.x : movement);
        animator.IsFalling = IsFalling;
        animator.IsRising = IsRising;
        animator.IsRunning = IsRunning;
        animator.IsGrounded = IsGrounded;
    }
}
