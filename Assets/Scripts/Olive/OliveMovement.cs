using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveMovement : MonoBehaviour
{
    [SerializeField]
    float runSpeed;

    [SerializeField]
    float burstJumpSpeed;

    Rigidbody2D rb;

    bool IsRising => rb.velocity.y > 0;
    bool IsFalling => rb.velocity.y < 0;
    bool IsRunning => rb.velocity.x != 0;
    bool IsGrounded => Physics2D.Raycast(transform.position, Vector2.down, 0.01f, 1 << LayerMask.NameToLayer("Ground"));

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move ()
    {
        float movement = Input.GetAxis("Horizontal");
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
        Debug.Log(grounded);
        if (grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.position = new Vector2(rb.position.x, Mathf.Floor(rb.position.y) + 0.5f);

            if (Input.GetButton("Jump"))
            {
                rb.AddForce(new Vector2(0, burstJumpSpeed), ForceMode2D.Impulse);
            }
        }
    }
}
