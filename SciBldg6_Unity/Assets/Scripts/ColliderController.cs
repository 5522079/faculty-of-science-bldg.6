using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private GameObject F5_onWallLeft;
    private GameObject F5_onWallRight;

    void Start()
    {
        F5_onWallLeft = GameObject.Find("F5_onWallLeft");
        F5_onWallRight = GameObject.Find("F5_onWallRight");
    }

    void OnTriggerEnter(Collider other)
    {
        F5_onWallLeft.SetActive(false);
        F5_onWallRight.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        F5_onWallLeft.SetActive(true);
        F5_onWallRight.SetActive(true);
    }
}
