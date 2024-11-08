using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int counter = 6;
    public bool isAbnormal = false;
    private FloorController _FloorController;
    public Vector3 teleportOffset = new Vector3(0f, 3.791f, 0f);
    public Image whiteoutImage;
    public float fadeStartY = 2.0f;  // ホワイトアウトが始まるY座標
    public float fadeEndY = 0.23f;    // 完全にホワイトアウトするY座標
    private GameObject activeAbnormalObject;

    void Start()
    {
        _FloorController = FindObjectOfType<FloorController>();
        Color color = whiteoutImage.color;
        color.a = 0f;
        whiteoutImage.color = color;
    }

    void Update()
    {
        if (counter == 1)
        {
            Whiteout();
        }
    }

    void Teleport()
    {
        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false;
        this.transform.position += teleportOffset;
        controller.enabled = true;
    }

    void Whiteout()
    {
        float playerY = transform.position.y;
        float t = Mathf.InverseLerp(fadeEndY, fadeStartY, playerY);
        Color color = whiteoutImage.color;
        color.a = 1f - t; // Alpha値を更新
        whiteoutImage.color = color;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject path = other.transform.parent.gameObject;

        if ((other.gameObject.CompareTag("GateLeft") && !isAbnormal) || (other.gameObject.CompareTag("GateRight") && isAbnormal))
        {
            GameController.instance.CollectAbnormal(activeAbnormalObject);

            if (counter == 2)
            {
                counter--;
                Debug.Log("Counter: " + counter);

                isAbnormal = _FloorController.FloorControl(counter, isAbnormal, path);
            }
            else
            {
                counter--;
                Debug.Log("Counter: " + counter);

                isAbnormal = _FloorController.FloorControl(counter, isAbnormal, path);

                activeAbnormalObject = _FloorController.GetActiveAbnormal();
                Debug.Log("Active Abnormal Object: " + (activeAbnormalObject != null ? activeAbnormalObject.name : "None"));

                Teleport();
            }
        }
        else if (other.gameObject.CompareTag("Goal"))
        {
            GameController.instance.LoadResultScene();
        }
        else if (other.gameObject.CompareTag("Collider"))
        {

        }
        else
        {
            counter = 6;
            Debug.Log("Counter: " + counter);

            isAbnormal = _FloorController.FloorControl(counter, isAbnormal, path);

            activeAbnormalObject = _FloorController.GetActiveAbnormal();
            Debug.Log("Active Abnormal Object: " + (activeAbnormalObject != null ? activeAbnormalObject.name : "None"));

            Teleport();
        }

        //Debug.Log("Counter: " + counter);
    }
}
