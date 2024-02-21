using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateManagerGeneratorWindow : ToolsGF
{
    public string NameOfFamilyName, safeTextFamily, safeText;
    public List<string> states = new List<string>();

    protected override Vector2 defaultWindowSize { get { return new Vector2(700f,100f); } }
    public static StateManagerGeneratorWindow _window;
    public static void CreatWindow()
    {
        _window = CreateWindow(ref _window, "StateManagerGenerator");
    }

    protected override void OnGUI()
    {
        if (_window == null)
        {
            _window = CreateWindow(ref _window, "BlueGlue");
            Start = false;
        }
        if (_window != null && !EditorApplication.isPlaying)
        {
            StartOnGUI(_window, defaultWindowSize);
            Resize();
            DisplayFamilyName();
            DisplayAllChild();
            DisplayAdd();
            DisplayGenerate();
        }
    }

    private  void Resize()
    {
        resizeWindow(_window, new Vector2(defaultWindowSize.x, defaultWindowSize.y + 20 * states.Count));
    }

    private void DisplayFamilyName()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Global State Name : ");
        NameOfFamilyName = GUILayout.TextArea(NameOfFamilyName,GUILayout.Width(550f), GUILayout.Height(15));
        GUILayout.EndHorizontal();
        GUILayout.Space(5f);
    }

    private void DisplayAllChild()
    {
        List<string> children = new List<string>();
        foreach (var state in states)
        {
            bool aAppuyé = false;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name : "+NameOfFamilyName+"_"+state);
            string newState = state;
            newState = GUILayout.TextArea(newState, GUILayout.Width(300), GUILayout.Height(15));
            if (GUILayout.Button("-", GUILayout.Width(30), GUILayout.Height(15f)))
            {
                aAppuyé = true;
            }
            GUILayout.EndHorizontal();
            if(!aAppuyé)
                children.Add(newState);
        }
        states = children;
    }

    private void DisplayAdd()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(50f);
        if (GUILayout.Button("+", GUILayout.Width(600f), GUILayout.Height(30f)))
        {
            states.Add("");
        }
        GUILayout.EndHorizontal();
        
    }
    private void DisplayGenerate()
    {
        if (states.Count != 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(50f);
            if (GUILayout.Button("Generate Scripts", GUILayout.Width(600f), GUILayout.Height(30f)))
            {
                ScriptGenerator.GenerateStatesScript("/Scripts", NameOfFamilyName, states);
            }
            GUILayout.EndHorizontal();
        }
    }
}
