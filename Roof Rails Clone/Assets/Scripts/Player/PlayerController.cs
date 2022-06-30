using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float limit;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private bool isControllerActive = true;
    public bool IsControllerActive { get { return isControllerActive; } set { isControllerActive = value; } }
    private InputHandler inputHandler;
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameFinished) return;
        if (!isControllerActive) return;
        float horizontal = 0;
        if (!IsLimitNeeded(transform.position.x, inputHandler.SwerveInput, limit))
        {
            horizontal = horizontalSpeed * inputHandler.SwerveInput * Time.deltaTime;
        }
        transform.Translate(new Vector3(horizontal, 0, forwardSpeed * Time.deltaTime));

    }
    private static bool IsLimitNeeded(float x, float input, float limit)
    {
        if (input == 0) return true;
        if (x < -limit && input < 0) return true;
        if (x > limit && input > 0) return true;
        return false;
    }
}
