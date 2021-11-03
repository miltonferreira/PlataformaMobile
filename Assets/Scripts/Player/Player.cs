using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private Animator anim;

    private InputController inputController;

    // Movimento Horizontal -------------------------------------------------------

    [SerializeField]private float _speed;

    private float _movement;

    // Controla pulos do player ---------------------------

    [SerializeField]private float _jumpForce;

    [SerializeField]private int _canJumps;  // total de pulos permitidos
    private int _totalJumps;                // total de vezes para player pular

    private bool _initJump;

    // Verifica se colide com o chao ----------------------

    [SerializeField]private Transform[] groundCheck;    // posicao dos objs que vao checar colisao com chao

    [SerializeField]private LayerMask groundLayer;      // indica a layer do chao

    [SerializeField]private bool isGrounded;            // controla se esta colidindo com chao

    private Collider2D[] collider_1, collider_2;        // colliders de interação com o chão

    const float groundCheckRadius = 0.036f;             // tamanho do circulo no pé do player


    
    // Start is called before the first frame update
    void Awake(){
        rb2D = GetComponent<Rigidbody2D>();
        inputController = GetComponent<InputController>();
        anim = GetComponent<Animator>();

        _totalJumps = _canJumps;
    }

    // Update is called once per frame
    void Update()
    {
        checkGround();
        move();
        jump();
    }

    void LookAt(){
        if(_movement > 0f){
            // direita
            transform.localScale = new Vector2(1f,1f);
        }else if(_movement < 0f){
            // esquerda
            transform.localScale = new Vector2(-1f,1f);
        }
    }

    void move(){
        _movement = inputController.horizontal;
        rb2D.velocity = new Vector2(_movement*_speed, rb2D.velocity.y);

        anim.SetFloat("xMove", rb2D.velocity.x);

        LookAt();
    }

    void checkGround(){
        collider_1 = Physics2D.OverlapCircleAll(groundCheck[0].position, groundCheckRadius, groundLayer);
        collider_2 = Physics2D.OverlapCircleAll(groundCheck[1].position, groundCheckRadius, groundLayer);

        if(collider_1.Length > 0 || collider_2.Length > 0){
            isGrounded = true;
        }else{
            isGrounded = false;
        }
    }

    void jump(){

        anim.SetBool("jump", !isGrounded);
        anim.SetFloat("yMove", rb2D.velocity.y);

        if(isGrounded && !_initJump){
            _totalJumps = _canJumps;    // seta quantidade de pulos
        }

        if(!isGrounded && _totalJumps == _canJumps && rb2D.velocity.y < -2.5f){
            _totalJumps --;
        }

        if(inputController.jump){
            if(_totalJumps > 0){
                rb2D.velocity = Vector2.up * _jumpForce;
                _totalJumps--;  // subtrai 1 pulo
                StartCoroutine(IEAddDelayJump());
            }
            
        }
    }

    IEnumerator IEAddDelayJump(){
        _initJump = true;
        yield return new WaitForSeconds(0.3f);
        _initJump = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck[0].position, groundCheckRadius);
        Gizmos.DrawSphere(groundCheck[1].position, groundCheckRadius);
    }


}
