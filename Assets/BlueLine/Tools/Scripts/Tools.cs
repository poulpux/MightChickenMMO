using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Net.NetworkInformation;

public partial class Tools : MonoBehaviour
{
    //public static void OrbitAround(ref Transform yourTransform , Transform TargetTransform, float speedRotation, bool sensHoraire = true)
    //{
    //    Vector3 direction = yourTransform.position - TargetTransform.position;  
    //    direction.Normalize();
    //    int sens = sensHoraire ? 1 : -1;
    //    yourTransform.RotateAround(TargetTransform.position,(Vector3.up * Mathf.Abs( direction.x)+ Vector3.right* Mathf.Abs(direction.y)) *sens, 80f * Time.deltaTime); // Par exemple, autour de l'axe vertical
    //}

    /// <summary>
    /// Will save a .json in your persistantDataPath
    /// </summary>
    /// <param name="jsonName">exemple : playerScore</param>
    /// <param name="content">exemple :  "playerName: Player3 ,playerScore: 300" </param>
    /// <param name="path">exemple : myProject/highscore</param>
    public static void SafeInJson(string jsonName, string content, string path = "")
    {
        // Chemin du fichier de sauvegarde
        string filePath = path != ""? Application.persistentDataPath + "/"+path + "/"+jsonName+".json" : Application.persistentDataPath + "/" + jsonName + ".json";

        // Construction du JSON manuellement
        string jsonData = "{\""+content+ "}";

        // Écriture dans le fichier
        File.WriteAllText(filePath, jsonData);
    }

    //Va enregister une classe directement
    public static void SafeInJson<T>(string jsonName, T classe, string path = "")
    {
        if (!typeof(T).IsClass)
        {
            Debug.LogError("la variable n'est pas une classe");
            return;
        }
        // Chemin du fichier de sauvegarde
        string filePath = path != "" ? Application.persistentDataPath + "/" + path + "/" + jsonName + ".json" : Application.persistentDataPath + "/" + jsonName + ".json";
        string content = "";
        Type type = typeof(T);
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        int counter = 0;
        foreach (FieldInfo field in fields)
        {
            object value = field.GetValue(classe);
            if(counter == fields.Length-1)
                content += "\"" + field.Name + "\"" + ":" + "\"" + value + "\"";
            else
                content += "\""+field.Name+"\"" + ":"+ "\""+ value+ "\"" + ",";
            counter++;
        }

        // Construction du JSON manuellement
        string jsonData = "{" + content + "}";

        // Écriture dans le fichier
        File.WriteAllText(filePath, jsonData);
    }

    /// <summary>
    /// To read .json in persistantDataPath
    /// </summary>
    /// <param name="jsonName">exemple : playerScore</param>
    /// <param name="path">exemple : myProject/highscore</param>
    /// <returns></returns>
   
    //Va recopier la classe du .json dans ta classe, ATTENTION : ne prend en compte que les variables publique et ça n'accepte pas les listes
    public static string ReadFromJSON<T>(string jsonName,ref T classe , string path = "" )
    {
        string filePath = path != "" ? Application.persistentDataPath + "/" + path + "/" + jsonName + ".json" : Application.persistentDataPath + "/" + jsonName + ".json";
        if(!classe.Equals(default(T)))
        {
            string jsonData = File.ReadAllText(filePath);
            classe = JsonUtility.FromJson<T>(jsonData);
        }

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Debug.Log("Le fichier JSON n'existe pas : " + filePath);
            return null;
        }
    }

    public static string ReadFromJSON(string jsonName, string path = "")
    {
        string filePath = path != "" ? Application.persistentDataPath + "/" + path + "/" + jsonName + ".json" : Application.persistentDataPath + "/" + jsonName + ".json";

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Debug.Log("Le fichier JSON n'existe pas : " + filePath);
            return null;
        }
    }
    public static string firstLetterToUpper(string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }
    
    public static string firstLetterToLower(string str)
    {
        return char.ToLower(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// This will play a LerpCurve. To restart it, just yourCurve.timeSinceBegin = 0f
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="valueToModify"></param>
    public static void PlayCurve(ref AnimatingCurve curve,ref Vector3 valueToModify)
    {
        curve.timeSinceBegin += Time.deltaTime * curve.reverse;
        if (curve.animCurv == null)
        {

            if(isCurveFinish(curve) == true)
                LoopGestion(ref curve, ref valueToModify);
            else
                DrawCurve(curve, ref valueToModify);
        }
        else
        {
            valueToModify =curve.endValue* curve.animCurv.Evaluate(curve.timeSinceBegin);
        }
    }

    public static void PlayCurve(ref AnimatingCurve curve, ref float valueToModify)
    {
        curve.timeSinceBegin += Time.deltaTime * curve.reverse;
        if (curve.animCurv == null)
        {
            if (isCurveFinish(curve) == true)
                LoopGestion(ref curve, ref valueToModify);
            else
                DrawCurve(curve, ref valueToModify);
        }
        else
        {
            valueToModify = curve.endValueF * curve.animCurv.Evaluate(curve.timeSinceBegin);
        }
    }

    public static bool isCurveFinish(AnimatingCurve curve)
    {
        if ((curve.timeSinceBegin - Time.deltaTime * curve.reverse < curve.duration && curve.reverse == 1f)|| (curve.timeSinceBegin + Time.deltaTime * curve.reverse > 0f && curve.reverse == -1f))
            return false;
        return true;
    }

    public static void LookAtTargetLerp(Transform YourTransform,  Transform TargetTransform,float rotationSpeed)
    {
        if (TargetTransform != null && YourTransform != null)
        {
            Vector3 targetDirection = TargetTransform.position - YourTransform.position;

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                YourTransform.rotation = Quaternion.Lerp(YourTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Will modifie timeScale and fixedDeltaTime so that the slowmotions are done without framerate modification
    /// </summary>
    /// <param name="timeScale"></param>
    public static void TimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
    }
}
