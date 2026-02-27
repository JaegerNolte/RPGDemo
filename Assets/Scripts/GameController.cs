using System;
using UnityEngine;



public enum GameState
{
    
    FreeRoam,
    Dialog,
    Battle
    
}

public class GameController : MonoBehaviour
{
    
    // allows this value to be visible to the inspector
    [SerializeField] PlayerMovement playerMovement;
    
    GameState state;

    private void Start()
    {

        DialogManager.Instance.OnShowDialog += () =>
        {

            state = GameState.Dialog;

        };
        
        DialogManager.Instance.OnHideDialog += () =>
        {
            
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;

        };

    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            
            playerMovement.HandleUpdate();
            
        } else if (state == GameState.Dialog)
        {

            DialogManager.Instance.HandleUpdate();

        } else if (state == GameState.Battle)
        {
            
            
            
        }
    }
    
}

    
   


