using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float startTime;
    public float clearTime;
    private const string BestTimeKey = "BestTime"; // PlayerPrefs用のキー
    public GameObject persistentObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetAllPlayerPrefs();
    }

    public void StartTimer()
    {
        startTime = Time.time;
    }

    public void StopTimer()
    {
        clearTime = Time.time - startTime;
        SaveBestTime();
    }

    private void SaveBestTime()
    {
        // 現在の自己ベストタイムを取得
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, float.MaxValue);

        // 新しいタイムが自己ベストであれば更新
        if (clearTime < bestTime)
        {
            PlayerPrefs.SetFloat(BestTimeKey, clearTime);
            PlayerPrefs.Save();
        }
    }

    public float GetBestTime()
    {
        // ベストタイムをPlayerPrefsから取得
        float bestTime = PlayerPrefs.GetFloat(BestTimeKey, float.MaxValue);
        return bestTime;
    }

    public float GetClearTime()
    {
        return clearTime;
    }

    public void CollectAbnormal(GameObject abnormalObject)
    {
        if (abnormalObject != null)
        {
            Debug.Log("Collect: " + abnormalObject.name);
            string abnormalObjectName = abnormalObject.name;

            PlayerPrefs.SetInt(abnormalObjectName, 1);
            PlayerPrefs.Save();
        }
    }

    public void LoadTitleScene()
    {
        FadeManager.Instance.LoadScene("00_Title", 0.2f);
    }

    public void ReturnToTitleScene()
    {
        GameObject persistentObject = GameObject.FindWithTag("tmp");
        if (persistentObject != null)
        {
            Destroy(persistentObject);
        }
        FadeManager.Instance.LoadScene("00_Title", 2.0f);
    }

    public void LoadCollectScene()
    {
        FadeManager.Instance.LoadScene("01_Collection", 0.2f);
    }

    public void LoadGameScene()
    {
        FadeManager.Instance.LoadScene("10_Game", 2.0f);
        StartTimer();
    }

    public void LoadResultScene()
    {
        StopTimer();
        FadeInManager.Instance.LoadScene("11_Result", 1.0f);
    }

    public void OpenContactURL()
    {
        Application.OpenURL("https://github.com/5522079/Faculty-of-Science-Bldg.6/issues");
    }

    public void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("すべてのPlayerPrefsを削除しました");
    }
}
