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
    private JetpackAbility jetpackAbility;

    private IMovementStrategy walkStrategy;
    private IMovementStrategy runStrategy;

    private Vector2 moveInput;
    private Vector3 currentVelocity;
    private bool isGrounded;
    private bool isJumpPressed;
    private bool isJumpDown;
    private bool isSprinting;
    private bool isJetpackActive;

    private void Awake()
    {
        // 🔥 VALIDACIONES (evita NullReference)
        if (config == null)
        {
            Debug.LogError("❌ MovementConfig no asignado", this);
            enabled = false;
            return;
        }

        if (groundCheck == null)
        {
            Debug.LogError("❌ GroundCheck no asignado", this);
            enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("❌ Rigidbody no encontrado", this);
            enabled = false;
            return;
        }

        // 🔹 Systems
        movementSystem = new MovementSystem();
        verticalSystem = new VerticalMovementSystem(config.gravity, config.jumpForce);

        JetpackSystem jetpackSystem = new JetpackSystem(
            config.jetpackForce,
            config.maxJetpackFuel
        );

        jetpackAbility = new JetpackAbility(jetpackSystem);

        // 🔹 Strategies
        walkStrategy = new WalkMovement(config.walkSpeed);
        runStrategy = new RunMovement(config.runSpeed);

        movementSystem.SetStrategy(walkStrategy);

        // 🔹 Rigidbody config
        rb.freezeRotation = true;
        rb.useGravity = false; // 🔥 IMPORTANTE: usamos gravedad manual
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

    public void SetJetpack(bool isActive)
    {
        this.isJetpackActive = isActive;
    }

    private void FixedUpdate()
    {
        CheckGround();

        // 🔹 Strategy según estado
        if (isGrounded)
            movementSystem.SetStrategy(isSprinting ? runStrategy : walkStrategy);
        else
            movementSystem.SetStrategy(walkStrategy);

        // 🔹 Movimiento Horizontal
        Vector3 desiredVelocity = movementSystem.CalculateVelocity(moveInput, transform);
        currentVelocity = Vector3.Lerp(
            currentVelocity,
            desiredVelocity,
            config.smoothing * Time.fixedDeltaTime
        );

        // 🔹 Movimiento Vertical (SALTO)
        verticalSystem.Tick(isGrounded, isJumpDown, Time.fixedDeltaTime);
        isJumpDown = false;

        // 🔹 Jetpack Ability
        jetpackAbility.SetInput(isJetpackActive);
        jetpackAbility.SetGrounded(isGrounded);
        jetpackAbility.Tick(Time.fixedDeltaTime);

        float jetpackVelocity = jetpackAbility.GetForce();

        // 🔹 Boost hacia adelante
        Vector3 forwardBoost = Vector3.zero;
        if (!isGrounded && isJetpackActive && isSprinting)
        {
            forwardBoost = transform.forward * config.jetpackBoostForce * Time.fixedDeltaTime;
        }

        Vector3 finalVelocity = currentVelocity + forwardBoost;

        // 🔹 Control del eje Y (PRIORIDAD LIMPIA)
        float verticalVelocity = verticalSystem.GetVelocity();
        
        float finalY = verticalVelocity + jetpackVelocity;

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