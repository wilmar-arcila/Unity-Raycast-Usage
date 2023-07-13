using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private float rayLength = 1;
    [SerializeField] private LayerMask rayMask;
    [SerializeField] private bool rayAll = false;

    private List<RaycastHit2D> Hit;
    private List<GameObject> targetObjects = new List<GameObject>();
    private List<GameObject> targetObjects_old = new List<GameObject>();


    void Update()
    {
        Debug.DrawRay(transform.position, rayLength*transform.right, Color.red);

        if(rayAll){
            Hit = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position, transform.right, rayLength, rayMask));
        }
        else{
            Hit = new List<RaycastHit2D>();
            RaycastHit2D h = Physics2D.Raycast(transform.position, transform.right, rayLength, rayMask);
            if(h.collider != null){
                Hit.Add(h);
            }
        }
        
        /* Debug.Log("Hit L: " + Hit.Count);
        Debug.Log("to L: " +targetObjects.Count);
        Debug.Log("to_old L: " +targetObjects_old.Count); */
        if(Hit.Count != 0){
            Debug.Log("**HIT**");
            foreach (RaycastHit2D hit in Hit){
                Debug.Log("Tag: " + hit.transform.tag);
                Debug.Log("Nombre: " + hit.transform.name);
                Debug.Log("Distancia: " + hit.distance);
                Debug.Log("Punto de impacto: " + hit.point);
                hit.transform.gameObject.GetComponent<SpriteRenderer>().color=Color.red;
                targetObjects.Add(hit.transform.gameObject);
            }
        }

        foreach (GameObject object_old in targetObjects_old){
            if(!targetObjects.Contains(object_old)){
                object_old.GetComponent<SpriteRenderer>().color=Color.white;
            }
        }

        targetObjects_old = new List<GameObject>(targetObjects);
        targetObjects.Clear();
    }
}
