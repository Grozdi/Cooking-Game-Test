using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayersMask;
    private bool isWalking;
    private Vector3 lastInteractDir;
 private void Update()
    {
        HandleMovement(); 

    }

    public bool IsWalking() 
    {
        return isWalking; 
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float interactDistance = 2f;
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        

        

      if  (Physics.Raycast(transform.position, lastInteractDir,out RaycastHit raycastHit, interactDistance,countersLayersMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                clearCounter.Interact();
            }
            
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            Vector3 moveDirx = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

            if (canMove)
            {
                moveDir = moveDirx;
            }
            else
            {
                Vector3 moveDirz = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirz, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirz;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 12f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
}
