using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    [SerializeField]protected float moveSpeed, chaseDist;
    protected Vector2 moveDirection;
    protected float minMoveDuration = 1f;
    protected float maxMoveDuration = 3f;
    protected float timer = 0f;
    public string opponentTag;
    public bool isChasing = false;

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        CustomStart();
    }

    abstract protected void CustomStart();

    abstract protected void Move();

    void FixedUpdate(){Move();}

    public void GenerateRandomDirection()
    {
        moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        timer = Random.Range(minMoveDuration, maxMoveDuration);
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag=="Player") {
            GameManager.instance.LoadBattle(gameObject);
        }
    }
}