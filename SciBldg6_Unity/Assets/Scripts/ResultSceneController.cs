using UnityEngine;
using UnityEngine.UI;

public class ResultSceneController : MonoBehaviour
{
    public Text clearTimeText;
    public Text bestTimeText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float clearTime = GameController.instance.GetClearTime();
        float bestTime = GameController.instance.GetBestTime();
        Debug.Log($"Clear Time: {clearTime}");

        DisplayClearTime(clearTime);
        DisplayBestTime(bestTime);

        if (clearTime == bestTime)
        {
            ChangeColor();
        }
    }

    private void DisplayClearTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        float milliseconds = (time * 1000F) % 1000F;

        clearTimeText.text = $"クリアタイム  {minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    private void DisplayBestTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        float milliseconds = (time * 1000F) % 1000F;

        bestTimeText.text = $"ベストタイム  {minutes:00}:{seconds:00}:{milliseconds:00}";
    }

    public void ChangeColor()
    {
        clearTimeText.color = Color.red;
        bestTimeText.color = Color.red;
    }
}
