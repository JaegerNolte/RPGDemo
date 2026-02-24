using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

/*
 
    This is for educational purposes to learn core unity concepts and object-oriented programming in C#
    This project demo will provide the basics for RPG development and provide opportunity to experiment with character
    and role-playing environments
    
*/
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;

    private bool isMoving;
    
    private Vector2 input;

    private Animator animator;
    
    public LayerMask solidObjectsLayer; // scene layer referencing the tile layer level for level editing

    public LayerMask interactableLayer;
    
    public LayerMask battleLayer;
    private void Awake()
    {
        
        animator = GetComponent<Animator>(); // Applies Idle animations configured in the Animator tool in Unity
        
    }

    public void HandleUpdate()
    {
        
        if (!isMoving)
        {
            
            input.x = Input.GetAxisRaw("Horizontal"); // if input different from zero then run something!
            input.y = Input.GetAxisRaw("Vertical");
            
            // Debug.Log("This is input.x " + input.x);
            // Debug.Log("This is input.y " + input.y);
            
            if (input.x != 0) input.y = 0; // Prevents diagonal movement
            
            if (input != Vector2.zero)
            {
                
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                
                var targetPos = transform.position; // Changes the objects transform value
                targetPos.x += input.x; // adds input to the x value coordinate
                targetPos.y += input.y; // adds input to the y value coordinate
                
                if (isWalkable(targetPos))
                    StartCoroutine(Move(targetPos)); // will be running constantly in the game
                
            }

        }

        animator.SetBool("isMoving", isMoving); // Applies Walking animations configured in the Animator tool in Unity
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            
            Interact();
            
        }
        
    }

    void Interact()
    {
        
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position +  facingDir;
        
        
        // this visualizes the intersect point of the facing direction to interact with objects        
        // Debug.DrawLine(transform.position, interactPos, Color.red, 1f);
        
        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
        if (collider != null)
        {
            
            // If it is a collider -> get the component -> if its interactable -> execute Interact
            collider.GetComponent<Interactable>()?.Interact();

        }
        
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

        CheckForEncounters();

    }

    private bool isWalkable(Vector3 targetPos) // applies the player collision barrier
    {

        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            
            return false;
            
        }
        
        return true;
        
    }

    private void CheckForEncounters()
    {

        if (Physics2D.OverlapCircle(transform.position, 0.1f, battleLayer) != null)
        {

            if (Random.Range(1, 100) <= 50)
            {
                
                Debug.Log("A battle has started");
                
            }

        }

    }

}
