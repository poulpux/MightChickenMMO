using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ToolsWidow : EditorWindow
{
    [MenuItem("BlueLine/Tools/Script", false,11)]
    public static void OpenScript()
    {
        string scriptPath = "Assets/BlueLine/Tools/Scripts/Tools.cs"; 
        System.Diagnostics.Process.Start("devenv", "/edit " + scriptPath);
    }
}
