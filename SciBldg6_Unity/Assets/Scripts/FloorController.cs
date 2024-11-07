using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class FloorController : MonoBehaviour
{
    private Dictionary<string, Material> materialCache = new Dictionary<string, Material>();
    private GameObject[] abnormalObjects;
    private int randomNum;
    private GameObject activeAbnormal;

    void Start()
    {
        LoadMaterials();
        LoadAbnormal();
    }

    private void LoadMaterials()
    {
        for (int floor = 1; floor <= 6; floor++)
        {
            for (int room = 1; room <= 5; room++)
            {
                string objectName = $"r{room}";
                string materialName = $"f{floor}_{objectName}";
                string materialPath = $"Materials/f{floor}/{materialName}";

                Material material = Resources.Load<Material>(materialPath);
                materialCache[materialName] = material;

                string pMaterialName = room <= 3 ? $"f{floor}_p" : $"f{floor}_p_1";
                string pMaterialPath = room <= 3 ? $"Materials/f{floor}/f{floor}_p" : $"Materials/f{floor}/f{floor}_p_1";

                Material pMaterial = Resources.Load<Material>(pMaterialPath);
                materialCache[pMaterialName] = pMaterial;
            }

            foreach (var side in new[] { "Right", "Left" })
            {
                string materialName = $"info_{floor}_{side}";
                string materialPath = $"Materials/f{floor}/{materialName}";
                Material material = Resources.Load<Material>(materialPath);
                materialCache[materialName] = material;
            }
        }
        //Debug.Log($"{materialCache.Count} materials loaded");
    }

    private void LoadAbnormal()
    {
        abnormalObjects = FindObjectsOfType<GameObject>();
        ResetAbnormal();

        foreach (var obj in abnormalObjects)
        {
            if (obj.name.Contains("_F"))
            {
                obj.SetActive(false); // Fオブジェクトを非表示
            }
        }
    }

    public bool FloorControl(int counter, bool isAbnormal, GameObject path)
    {
        var tmp = path;

        ApplyMaterial(tmp, counter);

        RemoveAbnormal();

        if (Random.value < 0.7f)
        {
            ApplyAbnormal();
            isAbnormal = true;
        }
        else
        {
            isAbnormal = false;
            activeAbnormal = null;
        }

        return isAbnormal;
    }

    private GameObject ApplyAbnormal()
    {
        randomNum = Random.Range(1, 18);
        string targetPrefix = $"ab_{randomNum}_";
        GameObject abnormal = null;

        var matchedObjects = abnormalObjects.Where(obj => obj.name.StartsWith(targetPrefix)).ToList();

        foreach (var obj in matchedObjects)
        {
            if (obj.name.Contains("_T"))
            {
                obj.SetActive(false); // 非表示
            }
            else if (obj.name.Contains("_F"))
            {
                obj.SetActive(true); // 表示
                abnormal = obj;
            }
        }

        activeAbnormal = abnormal;
        return abnormal;
    }

    public GameObject GetActiveAbnormal() => activeAbnormal;

    public void RemoveAbnormal()
    {
        foreach (var obj in abnormalObjects)
        {
            if (obj.name.Contains("_T"))
            {
                obj.SetActive(true); // 表示
            }
            else if (obj.name.Contains("_F"))
            {
                obj.SetActive(false); // 非表示
            }
        }
    }

    public void ResetAbnormal()
    {
        foreach (var obj in abnormalObjects)
        {
            obj.SetActive(true);
        }
    }

    private void ApplyMaterial(GameObject path, int counter)
    {
        if (counter <= 1)
        {
            counter = 2;
        }

        for (int room = 1; room <= 5; room++)
        {
            string objectName = $"r{room}";
            string materialName = $"f{counter}_{objectName}";

            Transform objectTransform = path.transform.Find(objectName);
            Renderer renderer = objectTransform.GetComponent<Renderer>();
            if (materialCache.TryGetValue(materialName, out Material material))
            {
                renderer.material = material;
            }

            string pObjectName = $"r{room}_p";
            string pMaterialName = room <= 3 ? $"f{counter}_p" : $"f{counter}_p_1";

            Transform pObjectTransform = path.transform.Find(pObjectName);
            Renderer pRenderer = pObjectTransform.GetComponent<Renderer>();
            if (materialCache.TryGetValue(pMaterialName, out Material pMaterial))
            {
                pRenderer.material = pMaterial;
            }
        }

        foreach (var side in new[] { "Right", "Left" })
        {
            string objectName = $"info_5_{side}";
            string materialName = $"info_{counter}_{side}";

            Transform objectTransform = path.transform.Find(objectName);
            Renderer renderer = objectTransform.GetComponent<Renderer>();
            if (materialCache.TryGetValue(materialName, out Material material))
            {
                renderer.material = material;
            }
        }
    }
}