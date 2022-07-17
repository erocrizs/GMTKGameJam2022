using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveMovement : MonoBehaviour
{
    [SerializeField]
    float runSpeed;

    [SerializeField]
    float burstJumpSpeed;

    [SerializeField]
    float maxFallSpeed;

    Rigidbody2D rb;
    OliveAnimator animator;

    bool IsRising => rb.velocity.y > 0;
    bool IsFalling => rb.velocity.y < 0;
    bool IsRunning => rb.velocity.x != 0;
    string[] groundLayers = { "Ground", "Die", "OliveGround" };
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
        float movement = Input.GetAxis("Horizontal Olive");
        if (movement >= 0.01 || movement <= -0.01)
        {
            rb.velocity = new Vector2(Mathf.Sign(movement) * runSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
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
        animator.SetDirection(rb.velocity.x);
        animator.IsFalling = IsFalling;
        animator.IsRising = IsRising;
        animator.IsRunning = IsRunning;
        animator.IsGrounded = IsGrounded;
    }
}
