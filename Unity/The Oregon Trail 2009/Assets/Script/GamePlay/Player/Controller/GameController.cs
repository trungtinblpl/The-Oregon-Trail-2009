using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerControl playerControl;
    // miles tăng theo tốc độ wagon
    public float travelSpeed = 2f; // miles / second

    // Hunger giảm mỗi 3 giây
    public float hungerRate = 3f;
    private float hungerTimer = 0f;

    void Start()
    {
        // Anti bug: miles mặc định luôn >= 0
        playerData.milesTravelled = Mathf.Max(playerData.milesTravelled, 0);
    }

    void Update()
    {
        UpdateMiles();
        UpdateHunger();
    }

    private void UpdateMiles()
    {
        // tăng dương theo thời gian
        playerData.milesTravelled += Time.deltaTime * travelSpeed;

        Debug.Log($"Miles = {playerData.milesTravelled:F2}");

        if (playerControl.isMoving)
        {
            transform.Translate(Vector3.left * playerControl.currentSpeed * Time.deltaTime);
        }
    }

    private void UpdateHunger()
    {
        hungerTimer += Time.deltaTime;

        if (hungerTimer >= hungerRate)
        {
            playerData.stats.hunger = Mathf.Max(0, playerData.stats.hunger - 1);
            Debug.Log($"Hunger = {playerData.stats.hunger}");

            hungerTimer = 0f;
        }
    }
}
