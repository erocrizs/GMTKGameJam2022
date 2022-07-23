using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveAnimator : MonoBehaviour
{
    public bool IsRunning;
    public bool IsRising;
    public bool IsFalling;
    public bool IsGrounded;

    Animator animator;
    SpriteRenderer sr;
    Action ChangeState;
    string currentKey;
    string idleKey = "OliveIdle";
    string runKey = "OliveRun";
    string riseKey = "OliveJumpRise";
    string fallKey = "OliveFall";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        Idle();
    }

    public void SetDirection(float direction)
    {
        if (direction == 0)
        {
            return;
        }

        sr.flipX = direction < 0;
    }

    void Update ()
    {
        ChangeState();
    }

    void Run ()
    {
        if (currentKey == runKey)
        {
            return;
        }

        ChangeState = RunningChangeState;
        animator.Play(runKey);
        currentKey = runKey;
    }

    void Rise ()
    {
        if (currentKey == riseKey)
        {
            return;
        }

        ChangeState = RisingChangeState;
        animator.Play(riseKey);
        currentKey = riseKey;
    }

    void Fall ()
    {
        if (currentKey == fallKey)
        {
            return;
        }

        ChangeState = FallingChangeState;
        animator.Play(fallKey);
        currentKey = fallKey;
    }

    void Idle ()
    {
        if (currentKey == idleKey)
        {
            return;
        }

        ChangeState = IdleChangeState;
        animator.Play(idleKey);
        currentKey = idleKey;
    }

    void IdleChangeState ()
    {
        if (IsGrounded)
        {
            if (IsRunning)
            {
                Run();
            }
        }
        else if (IsRising)
        {
            Rise();
        }
        else if (IsFalling)
        {
            Fall();
        }
    }

    void RunningChangeState ()
    {
        if (IsRising)
        {
            Rise();
        }
        else if (IsFalling && !IsGrounded)
        {
            Fall();
        }
        else if (!IsRunning)
        {
            Idle();
        }
    }

    void RisingChangeState ()
    {
        if (!IsRising)
        {
            if (!IsGrounded)
            {
                Fall();
            }
            else if (IsRunning)
            {
                Run();
            }
            else
            {
                Idle();
            }
        }
    }

    void FallingChangeState ()
    {
        if (IsGrounded)
        {
            if (IsRising)
            {
                Rise();
            }
            else if (IsRunning)
            {
                Run();
            }
            else
            {
                Idle();
            }
        }
    }
}
