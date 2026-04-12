public class JetpackSystem
{
    private float jetpackForce;

    private float maxFuel;
    private float currentFuel;

    public JetpackSystem(float force, float maxTime)
    {
        this.jetpackForce = force;

        this.maxFuel = maxTime;
        this.currentFuel = maxTime;
    }

    public float Tick(bool isGrounded, bool isJetpackActive, float deltaTime)
    {
        // Recarga automática en el suelo
        if (isGrounded)
        {
            Recharge(deltaTime);
        }

        // Uso del jetpack
        if (isJetpackActive && currentFuel > 0f)
        {
            currentFuel -= deltaTime;

            if (currentFuel < 0f)
                currentFuel = 0f;

            return jetpackForce;
        }

        return 0f;
    }

    public void Recharge(float amount)
    {
        currentFuel += amount;

        if (currentFuel > maxFuel)
            currentFuel = maxFuel;
    }

    public float GetFuelRatio()
    {
        return currentFuel / maxFuel;
    }

    public float GetCurrentFuel()
    {
        return currentFuel;
    }
}