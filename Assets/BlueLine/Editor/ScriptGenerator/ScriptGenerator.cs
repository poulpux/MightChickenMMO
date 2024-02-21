using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptGenerator
{
    public static void GenerateScript(Action<string, string> function, string path, string familyName, List<string> listChild = null)
    {
        function.Invoke(path, familyName);
        AssetDatabase.Refresh();
    }
    public static void GenerateStatesScript(string path, string familyName, List<string> listChild)
    {
        string scriptContent = GenerateScriptStateParentContent(familyName, listChild);

        string filePath = Path.Combine(Application.dataPath + path, familyName + ".cs");

        // Écriture du contenu dans le fichier
        File.WriteAllText(filePath, scriptContent);
        Debug.Log("Script généré : " + familyName);

        foreach (var child in listChild)
        {
            scriptContent = GenerateScriptGenerateStateContent(child, familyName);

            filePath = Path.Combine(Application.dataPath + path, familyName+"_"+child + ".cs");

            // Écriture du contenu dans le fichier
            File.WriteAllText(filePath, scriptContent);
            Debug.Log("Script généré : " + familyName + "_" + child);
        }
        AssetDatabase.Refresh();
    }

    public static void GenerateCooldownScript(string path,string cooldownName)
    {

        string UpperName = Tools.firstLetterToUpper(cooldownName);
        string scriptContent = GenerateScriptCooldownContent(UpperName);

        string filePath = Path.Combine(Application.dataPath + path, "Cooldown_"+ UpperName + ".cs");

        // Écriture du contenu dans le fichier
        File.WriteAllText(filePath, scriptContent);
        Debug.Log("Script généré : " + "Cooldown_" + UpperName);
    }

    public static void GenerateMVCDesignePatternScript(string path, string cooldownName)
    {

        string UpperName = Tools.firstLetterToUpper(cooldownName);
        string scriptFunctionContent = GenerateMVCFunctionContent(UpperName);
        string scriptHeaderContent = GenerateMVCHeaderContent(UpperName);
        string scriptPresenterContent = GenerateMVCPresenterContent(UpperName);

        string filePathFunction = Path.Combine(Application.dataPath + path, UpperName+"_Function" + ".cs");
        string filePathPresenter = Path.Combine(Application.dataPath + path, UpperName+"_View" + ".cs");
        string filePathHeader = Path.Combine(Application.dataPath + path, UpperName+"_Model" + ".cs");

        // Écriture du contenu dans le fichier
        File.WriteAllText(filePathFunction, scriptFunctionContent);
        Debug.Log("Script généré : " + UpperName + "_Function");

        File.WriteAllText(filePathHeader, scriptHeaderContent);
        Debug.Log("Script généré : " + UpperName + "_Header");
        
        File.WriteAllText(filePathPresenter, scriptPresenterContent);
        Debug.Log("Script généré : " + UpperName + "_Presenter");

    }

    private static string GenerateScriptStateParentContent(string familyName, List<string> listChild)
    {
        string upperFamilyName = Tools.firstLetterToUpper(familyName);
        string lowerFirstChild = Tools.firstLetterToLower(listChild[0]);

        string startContent = "";
        foreach (var item in listChild)
        {
            string upperChildName = Tools.firstLetterToUpper(item);
            string lowerChildName = Tools.firstLetterToLower(item);
            startContent += "\r\n        "+ lowerChildName + ".InitState(on"+ upperChildName + "Enter, on"+ upperChildName + "Update, on"+ upperChildName + "Exit); ";
        }
        return
            "using System.Collections;" +
            "\r\nusing System.Collections.Generic;" +
            "\r\nusing UnityEngine;" +
            "\r\n\r\n\r\npublic partial class " + upperFamilyName + " : StateManager" +
            "\r\n{" +
            "\r\n\r\n    protected override void Start()" +
            "\r\n    {" +
            "\r\n        base.Start();" +

            "\r\n"+startContent +
            "\r\n        ForcedCurrentState(" + lowerFirstChild + ");" +
            "\r\n    }" +
            "\r\n\r\n    protected override void Update()" +
            "\r\n    {" +
            "\r\n        base.Update();" +
            "\r\n    }" +
            "\r\n}";
    }

    private static string GenerateScriptGenerateStateContent(string state, string familyName)
    {
        string upperFamilyName = Tools.firstLetterToUpper(familyName);
        string upperChildName = Tools.firstLetterToUpper(state);
        string lowerChildName = Tools.firstLetterToLower(state);
        return
            "using System.Collections;" +
            "\r\nusing System.Collections.Generic;" +
            "\r\nusing UnityEngine;" +
            "\r\n\r\npublic partial class "+ upperFamilyName + "" +
            "\r\n{" +
            "\r\n    State "+ lowerChildName + " = new State();" +
            "\r\n\r\n    private void on"+ upperChildName + "Enter()" +
            "\r\n    {" +
            "\r\n       " +
            "\r\n    }" +
            "\r\n    private void on"+ upperChildName + "Update()" +
            "\r\n    {" +
            "\r\n\r\n    }" +
            "\r\n\r\n    private void on"+ upperChildName + "Exit()" +
            "\r\n    {" +
            "\r\n\r\n    }" +
            "\r\n}";
    }

    private static string GenerateScriptCooldownContent(string cooldownName)
    {
        return
            "using System.Collections;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using UnityEngine;\r\n\r\n" +
            "public class Cooldown_"+ cooldownName + " : MonoBehaviour\r\n" +
            "{\r\n" +
            "    [SerializeField]\r\n" +
            "    private float cooldownDuration = 3f;\r\n\r\n" +
            "    private float timer = 0f;\r\n" +
            "    void Start()\r\n" +
            "    {\r\n" +
            "        \r\n" +
            "    }\r\n" +
            "    void Update()\r\n" +
            "    {\r\n" +
            "        if(timer > cooldownDuration )\r\n" +
            "        {\r\n" +
            "            use"+ cooldownName + "();\r\n" +
            "            timer = 0;\r\n" +
            "        }\r\n" +
            "        else\r\n" +
            "        {\r\n" +
            "            timer += Time.deltaTime;\r\n" +
            "        }\r\n" +
            "    }\r\n" +
            "    private void use"+ cooldownName + "()\r\n" +
            "    {\r\n\r\n" +
            "    }\r\n" +
            "}";
    }

    private static string GenerateMVCPresenterContent(string scriptName)
    {
        return
            "using System.Collections;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using UnityEngine;\r\n\r\n" +
            "[RequireComponent(typeof(" + scriptName + "_Function))]\r\n" +
            "[RequireComponent(typeof(" + scriptName + "_Model))]\r\n" +
            "public class " + scriptName + "_View : MonoBehaviour\r\n" +
            "{\r\n" +
            "\r\n" +
            "}";
    }
    private static string GenerateMVCHeaderContent(string scriptName)
    {
        return
            "using System.Collections;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using UnityEngine;\r\n\r\n" +
            "[RequireComponent(typeof(" + scriptName + "_Function))]\r\n" +
            "[RequireComponent(typeof(" + scriptName + "_View))]\r\n" +
            "public class "+scriptName+ "_Model : MonoBehaviour\r\n" +
            "{\r\n" +
            "\r\n"+
            "}";
    }
    private static string GenerateMVCFunctionContent(string scriptName)
    {
        return
            "using System.Collections;\r\n" +
            "using System.Collections.Generic;\r\n" +
            "using UnityEngine;\r\n\r\n" +
            "[RequireComponent(typeof("+scriptName+ "_Model))]\r\n" +
            "[RequireComponent(typeof("+scriptName+"_View))]\r\n" +
            "public class "+scriptName+"_Function : MonoBehaviour\r\n" +
            "{\r\n" +
            "    private "+scriptName+"_Model _model;\r\n" +
             "    private " + scriptName + "_View _view;\r\n" +
            "    void Start()\r\n" +
            "    {\r\n" +
            "        _model = GetComponent<" + scriptName + "_Model>();\r\n" +
            "        _view = GetComponent<" + scriptName + "_View>();\r\n" +
            "    }\r\n\r\n" +
            "    void Update()\r\n" +
            "    {\r\n" +
            "        \r\n" +
            "    }\r\n" +
            "}\r\n";
    }
}