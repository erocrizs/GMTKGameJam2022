using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMovement : MonoBehaviour
{
    [SerializeField]
    float rollTime;

    bool isMoving = false;
    float moveLerp = 1;
    float moveDirection = 0;
    Vector2 preMovementPosition;
    Rigidbody2D rb;
    Action<float> Roll;

    private Vector2 RoundVector (Vector2 v)
    {
        float x = Mathf.RoundToInt(v.x);
        float y = Mathf.RoundToInt(v.y);
        return new Vector2(x, y);
    }

    private Vector2 LerpPosition (float degree, float rotationalDirection, Vector2 corner)
    {
        Vector2 cornerPosition = preMovementPosition + corner;
        Vector2 cornerToCenter = -corner;
        Vector2 rotatedCornerToCenter = Quaternion.Euler(0, 0, degree * rotationalDirection) * cornerToCenter;

        return cornerPosition + rotatedCornerToCenter;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Roll = RollHorizontal;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << LayerMask.NameToLayer("Ground"));
            float movement = Input.GetAxis("Horizontal");
            isMoving = movement >= 0.01 || movement <= -0.01;
            if (isMoving && isGrounded)
            {
                moveLerp = 0;
                moveDirection = movement;
                rb.isKinematic = true;
                rb.simulated = false;
                preMovementPosition = RoundVector(rb.position);
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            float progress = Mathf.Min(Time.fixedDeltaTime / rollTime, 1 - moveLerp);
            moveLerp += progress;
            Roll(progress);
        }
        else
        {
            rb.position = new Vector2(Mathf.RoundToInt(rb.position.x), rb.position.y);
        }
    }

    void RollHorizontal (float progress)
    {
        float degree = moveLerp * 90;
        float rotationalDirection = -1 * Mathf.Sign(moveDirection);
        Vector2 corner = new Vector2(Mathf.Sign(moveDirection) * 0.5f, -0.5f);

        transform.position = LerpPosition(degree, rotationalDirection, corner);
        transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

        if (moveLerp == 1)
        {
            // bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << LayerMask.NameToLayer("Ground"));
            
            RollStop();
        }
    }

    void RollUp (float progress)
    {
        
    }

    void RollDown (float progress)
    {
        // TODO
    }

    void RollStop ()
    {
        isMoving = false;
        rb.isKinematic = false;
        rb.simulated = true;
        rb.position = RoundVector(rb.position);
    }
}
