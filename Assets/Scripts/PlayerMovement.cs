using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;

    private bool isMoving;
    
    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        
        animator = GetComponent<Animator>(); // Applies Idle animations configured in the Animator tool in Unity
        
    }

    private void Update()
    {
        
        if (!isMoving)
        {
            
            input.x = Input.GetAxisRaw("Horizontal"); // if input different from zero then run something!
            input.y = Input.GetAxisRaw("Vertical");
            
            Debug.Log("This is input.x " + input.x);
            Debug.Log("This is input.y " + input.y);
            
            if (input.x != 0) input.y = 0; // Prevents diagonal movement
            
            if (input != Vector2.zero)
            {
                
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);
                
                var targetPos = transform.position; // Changes the objects transform value
                targetPos.x += input.x; // adds input to the x value coordinate
                targetPos.y += input.y; // adds input to the y value coordinate
                
                StartCoroutine(Move(targetPos)); // will be running constantly in the game
                
            }

        }

        animator.SetBool("isMoving", isMoving); // Applies Walking animations configured in the Animator tool in Unity
        
    }

    IEnumerator Move(Vector3 targetPos)
    {

        isMoving = true;
        
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) // If anything bigger than zero happened, then the user has moved!
        {
            
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed  * Time.deltaTime); // The original transform position and then moving to the target position
            yield return null;
            
        }
        
        transform.position = targetPos;
        
        isMoving = false;
        
    }

}
