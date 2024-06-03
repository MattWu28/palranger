using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]protected float moveSpeed;
    public static Player Instance;
    Vector2 movement;
    public Animator animator;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
    

    void Update(){

        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        
        
        if(movement.x == 1 || movement.x == -1 || movement.y == 1 || movement.y == -1){
            animator.SetFloat("LastHorizontal", movement.x);
            animator.SetFloat("LastVertical", movement.y);
        }
    
    }

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }
}