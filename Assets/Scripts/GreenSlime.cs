using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{

    Transform targetPosition;
    private enum State{idle,Chase};
    State currentState;
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
        if (currentState == State.idle){
            animator.SetBool("isMove",false);
            if((transform.position-targetPosition.position).magnitude < chaseDist){
                currentState = State.Chase;
            }
        }
        else{
            Vector3 direction = (targetPosition.position - transform.position).normalized;
            moveDirection = direction;
            animator.SetBool("isMove",true);
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



    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag=="Player")
            {
                Destroy(gameObject);
            }
    }

    private void OnDestroy()
    {
        if(gameObject.scene.isLoaded) //Was Deleted
        {
            compManager.updateCompanion("GreenSlime");
        }
    }
}

