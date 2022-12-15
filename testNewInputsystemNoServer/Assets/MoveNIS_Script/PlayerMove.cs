using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{

    [Header(" Public Parameter for multi-scrip")]
    public float timer = 0;

    #region Player move parameters
    [Header("Movement & Rotation Parameters")]
  
    [SerializeField] float speed = 5f;
    Rigidbody rgbd;
    Vector2 playerDirection;
    TestNewInputsystemNoServer playerInputsActions;
     float rotationSpeed = 0.5f;
     Quaternion rotationLookAt;
     Quaternion turnSpeed;


    #endregion

    #region Jump parameters
    [Header("Jump & Fall parameter")]
    [SerializeField] float jumpForce = 5f;
    //[SerializeField] bool isJumping = false;
    //[SyncVar][SerializeField] float jumpDuration = 0.03f;
    //[SyncVar] float lastJump;

    #endregion

   
    public void Awake()
    {
        
        /// gets player rigidbody
        rgbd = GetComponent<Rigidbody>();

        ///gets C# scripts (automatically generated thanks from bindings and schemes, option must be clicked on inspector)
        playerInputsActions = new TestNewInputsystemNoServer();

        ///enables Player Action maps
        playerInputsActions.Player.Enable();

        ///it binds the input and the method Jump
        playerInputsActions.Player.Jump.performed += Jump;
        
    }

    private void Update()
    {
        ///Timers are always useful
        timer += Time.deltaTime; 
    }

    public void Jump(InputAction.CallbackContext context) 
    {
        ///Permits Jump only one time when context is performed 
        ///if not used  jump happens 3 times AKA 3 contexts:
        ///start 
        ///performed 
        ///finished  
        if (context.performed)
        {
            rgbd.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    { 

        ///Gets Vector2 from input system
        playerDirection = playerInputsActions.Player.Move.ReadValue<Vector2>();

        ///Vector3 for correct movement (in Vector 2 X => up/down)  
        rgbd.velocity = new Vector3(playerDirection.x * speed, rgbd.velocity.y, playerDirection.y * speed);


        if (playerDirection != new Vector2(0,0))
        {
            //to rotate body to wanted point relative to camera 
            rotationLookAt = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.y));
            rgbd.MoveRotation(rotationLookAt);

            //to regulate the speed of rotation
            turnSpeed = Quaternion.RotateTowards(rgbd.rotation, rotationLookAt, rotationSpeed * Time.fixedDeltaTime);
            rgbd.rotation = turnSpeed;
        }


    }
    
}
