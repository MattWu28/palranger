using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]protected float moveSpeed, chaseDist;
    Transform targetPosition;
    private enum State{idle,Chase};
    State currentState;
    public Animator animator;
    private Vector3 Scale;
    protected Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        currentState = State.idle;
        targetPosition = GameObject.Find("Player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        if(currentState == State.idle){ 
            animator.SetBool("isMove",false);
            if((transform.position-targetPosition.position).magnitude > chaseDist){
                currentState = State.Chase;
            }
        }
        else{
            Vector3 direction = (targetPosition.position - transform.position).normalized;
            moveDirection = direction;
            animator.SetBool("isMove",true);
            Move();
            Debug.Log(transform.position);
            Debug.Log(targetPosition.position);
            if((transform.position-targetPosition.position).magnitude < chaseDist){
                currentState = State.idle;
                rigidBody.velocity = new Vector2(0,0);
            }
            if((transform.position-targetPosition.position).magnitude > 1.3)
            {
                
                transform.position = new Vector2(targetPosition.position.x-0.07f,targetPosition.position.y);
            }
        }
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        if(rigidBody.velocity.x > 0){
            spriteRenderer.flipX = true;
        }
        else{
            spriteRenderer.flipX = false;
        }
    }

}
