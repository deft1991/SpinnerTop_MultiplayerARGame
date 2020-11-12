using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotScript : MonoBehaviour
{
    public float speed = 5;
    public float maxVelocityChange = 5f;
    public float tiltAmount = 5f; // величина наклона 
    private Vector3 _velocityVector = Vector3.zero; // initial velocity 
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBot();
    }
    
    private void FixedUpdate()
    {
        if (Vector3.zero != _velocityVector)
        {
            // Get rigidbody`s current velocity
            Vector3 currentVelocity = _rigidbody.velocity;
            Vector3 changVelocity = _velocityVector - currentVelocity;

            // Apply a force by amount of velocity change to reach the target velocity

            changVelocity.x = Mathf.Clamp(changVelocity.x, -maxVelocityChange, maxVelocityChange);
            changVelocity.z = Mathf.Clamp(changVelocity.z, -maxVelocityChange, maxVelocityChange);
            changVelocity.y = 0; // we do no want have jumping spinner 
            _rigidbody.AddForce(changVelocity, ForceMode.Acceleration);
        }
    }
    
    public void MoveBot()
    {
        // taking the joystick input
        float xMovementInput = Random.Range(-10000, 10000);;
        float zMovementInput = Random.Range(-10000, 10000);
        MovePlayer(xMovementInput, zMovementInput);
    }
    
    private void MovePlayer(float xMovementInput, float zMovementInput)
    {
        // calculating velocity vectors
        var movementVelocityVector = CalculateMovementVelocityVector(xMovementInput, zMovementInput);
        // Apply movement
        Move(movementVelocityVector);
        // ChangeTilt(xMovementInput, zMovementInput);
    }
    
    private Vector3 CalculateMovementVelocityVector(float xMovementInput, float zMovementInput)
    {
        Vector3 movementHorizontal = transform.right * xMovementInput;
        Vector3 movementVertical = transform.forward * zMovementInput;

        // calculating final movement velocity vector
        return (movementHorizontal + movementVertical).normalized * speed;
    }
    
    private void Move(Vector3 movementVelocityVector)
    {
        _velocityVector = movementVelocityVector;
    }
    
    /**
     * Add tilt (Наклон) to our rigidBody
     */
    private void ChangeTilt(float xMovementInput, float zMovementInput)
    {
        var verticalTilt = xMovementInput/1000 * speed * tiltAmount;
        var horizontalTilt = -1 * zMovementInput/1000 * speed * tiltAmount;
        _rigidbody.rotation = Quaternion.Euler(verticalTilt, 0,
            horizontalTilt);
    }
}
