
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{

    [SerializeField] private float radius; 
    [SerializeField] private PlayerMovement player; 
    [SerializeField] private LayerMask whatIsGround; 

    private bool canDetected;

    private void Update(){
        if(canDetected)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround); 
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            canDetected = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision){
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")){
            canDetected = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
