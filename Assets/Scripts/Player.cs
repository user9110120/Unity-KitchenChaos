using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IKitchenObjectParent
{

    public static Player Instance { get; private set; }

    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEvendArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangedEvendArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    //设为private但是可在UI编辑 [SerializeField]
    //Speed 
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitChenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There are more than one player instance"); 
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) { return; }

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) { return; }

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
       
    }

    private void Update()
    {
        HandleMovement();
        HandleInteration();

    }

    public bool IsWalking() { 
        return isWalking;
    }

    private void HandleInteration()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Walk   移动
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //记录最后的方向
        if (moveDir != Vector3.zero) {   
            lastInteractDir = moveDir;
        };

        //激光检测前方物体
        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has ClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);    
                }
            }
            else
            {
                SetSelectedCounter(null);              
            }

        }
        else
        {
            SetSelectedCounter(null);           
        }
        
           
    }
    private void HandleMovement()
    {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Walk   移动
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);




        //Collisions  碰撞检测
        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            //cannot move towards moveDir

            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can only move on x
                moveDir = moveDirX;
            }
            else
            {
                //cannot move on x

                //Attemp only z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //can only move on z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move anywhere
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        //转动人物朝向
        //移位置用 Lerp，转角度用 Slerp
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangedEvendArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitChenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject == null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
