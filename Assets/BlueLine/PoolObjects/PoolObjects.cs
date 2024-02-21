using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolObjects : MonoBehaviour
{
    private List<Queue<GameObject>> list = new List<Queue<GameObject>>();
    [HideInInspector]
    public List<GameObject> _AllPrefabs = new List<GameObject>();

    private static PoolObjects instance;
    public static PoolObjects Instance { get { return instance; } }

    public GameObject GetObject(GameObject objectPrefab, Transform parent = null)
    {
        int index = _AllPrefabs.FindIndex(item => item == objectPrefab);
        if (index == -1)
        {
            _AllPrefabs.Add(objectPrefab);
            list.Add(new Queue<GameObject>());
            index = _AllPrefabs.Count - 1;
        }

        if (list[index].Count == 0)
            Populate(1, index, parent);

        GameObject obj = list[index].Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }
    public void ReturnObject(GameObject obj, GameObject prefab)
    {
        int index = _AllPrefabs.FindIndex(item => item == prefab);

        if (obj != null && obj.activeSelf && index != -1)
        {
            CopyComponentValues(prefab, obj);
            obj.gameObject.SetActive(false);
            list[index].Enqueue(obj);
        }
        else if (index != -1)
            Debug.LogWarning("Objet null ou inactif.");
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        transform.position = Vector3.zero;
    }

    //--------------------------------------------------------------------------------------------------------------------

    private void Populate(int count, int index, Transform parent)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newObj = parent == null ? GameObject.Instantiate(_AllPrefabs[index], transform) : GameObject.Instantiate(_AllPrefabs[index], parent);
            newObj.SetActive(false);
            list[index].Enqueue(newObj);
        }
    }

    private void CopyComponentValues(GameObject source, GameObject target)
    {
        foreach (var sourceComponent in source.GetComponents<Component>())
        {
            if (!(sourceComponent is Transform))
            {
                Component targetComponent = target.GetComponent(sourceComponent.GetType());
                if (targetComponent == null)
                    targetComponent = target.AddComponent(sourceComponent.GetType());
            }
            else
            {
                target.transform.position = sourceComponent.transform.position;
                target.transform.rotation = sourceComponent.transform.rotation;
                target.transform.localScale = sourceComponent.transform.localScale;
            }
        }
    }
}
