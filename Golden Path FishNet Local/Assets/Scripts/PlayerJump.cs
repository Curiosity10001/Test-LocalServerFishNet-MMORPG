using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMove;

public class PlayerJump : NetworkBehaviour
{ /*
    #region Multi use parameters
    [Header("Parameter for multi-scrip")]
    // if subscription to time manager  is done = true;
    //the subscription is so the method can syncronise data between base.timeManger and Server timeManager
    //by comparing the client data reception  tick and server intended application tick
    bool timerSubsciption = false;
    #endregion

    #region Jump parameters
    [Header("Jump & Fall parameter")]
    PlayerInputsActions playerInputsActions;
    Rigidbody playerRigidbody;
    [SerializeField] float jumpForce = 5f;
    //[SerializeField] bool isJumping = false;
    //[SerializeField] float jumpDuration = 0.03f;
    //float lastJump;

    public struct ServerAuthorizedData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;

        /// <summary>
        ///  Information for verification by Server;
        ///  In case of divergence this is the data imposed on client.
        /// </summary>
        public ServerAuthorizedData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
        {
            Position.y = position.y;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }

    }

    #endregion
    void TimeManagerSubscription(bool subscribed)
    {
        //if client time manager not yet activated, it cannot be subscribed to.
        if (TimeManager == null) return;

        //If already subscribed/unsubscribed no need to do it again.
        if (subscribed == timerSubsciption) return;

        // Instills value of timerSucscription to subscribed
        timerSubsciption = subscribed;

        //OnTick(equivalent to fixedupdates) is the moment before physics event happens.
        //OnPostTick is the moment after physics event happens.
        if (subscribed)
        {
            //synchronises info if client tick is earlier than server intended application tick
            //(12/2022 => current comprehension)
            TimeManager.OnTick += TimeManager_OnTick;
            TimeManager.OnPostTick += TimeManager_OnPostTick; 
        }
        else
        {
            //synchronises info if client tick is later than server intended application tick
            //(12/2022 => current comprehension)
            TimeManager.OnTick -= TimeManager_OnTick;
            TimeManager.OnPostTick -= TimeManager_OnPostTick;
        }
    }
    private void OnDestroy()
    {
        ///on destroy unsubscribe to timer reconciliation
        TimeManagerSubscription(false);
    }

    private void TimeManager_OnTick()
    {
        //Only owner must be able to do the following.
        if (IsOwner)
        {
            //In this case it imposes the default settings(last reconciled data) on client.
            ReconcilePhysicsData(default, false);

            Jump(context.performed);

        }
        if (IsServer)
        {
            //Simulation on server = Does the same movement on server side this time to keep the synchronisation (if correct)
            Move(default, true);
            // Jump();

            //Gather data after simulation on Server
            ServerAuthorizedData data = new ServerAuthorizedData(transform.position, transform.rotation, playerRigidbody.velocity, playerRigidbody.angularVelocity);

            //data reconciliation on client side with Server data
            ReconcilePhysicsData(data, true);
        }
    }

    /// <summary>
    /// Post event reception information processed in server in the order following:
    /// (used because combination of add force for jump and transfom for move so Ontick is not sufficient,
    /// it also helps reconfirm data in the tick following the physics simulation)
    /// 1 - Gather simulation data by server.
    /// 2 - reconcile client data with server data .
    /// Simulation data should be the same if the synchronisation is done correctly if not server data is imposed
    /// </summary>
   private void TimeManager_OnPostTick()
    {
        //Gather data after simulation on Server
        ServerAuthorizedData data= new ServerAuthorizedData(transform.position,transform.rotation,playerRigidbody.velocity,playerRigidbody.angularVelocity);
    
        //data reconciliation on client side with Server data
        ReconcilePhysicsData(data, true);
    }

    public override void OnStartServer()
    {
        //works if a server is connected and contains this script
        base.OnStartServer();

        //the method calling it is One of the events that enclenches client subscription
        TimeManagerSubscription(true);

        Debug.Log(TimeManager);
        //gets player rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

        Debug.Log(timerSubsciption + playerRigidbody.name);
    }

    public override void OnStartClient()
    {
        //Works if a client is connected
        base.OnStartClient();

        //the method calling it is One of the events that enclenches client subscription
        TimeManagerSubscription(true);

        //stop reading script if no client connected and is not ownerof object on which modification is wanted
        if (!base.IsClient)
            return;

        //gets player rigidbody
        playerRigidbody = GetComponent<Rigidbody>();

        //gets C# scripts (automatically generated from bindings and schemes, option must be clicked on inspector)
        playerInputsActions = new PlayerInputsActions();

        //enables Player Action maps
        playerInputsActions.Player.Enable();

        //it binds the input and the method
        playerInputsActions.Player.Jump.performed += Jump;
    }

    [Replicate]
    private void Jump(InputAction.CallbackContext context)
    {
        //Permits Jump only one time when context is performed 
        //if not used  jump read internally 3 times AKA 3 contexts:
        //start 
        //performed 
        //finished  
        if (context.performed)
            Debug.Log("jump");
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    [Reconcile]
    private void ReconcilePhysicsData(ServerAuthorizedData data, bool asServer)
    {
        transform.position.y = data.Position.y;
        transform.rotation = data.Rotation;
        playerRigidbody.velocity = data.Velocity;
        playerRigidbody.angularVelocity = data.AngularVelocity;
    }*/
}
