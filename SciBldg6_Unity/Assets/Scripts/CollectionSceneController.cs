using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionSceneController : MonoBehaviour
{
    public GameObject scrollViewContent; // ScrollViewのContentをアサイン
    public Sprite checkIcon; // アイコンをアサイン
    public TextMeshProUGUI collectionStatusText; // TMPをアサイン

    void Start()
    {
        UpdateCollectionStatus();
    }

    private void UpdateCollectionStatus()
    {
        int totalCards = scrollViewContent.transform.childCount; // 総カード数
        int discoveredCards = 0; // 発見したカード数

        // ScrollViewのContent内のすべてのカードを取得する
        foreach (Transform card in scrollViewContent.transform)
        {
            string cardId = card.name;

            // PlayerPrefsで獲得状況をチェック
            if (PlayerPrefs.GetInt(cardId, 0) == 1) // デフォルト値0（未獲得）
            {
                // 獲得済みで差し替え
                var iconImage = card.Find("icon").GetComponent<Image>();
                if (iconImage != null)
                {
                    iconImage.sprite = checkIcon;
                }
                discoveredCards++;
            }
        }
        collectionStatusText.text = $"発見した異変     <size=33>{discoveredCards}</size> / {totalCards}";
    }
}
