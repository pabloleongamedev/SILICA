using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAudio : MonoBehaviour
{
    [Header("Input Action")]
    public InputActionReference walkAction;
    public InputActionReference runAction;
    public InputActionReference jetPackAction;
    private bool isWalkingSoundPlaying = false;
    private bool isJetpackSoundPlaying = false;
    private bool isGrounded = false; // This should be set based on your player's grounded state
    private void OnEnable()
    {
        walkAction.action.Enable();
        if (runAction != null)
        {
            runAction.action.Enable();
        }
        if (jetPackAction != null)
        {
            jetPackAction.action.Enable();
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputMovement = walkAction.action.ReadValue<Vector2>().magnitude;
        bool isRunning = runAction.action.IsPressed();
        bool isJetPackActive = jetPackAction.action.IsPressed();
        if (inputMovement > 0.1f && isGrounded)
        {
            if (!isWalkingSoundPlaying)
            {
                AudioManager.Instance.Play("Playerwalksound");
                isWalkingSoundPlaying=true;
            }
        float targetPitch = isRunning ? 1.5f : 1f; // Adjust pitch for running
        AudioManager.Instance.ChangePitch("Playerwalksound", targetPitch);
        }
        else
        {
            if (isWalkingSoundPlaying)
            {
                AudioManager.Instance.Stop("Playerwalksound");
                isWalkingSoundPlaying = false;
            }
        }
        if (isJetPackActive)
        {
            if (!isJetpackSoundPlaying)
            {
                AudioManager.Instance.Play("Playerjetpacksound");
                isJetpackSoundPlaying=true;
            }
        }else
        {
            if (isJetpackSoundPlaying)
            {
                AudioManager.Instance.Stop("Playerjetpacksound");
                isJetpackSoundPlaying = false;
            }
        }
    }
    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.Instance.Play("Playerjumpsound");
            isGrounded = true;
        }
    }
    
}

