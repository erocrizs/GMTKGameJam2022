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

    [SerializeField]
    float maxFallSpeed;

    [SerializeField]
    GameObject olive;

    bool isMoving = false;
    float moveLerp = 1;
    float moveDirection = 0;
    Vector2 preMovementPosition;
    Rigidbody2D rb;
    OliveDetector oliveDetector;
    Action Move;
    Collider2D oliveCollider;
    Collider2D dieCollider;
    EventObservable onStop;
    public EventObservable OnStop => onStop;

    bool IsGrounded => Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << LayerMask.NameToLayer("Ground"));

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
        oliveDetector = GetComponentInChildren<OliveDetector>();
        Move = RollHorizontal;
        oliveCollider = olive.GetComponent<BoxCollider2D>();
        dieCollider = GetComponent<BoxCollider2D>();
        onStop = new EventObservable();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Die"), LayerMask.NameToLayer("OliveGround"));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DieMoving"), LayerMask.NameToLayer("OliveGround"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            float movement = Input.GetAxis("Horizontal Die");
            isMoving = movement >= 0.01 || movement <= -0.01;
            if (isMoving && IsGrounded)
            {
                bool isFacingMudPole = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(movement), 1f, 1 << LayerMask.NameToLayer("MuddyPole"));
                bool isFacingWall = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(movement), 0.6f, 1 << LayerMask.NameToLayer("Ground"));

                if (isFacingMudPole)
                {
                    RollStart(movement, RollRotate);
                }
                else if (isFacingWall)
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
            else if (!isMoving && !IsGrounded)
            {
                RollStart(0, Fall);
                moveLerp = 1;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.isKinematic = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
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
            onStop.Play();
            if (IsGrounded)
            {
                MoveStop();
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

    void RollRotate ()
    {
        float progress = Mathf.Min(Time.fixedDeltaTime / horizontalRollTime, 1 - moveLerp);
        moveLerp += progress;
        float degree = moveLerp * 90;
        float rotationalDirection = -1 * Mathf.Sign(moveDirection);
        Vector2 corner = new Vector2(Mathf.Sign(moveDirection) * 0.5f, -0.5f);

        Vector2 newPosition = LerpPosition(degree, rotationalDirection, corner);
        newPosition.x = preMovementPosition.x;
        transform.position = newPosition;
        transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

        if (moveLerp == 1)
        {
            onStop.Play();
            if (IsGrounded)
            {
                MoveStop();
            }
            else
            {
                ResetRoll(RollDown);
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
            onStop.Play();
            if (IsGrounded)
            {
                MoveStop();
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.isKinematic = false;
                Move = Fall;
            }
        }
    }

    void Fall ()
    {
        if (IsGrounded)
        {
            onStop.Play();
            rb.isKinematic = true;
            MoveStop();
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        rb.velocity = new Vector2(0, Mathf.Max(rb.velocity.y, -maxFallSpeed));
    }

    void MoveStop ()
    {
        isMoving = false;
        moveDirection = 0;
        
        if (oliveDetector.IsOliveInside)
        {
            oliveDetector.SubscribeExitOnce(() => IgnoreOliveCollision(false));
        }
        else
        {
            IgnoreOliveCollision(false);
        }

        rb.position = RoundVector(rb.position);
    }

    void ResetRoll (Action NextRoll)
    {
        moveLerp = 0;
        preMovementPosition = RoundVector(rb.position);
        Move = NextRoll;
    }

    void RollStart (float direction, Action NextRoll)
    {
        isMoving = true;
        IgnoreOliveCollision(true);
        moveDirection = direction;
        ResetRoll(NextRoll);
    }

    void IgnoreOliveCollision (bool ignore)
    {
        Physics2D.IgnoreCollision(oliveCollider, dieCollider, ignore);
        this.gameObject.layer = ignore ? LayerMask.NameToLayer("DieMoving") : LayerMask.NameToLayer("Die");
    }
}
