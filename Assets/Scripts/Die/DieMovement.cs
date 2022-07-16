using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMovement : MonoBehaviour
{
    [SerializeField]
    float rollTime;

    bool isMoving = false;
    bool isGrounded = false;

    float moveLerp = 1;
    float moveDirection = 0;
    Vector2 preMovementPosition;
    Rigidbody2D rb;

    private Vector2 RoundVector (Vector2 v)
    {
        float x = Mathf.RoundToInt(v.x);
        float y = Mathf.RoundToInt(v.y);
        return new Vector2(x, y);
    }

    private Vector2 LerpPosition (float degree, float direction)
    {
        float directionSign = Mathf.Sign(direction);
        Vector2 cornerToCenter = Quaternion.Euler(0, 0, directionSign * 45) * Vector2.up * (Mathf.Sqrt(2) / 2);
        Vector2 rotatedCornerToCenter = Quaternion.Euler(0, 0, -1f * degree * directionSign) * cornerToCenter;
        Vector2 corner = preMovementPosition + new Vector2(directionSign * 0.5f, -0.5f);
        Debug.Log(preMovementPosition + " " + rotatedCornerToCenter + " " + corner + " " + (corner + rotatedCornerToCenter));
        return corner + rotatedCornerToCenter;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, 1 << LayerMask.NameToLayer("Ground"));
        float movement = Input.GetAxis("Horizontal");
        if (!isMoving && isGrounded)
        {
            isMoving = movement >= 0.01 || movement <= -0.01;
            if (isMoving)
            {
                Debug.Log("Moving");
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
            float degree = moveLerp * 90;
            transform.position = LerpPosition(degree, moveDirection);
            transform.Rotate(0, 0, progress * -90 * Mathf.Sign(moveDirection));

            if (moveLerp == 1)
            {
                Debug.Log("Stop");
                isMoving = false;
                rb.isKinematic = false;
                rb.simulated = true;
                rb.position = RoundVector(rb.position);
            }
        }
        else
        {
            rb.position = new Vector2(Mathf.RoundToInt(rb.position.x), rb.position.y);
        }
    }
}
