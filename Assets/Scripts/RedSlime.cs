using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Monster
{
    Transform targetPosition;
    public enum State{idle,chase,battling};
    public State currentState;
    public Animator animator;
    private Vector3 Scale;
    public Comp_Manager compManager;

    protected override void CustomStart()
    {
        currentState = State.idle;
        targetPosition = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if(currentState == State.battling){
            isChasing = false;
            if(timer <= 0f){
                GenerateRandomDirection();
            }
            animator.SetBool("isMove",true);
            timer -= Time.deltaTime;
        }else{
            float distToPlayer = Vector3.Distance(transform.position, targetPosition.position);
            // If the player is within chaseDist, chase the player
            if(distToPlayer <= chaseDist){
                isChasing = true;
            }else{
                isChasing = false;
            }

            if(isChasing){
                Vector3 direction = (targetPosition.position - transform.position).normalized;
                moveDirection = direction;
                animator.SetBool("isMove",true);
            }
            else{
                moveDirection = Vector3.zero;
                animator.SetBool("isMove",false);
            }
        }
    }

    protected override void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        if(rigidBody.velocity.x > 0){
            spriteRenderer.flipX = true;
        }
        else{
            spriteRenderer.flipX = false;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("destroyed");
    }
}

