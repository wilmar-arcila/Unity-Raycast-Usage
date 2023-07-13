using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask rayMask;

    float fuerzaSalto          = 50;   // x veces la masa del personaje
    float fuerzaImpulso        = 500;  // Fuerza en Newtons
    float fuerzaDesplazamiento = 30;   // Fuerza en Newtons

    private Rigidbody2D rb2d;       // Variable para mantener la referencia al componente Rigidbody2D
    
    bool enElPiso   = false; // Bandera que verifica que el personaje ha tocado el piso
    bool enElMuroL  = false; // Bandera que verifica que el personaje ha tocado el muro izquierdo
    bool enElMuroR  = false; // Bandera que verifica que el personaje ha tocado el muro derecho

    private RaycastHit2D HitL, HitR;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Se dibujan los rayos solo para DEPURACIÓN
        Debug.DrawRay(transform.position, 0.35f*transform.right, Color.red);
        Debug.DrawRay(transform.position, -0.35f*transform.right, Color.red);

        HitR = Physics2D.Raycast(transform.position, transform.right, 0.35f, rayMask);
        HitL = Physics2D.Raycast(transform.position, transform.right, -0.35f, rayMask);

        // Movimiento a la izquierda o a la derecha
        if(Input.GetKey("right") && enElPiso){
            rb2d.AddForce(new Vector2(fuerzaDesplazamiento, 0));
        }
        else if(Input.GetKey("left") && enElPiso){
            rb2d.AddForce(new Vector2(-fuerzaDesplazamiento, 0));
        }

        // Implementación del salto
        if(Input.GetKeyDown("space") && enElPiso){
            rb2d.AddForce(new Vector2(0, -fuerzaSalto*Physics2D.gravity[1]*rb2d.mass));
            Debug.Log("JUMP");
            enElPiso = false;
        }

        // Implementación del salto del muro
        if(Input.GetKeyDown("space") && (enElMuroL || enElMuroR)){
            Debug.Log("WALL JUMP");
            if(enElMuroL){
                rb2d.AddForce(new Vector2(fuerzaImpulso, -0.5f*fuerzaSalto*Physics2D.gravity[1]*rb2d.mass));
                enElMuroL = false;
            }
            else{
                rb2d.AddForce(new Vector2(-fuerzaImpulso, -0.5f*fuerzaSalto*Physics2D.gravity[1]*rb2d.mass));
                enElMuroR = false;
            }            
        }

        // Personaje tocando un muro
        if(HitL.collider != null){ // izquierdo
            Debug.Log("WALL LEFT");
            rb2d.gravityScale = 0.1f;
            enElMuroL = true;
            enElPiso = false;
        }
        else if(HitR.collider != null){ //derecho
            Debug.Log("WALL RIGHT");
            rb2d.gravityScale = 0.1f;
            enElMuroR = true;
            enElPiso = false;
        }

        // Personaje en el aire
        if((HitL.collider == null) && (HitR.collider == null) && !enElPiso){
            Debug.Log("AIRE");
            enElMuroL = false;
            enElMuroR = false;
            rb2d.gravityScale = 1f;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.tag == "Ground"){
            Debug.Log("GROUND");
            enElPiso = true;
            enElMuroL = false;
            enElMuroR = false;
        }
        else if(collision.transform.tag == "Obstaculo"){
            enElPiso = true;
        }
    }
}
