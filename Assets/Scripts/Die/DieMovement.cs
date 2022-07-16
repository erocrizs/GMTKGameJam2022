using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMovement : MonoBehaviour
{
    [SerializeField]
    float horizontalRollTime;

    [SerializeField]
    float verticalRollTime;

    public bool isMoving = false;
    float moveLerp = 1;
    float moveDirection = 0;
    Vector2 preMovementPosition;
    Rigidbody2D rb;
    Action Roll;

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
                bool isFacingWall = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(movement), 0.6f, 1 << LayerMask.NameToLayer("Ground"));

                if (isFacingWall)
                {
                    RollStart(movement, RollUp);
                }
                else
                {
                    RollStart(movement, RollHorizontal);
                }

                moveLerp = 0;
                preMovementPosition = RoundVector(rb.position);
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Roll();
        }
        else
        {
            rb.position = new Vector2(Mathf.RoundToInt(rb.position.x), rb.position.y);
        }
    }

    void RollHorizontalBase (float progress)
    {
        moveLerp += progress;
        float degree = moveLerp * 90;
        float rotationalDirection = -1 * Mathf.Sign(moveDirection);
        Vector2 corner = new Vector2(Mathf.Sign(moveDirection) * 0.5f, -0.5f);

        transform.position = LerpPosition(degree, rotationalDirection, corner);
        transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

        if (moveLerp == 1)
        {
            bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << LayerMask.NameToLayer("Ground"));

            if (isGrounded)
            {
                RollStop();
            }
            else
            {
                ResetRoll(RollDown);
            }
        }
    }

    void RollHorizontal ()
    {
        float progress = Mathf.Min(Time.fixedDeltaTime / horizontalRollTime, 1 - moveLerp);
        RollHorizontalBase(progress);
    }

    void RollHorizontalFromUp ()
    {
        float progress = Mathf.Min(Time.fixedDeltaTime / horizontalRollTime, 1 - moveLerp);
        RollHorizontalBase(progress);
    }

    void RollUp ()
    {
        float progress = Mathf.Min(Time.fixedDeltaTime / horizontalRollTime, 1 - moveLerp);
        moveLerp += progress;
        float degree = moveLerp * 90;
        float rotationalDirection = -1 * Mathf.Sign(moveDirection);
        Vector2 corner = new Vector2(Mathf.Sign(moveDirection) * 0.5f, 0.5f);

        transform.position = LerpPosition(degree, rotationalDirection, corner);
        transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

        if (moveLerp == 1)
        {
            bool isFacingWall = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(moveDirection), 0.6f, 1 << LayerMask.NameToLayer("Ground"));

            if (isFacingWall)
            {
                moveDirection = -moveDirection;
                ResetRoll(RollDown);
            }
            else
            {
                ResetRoll(RollHorizontalFromUp);
            }
        }
    }

    void RollDown ()
    {
        float progress = Mathf.Min(Time.fixedDeltaTime / verticalRollTime, 1 - moveLerp);
        moveLerp += progress;
        float degree = moveLerp * 90;
        float rotationalDirection = -1 * Mathf.Sign(moveDirection);
        Vector2 corner = new Vector2(-1 * Mathf.Sign(moveDirection) * 0.5f, -0.5f);

        transform.position = LerpPosition(degree, rotationalDirection, corner);
        transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

        if (moveLerp == 1)
        {
            RollStop();
        }
    }

    void RollStop ()
    {
        isMoving = false;
        moveDirection = 0;
        rb.isKinematic = false;
        rb.simulated = true;
        rb.position = RoundVector(rb.position);
    }

    void ResetRoll (Action NextRoll)
    {
        moveLerp = 0;
        preMovementPosition = RoundVector(rb.position);
        Roll = NextRoll;
    }

    void RollStart (float direction, Action NextRoll)
    {
        isMoving = true;
        moveDirection = direction;
        rb.isKinematic = true;
        rb.simulated = false;
        ResetRoll(NextRoll);
    }
}
