using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : NetworkBehaviour
{

    #region Multi use parameters
    [Header("Parameter for multi-scrip")]
    // if subscription to time manager  is done = true;
    //the subscription is so the method can syncronise data between base.timeManger and Server timeManager
    //by comparing the client data reception  tick and server intended application tick
    bool timerSubsciption = false;
    #endregion

    #region Player move parameters
    [Header("Movement & Rotation Parameters")]
    Rigidbody playerRigidbody;
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed =15f; 
    Quaternion turnSpeed;

    PlayerInputsActions playerInputsActions;
    public struct ClientMoveRequest
    {   
        public Vector2 InputData;
        public Vector3 PlayerDirection;
        public Quaternion RotationLookAt;

        /// <summary>
        /// Information replicated and sent by client to server.
        /// </summary>
        public ClientMoveRequest(Vector2 inputData,Vector3 direction, Quaternion rotationLookAt)
        {
            InputData = inputData;
            PlayerDirection = direction;
            RotationLookAt = rotationLookAt;
        }

    }
    
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
        public ServerAuthorizedData(Vector3 position, Quaternion rotation,Vector3 velocity, Vector3 angularVelocity)
        {
            Position = position;
            Rotation= rotation;
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
           // TimeManager.OnPostTick += TimeManager_OnPostTick; =>Not needed as using transfom in movement and not add force
        }
        else
        {
            //synchronises info if client tick is later than server intended application tick
            //(12/2022 => current comprehension)
            TimeManager.OnTick -= TimeManager_OnTick;
            //TimeManager.OnPostTick -= TimeManager_OnPostTick;
        }
    }
    private void OnDestroy()
    {
        ///on destroy unsubscribe to timer reconciliation
        TimeManagerSubscription(false);
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

        Debug.Log(timerSubsciption  + playerRigidbody.name);
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

            //Gets C# scripts (automatically generated from bindings and schemes, option must be clicked on inspector)
            playerInputsActions = new PlayerInputsActions();

            //Enables Player Action maps
            playerInputsActions.Player.Enable();

    }

    private void TimeManager_OnTick()
    {
        //Only owner must be able to do the following.
        if (IsOwner)
        {
            //In this case it imposes the default settings(last reconciled data) on client.
            ReconcilePhysicsData(default, false);

            //Gather movement data to use in the following method for client & Server.
            ClientMoveRequest info;

            // Calls on the movement data for processing
            MoveData(out info);

            // Movement (simulation) locally for client with gathered info (client prediction)
            Move(info, false);
        }
        if(IsServer)
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
   /*private void TimeManager_OnPostTick()
    {
        //Gather data after simulation on Server
        ServerAuthorizedData data= new ServerAuthorizedData(transform.position,transform.rotation,playerRigidbody.velocity,playerRigidbody.angularVelocity);
    
        //data reconciliation on client side with Server data
        ReconcilePhysicsData(data, true);
    }*/

    //  out : Memo of first use
    //  out parameter doesn’t require the variables to be initialized before it passed to the method.
    //  But before it returns a value to the calling method, the variable must be initialized in the called method.
    public void MoveData(out ClientMoveRequest info )
    {
        info = default;

        //Gets Vector2 from input system
        Vector2 inputData= playerInputsActions.Player.Move.ReadValue<Vector2>();
        Vector3 direction = new Vector3 (inputData.x, playerRigidbody.velocity.y, inputData.y);

        if (inputData == Vector2.zero) return;

        Quaternion rotationLookAt = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //Updates direction infos while disregarding older one 
        //every new physics event on tick registered
        info = new ClientMoveRequest(inputData,direction,rotationLookAt);
    }

    // Kind of replaces serverRPC : in this case it is a replica of the request  
    // to check if the calculation done on client side are matching with the server data
    // In all cases (correct/incorrect client prediction) Server Data in this case wins 
    // in case of cheat sever imposes it s calculations 
    [Replicate]
    private void Move(ClientMoveRequest info,bool asServer, bool replaying = false)
    {
        //Vector3 for correct movement (in Vector 2 X => up/down)  
        playerRigidbody.velocity = new Vector3(info.PlayerDirection.x*speed,info.PlayerDirection.y, info.PlayerDirection.z*speed);

        //Turn only if movement detected , and stay looking at the last direction registered
        if ( info.InputData != new Vector2(0, 0))
        {
            //To rotate towards movement direction and  regulate the speed of rotation.
            turnSpeed = Quaternion.RotateTowards(playerRigidbody.rotation, info.RotationLookAt, rotationSpeed);
            playerRigidbody.rotation = turnSpeed;
        }
    }

    [Reconcile]
    private void ReconcilePhysicsData(ServerAuthorizedData data, bool asServer)
    {
        transform.position = data.Position; 
        transform.rotation = data.Rotation;
        playerRigidbody.velocity = data.Velocity;
        playerRigidbody.angularVelocity = data.AngularVelocity;
    }
}
