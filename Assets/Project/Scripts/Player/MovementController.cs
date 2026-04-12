using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [SerializeField] private MovementConfig_SO config;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private MovementSystem movementSystem;
    private VerticalMovementSystem verticalSystem;
    private JetpackSystem jetpackSystem;

    private IMovementStrategy walkStrategy;
    private IMovementStrategy runStrategy;

    private Vector2 moveInput;
    private Vector3 currentVelocity;
    private bool isGrounded;
    private bool isJumpPressed;
    private bool isJumpDown;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        movementSystem = new MovementSystem();
        verticalSystem = new VerticalMovementSystem(config.gravity, config.jumpForce);
        jetpackSystem = new JetpackSystem(config.jetpackForce, config.maxJetpackFuel);

        walkStrategy = new WalkMovement(config.walkSpeed);
        runStrategy = new RunMovement(config.runSpeed);

        movementSystem.SetStrategy(walkStrategy);

        rb.freezeRotation = true;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void SetSprint(bool isSprinting)
    {
        this.isSprinting = isSprinting;

        if (isGrounded)
            movementSystem.SetStrategy(isSprinting ? runStrategy : walkStrategy);
    }

    public void OnJumpStarted()
    {
        isJumpDown = true;
    }

    public void SetJumpHolding(bool isHolding)
    {
        isJumpPressed = isHolding;
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (isGrounded)
            movementSystem.SetStrategy(isSprinting ? runStrategy : walkStrategy);
        else
            movementSystem.SetStrategy(walkStrategy);

        // Movimiento Horizontal
        Vector3 desiredVelocity = movementSystem.CalculateVelocity(moveInput, transform);
        currentVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, config.smoothing * Time.fixedDeltaTime);

        // Movimiento Vertical (SALTO)
        verticalSystem.Tick(isGrounded, isJumpDown, Time.fixedDeltaTime);
        isJumpDown = false;

        // Movimiento Jetpack
        float jetpackVelocity = jetpackSystem.Tick(
            isGrounded,
            isJumpPressed,
            Time.fixedDeltaTime
        );

        // Boost hacia adelante (arreglado con deltaTime)
        Vector3 forwardBoost = Vector3.zero;
        if (!isGrounded && isJumpPressed && isSprinting)
        {
            forwardBoost = transform.forward * config.jetpackBoostForce * Time.fixedDeltaTime;
        }

        Vector3 finalVelocity = currentVelocity + forwardBoost;

        float finalY = verticalSystem.GetVelocity() + jetpackVelocity;

        rb.linearVelocity = new Vector3(finalVelocity.x, finalY, finalVelocity.z);
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );
    }

    public float GetHorizontalSpeed()
    {
        return new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}