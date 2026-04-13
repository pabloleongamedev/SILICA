public class JetpackAbility : IAbility
{
    private JetpackSystem jetpackSystem;

    private bool isActive;
    private bool isGrounded;

    private float currentForce;

    public JetpackAbility(JetpackSystem system)
    {
        this.jetpackSystem = system;
    }

    public void SetInput(bool isActive)
    {
        this.isActive = isActive;
    }

    public void SetGrounded(bool grounded)
    {
        this.isGrounded = grounded;
    }

    public void Tick(float deltaTime)
    {
        currentForce = jetpackSystem.Tick(
            isGrounded,
            isActive,
            deltaTime
        );
    }

    public float GetForce()
    {
        return currentForce;
    }

    public float GetFuelRatio()
    {
        return jetpackSystem.GetFuelRatio();
    }
}