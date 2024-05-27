using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    public int health;
    [SerializeField]protected float moveSpeed;
    public string opponentTag;
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

    private void OnCollisionEnter(Collision collision){
        Debug.Log("Collision");
        if(collision.gameObject.tag=="Player")
            {
                Debug.Log("Battle!");
                Destroy(gameObject);
            }
    }
    
    void FixedUpdate(){
        
    }

    public void takeDamage(){
        health = health - 1;
        if(health<=0){
            Destroy(gameObject);
        }
    }

    void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }


}