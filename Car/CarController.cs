using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CarController : MonoBehaviour
{
    private Player player;
    private Car car;
    private Rigidbody2D carRB;

    //Local variables
    private float accelerationInput = 0f;
    private float steeringInput = 0f;

    private float rotationAngle = 0f;
    private float velocityVsUp;

    private bool handBrakeInput;
    private float carSpeed;
    private void Awake()
    {
        //Load components
        player = GameManager.Instance.GetPlayer();
        car = GetComponent<Car>();
        carRB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (player.activeCar.activeCar == null) return;

        SetInputVector();

    }
    private void FixedUpdate()
    {
        if (player.activeCar.activeCar == null) return;

        if (car.gasAmount <= 0f)
        {
            carRB.velocity = Vector2.zero;
            return;
        }

        if (!car.isCarActive) return;

        //Brake
        if (handBrakeInput)
        {
            carRB.drag = Mathf.Lerp(carRB.drag, 5.0f, Time.fixedDeltaTime * 3f);
            return;
        }
        if (accelerationInput != 0 || steeringInput != 0)
        {
            car.gasAmount -= Time.deltaTime * 2f;
            carSpeed = carRB.velocity.sqrMagnitude;
        }


        //Apply engine
        ApplyEngineForce();

        //Kill orthogonol velocity
        KillOrthogonolVelocity();

        //Apply steering
        ApplySteering();
    }

    //<summary>
    //Apply force to the car
    //</summary>
    private void ApplyEngineForce()
    {
        float maxSpeed = car.carDetailsSO.maxSpeed;

        //Calculate how much "forward" we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, carRB.velocity);

        //Limit so we cannot go faster than the max speed in the "forward" direction
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        //Limit so we cannot go faster than the 50% of max speed in the "reverse" direction
        if (velocityVsUp < -maxSpeed * .5f && accelerationInput < 0)
            return;

        //Limit so we cannot go faster in any direction while accelerating
        if (carRB.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;


        //Apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
            carRB.drag = Mathf.Lerp(carRB.drag, 3.0f, Time.fixedDeltaTime * 1.5f);
        else
        {

            carRB.drag = 0;
        }


        //Create a force the engine
        Vector2 engineForceVector = transform.up * accelerationInput * car.carDetailsSO.accelerationFactor;

        //Add force to the rigidbody2d
        carRB.AddForce(engineForceVector, ForceMode2D.Force);
    }
    //<summary>
    //Apply steering to the car
    //</summary>
    private void ApplySteering()
    {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (carRB.velocity.magnitude / 8f);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the rotation angle based on input
        float targetRotationAngle = rotationAngle - steeringInput * car.carDetailsSO.turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply interpolation for smoother turning
        float rotationSpeed = car.carDetailsSO.rotationSpeed;
        rotationAngle = Mathf.LerpAngle(rotationAngle, targetRotationAngle, Time.fixedDeltaTime * rotationSpeed);

        //Move rotation of the rigidbody2d
        carRB.MoveRotation(rotationAngle);
    }
    //<summary>
    //Kill orthogonol velocity. This stops car by going to far without any input and also add some drift factor.
    //</summary>
    private void KillOrthogonolVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRB.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRB.velocity, transform.right);

        carRB.velocity = forwardVelocity + rightVelocity * car.carDetailsSO.driftFactor;
    }



    public float GetSpeed()
    {
        return carSpeed;

    }
    public float GetSpeedMax()
    {
        return car.carDetailsSO.maxSpeed * car.carDetailsSO.maxSpeed;
    }

    private void SetInputVector()
    {

        accelerationInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        handBrakeInput = Input.GetKey(KeyCode.Space);
    }
}
