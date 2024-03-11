using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class dataToSave
{
    public string userName;
    public int totalCoins;
    public int crrLevel;
    public int hightScore;
    public List<int> objects = new List<int>();
}

[Serializable]
public class nbFamililier
{
    public int nbFamilier;
}
public class testRealTimeFireBase : MonoBehaviour
{
    public dataToSave dts;
    public nbFamililier familier;
    public string userId;
    DatabaseReference dbRef;

    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        //SaveClassData(familier, "pop/fsdf");
    }

    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

    public void SaveDataFn()
    {
        string json = JsonUtility.ToJson(dts);
        dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json);

        //string json = JsonUtility.ToJson(dts);
        //dbRef.Child("users").Child(userId).Child("StatsPrincipales").SetRawJsonValueAsync(json);

        //string json2 = JsonUtility.ToJson(familier);
        //dbRef.Child("users").Child(userId).Child("StatsSecondaires").SetRawJsonValueAsync(json2);
    }

    public void LoadDataFn()
    {
        StartCoroutine(LoadDataEnum());
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    IEnumerator LoadDataEnum()
    {
        var serverData = dbRef.Child("users").Child(userId).GetValueAsync();
        yield return new WaitUntil(predicate: () => serverData.IsCompleted);

        print("processs is complete");

        DataSnapshot snapshot = serverData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if(jsonData != null)
        {
            print("data found");
            dts = JsonUtility.FromJson<dataToSave>(jsonData);
        }
        else
        {
            print("NO data found");
        }
    }

    private void SaveClassData<T>(T className, string path)
    {
        string ajustedPath = "/"+userId+"/"+path;
        string[] parties = path.Split('/');

        DatabaseReference a = dbRef.Child("users");
        foreach (var item in parties)
        {
            Debug.Log(item);
            a = a.Child(item);
        }

        Debug.Log(a.ToString());
        //string json = JsonUtility.ToJson(className);
        //dbRef.Child("users").Child(userId).Child("StatsPrincipales").SetRawJsonValueAsync(json);
        //DatabaseReference a = dbRef.Child("users");
        //DatabaseReference b = a.Child(userId);
    }


}
