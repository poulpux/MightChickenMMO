using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CurveWindow : EditorWindow
{
    private static Texture2D image;
    public static CurveWindow _window;
    [MenuItem("BlueLine/Tools/HelpCurve", false, 22)]
    public static void OpenScript()
    {
        InitWindow();
    }

    protected void OnGUI()
    {
        if (_window == null)
        {
            InitWindow();
        }
        if (image != null && _window != null && !EditorApplication.isPlaying)
        {
            EditorGUI.DrawRect(new Rect(0, 0, _window.position.width, _window.position.height), AllColorOfBlueLine.backgroundColor);
            EditorGUI.DrawTextureTransparent(new Rect(0, 0, position.width, position.height), image, ScaleMode.ScaleToFit);
        }
    }

    protected static void InitWindow()
    {
        _window = GetWindow<CurveWindow>();
        _window.titleContent = new GUIContent("AllCuves");

        image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/BlueLine/Tools/Visu/AllCurvesDraw.png");
    }
}
