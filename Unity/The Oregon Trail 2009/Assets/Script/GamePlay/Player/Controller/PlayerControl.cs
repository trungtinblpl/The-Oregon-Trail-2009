using UnityEngine;

public enum PlayerSpeed { Stop, Walk, Run, Fast }

public class PlayerControl : MonoBehaviour
{
    public GameObject playerCtrl;   // cha chứa tất cả model (Dad, Mom, Daughter, Oxen)
    private Animator[] animators; // lấy tất cả Animator con

    [Header("Move controll")]
    private float currentSpeed = 0f;
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float fastSpeed = 6f;

    private bool isMoving = false;
    public static PlayerControl Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (playerCtrl != null)
        {
            animators = playerCtrl.GetComponentsInChildren<Animator>();

            foreach (var ani in animators)
            {
                ani.enabled = true; // bật animator
                ani.SetTrigger("Idle"); // mặc định Idle
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        }
    }

    public void SetSpeed(PlayerSpeed speedType)
    {
        switch (speedType)
        {
            case PlayerSpeed.Stop:
                currentSpeed = 0f;
                isMoving = false;
                SetAnimTrigger("Idle");
                break;

            case PlayerSpeed.Walk:
                currentSpeed = walkSpeed;
                isMoving = true;
                SetAnimTrigger("Walk");
                break;

            case PlayerSpeed.Run:
                currentSpeed = runSpeed;
                isMoving = true;
                SetAnimTrigger("Run");
                break;

            case PlayerSpeed.Fast:
                currentSpeed = fastSpeed;
                isMoving = true;
                SetAnimTrigger("Fast");
                break;
        }
    }

    private void SetAnimTrigger(string triggerName)
    {
        if (animators == null) return;

        foreach (var ani in animators)
        {
            ani.ResetTrigger("Idle");
            ani.ResetTrigger("Walk");
            ani.ResetTrigger("Run");
            ani.ResetTrigger("Fast");
            ani.SetTrigger(triggerName);
        }
    }
}
