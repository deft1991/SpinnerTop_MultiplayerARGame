using Photon.Pun;
using UnityEngine;

public class MySynchronizationScript : MonoBehaviour, IPunObservable
{
    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceIsGreaterThan = 1.0f;

    private Rigidbody _rigidbody;
    private PhotonView _photonView;
    private Vector3 _networkPosition;
    private Quaternion _networkRotation;
    private float _distance;
    private float _angleDiff;
    private GameObject _battleArenaGameObject; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
        
        _networkPosition = new Vector3();
        _networkRotation = new Quaternion();
        
        _battleArenaGameObject = GameObject.Find("BattleArena"); // find battleArena gameObject in runtime
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            // move from current to network position on distance
            // PhotonNetwork.SerializationRate) - numbers per second that OnPhotonSerializedView called. Default is 10
            // this method calls 10 times per second
            var serializationTimesPerSecond = 1 / PhotonNetwork.SerializationRate;
            SetRigidBodyPosition(serializationTimesPerSecond);
            SetRigidBodyRotation(serializationTimesPerSecond);
        }
    }

    private void SetRigidBodyPosition(int serializationTimesPerSecond)
    {
        // bigger distance --> faster moving
        _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, _networkPosition,
            _distance * serializationTimesPerSecond);
    }

    private void SetRigidBodyRotation(int serializationTimesPerSecond)
    {
        // bigger angle --> faster rotation
        _rigidbody.rotation =
            Quaternion.RotateTowards(_rigidbody.rotation, _networkRotation,
                _angleDiff * serializationTimesPerSecond);
    }

    /**
     * Called several times per second by Photon
     * Every call we send and receive data of palyers
     * such as rigidBody position, rotation and velocity
     */
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Then Photon View is mine and I am the one who controls the player.
            // should send position, velocity ect. to the other players
            stream.SendNext(_rigidbody.position - _battleArenaGameObject.transform.position); // send position in world, without battleArena position 
            stream.SendNext(_rigidbody.rotation);

            if (synchronizeVelocity)
            {
                stream.SendNext(_rigidbody.velocity);
            }

            if (synchronizeAngularVelocity)
            {
                stream.SendNext(_rigidbody.angularVelocity);
            }
        }
        else // reading stream
        {
            // Called on my players gameObject that exists in remote player`s game
            _networkPosition = (Vector3) stream.ReceiveNext() + _battleArenaGameObject.transform.position; // add to world position our battle arena position
            _networkRotation = (Quaternion) stream.ReceiveNext();

            // if teleport is enabled 
            if (isTeleportEnabled)
            {
                TryToMakeTeleport();
            }

            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                // calculate time lag
                float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    SynchronizeVelocity(stream, lag);
                }

                if (synchronizeAngularVelocity)
                {
                    SynchronizeAngularVelocity(stream, lag);
                }
            }
        }
    }

    private void TryToMakeTeleport()
    {
        // check that distance greater than tele portIfDistanceIsGreaterThan
        if (Vector3.Distance(_rigidbody.position, _networkPosition) > teleportIfDistanceIsGreaterThan)
        {
            _rigidbody.position = _networkPosition;
        }
    }

    private void SynchronizeVelocity(PhotonStream stream, float lag)
    {
        // receive and set velocity 
        _rigidbody.velocity = (Vector3) stream.ReceiveNext();

        // calculate position based on velocity and lag
        _networkPosition = _rigidbody.velocity * lag;
        // difference between local and network position
        _distance = Vector3.Distance(_rigidbody.position, _networkPosition);
    }

    private void SynchronizeAngularVelocity(PhotonStream stream, float lag)
    {
        // receive angular velocity
        _rigidbody.angularVelocity = (Vector3) stream.ReceiveNext();

        _networkRotation = Quaternion.Euler(_rigidbody.angularVelocity * lag);

        _angleDiff = Quaternion.Angle(_rigidbody.rotation, _networkRotation);
    }
}