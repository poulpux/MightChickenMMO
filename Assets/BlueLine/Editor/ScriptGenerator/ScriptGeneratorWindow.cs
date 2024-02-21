using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public partial class ScriptGeneratorWindow : ToolsGF
{
    private bool labelEnPlus, cooldownPress, MVCDesignPatternPress;
    private string saveName;
    private string StateManager = "StateManager";
    private string MVCDesignPattern = "MVCDesignPattern";
    private string Cooldown = "Cooldown";
    private List<string> allBouton = new List<string>();
    protected override Vector2 defaultWindowSize { get { return new Vector2(800f, 20f); } }
    public static ScriptGeneratorWindow _window;
    [MenuItem("BlueLine/ScriptGenerator", false, 12)]
    public static void OpenScript()
    {
        _window = CreateWindow(ref _window, "ScriptGenerator");
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
            if (!Start)
                setAllBouton();
            StartOnGUI(_window, defaultWindowSize);
            Resize();
            DisplayAllButton();
            defLabelEnPlus();
            DisplayLabelEnPlus();
            DisplayBoutonGenerate();
        }
    }

    private void Resize()
    {
        float sizeY = defaultWindowSize.y + 40 * allBouton.Count;
        if (labelEnPlus)
            sizeY += 60f;
        if (!string.IsNullOrEmpty(saveName))
            sizeY += 30f;
        resizeWindow(_window, new Vector2(defaultWindowSize.x, sizeY));
    }

    private void DisplayAllButton()
    {
        foreach (var boutonName in allBouton)
        {
            petitEspace();
            GUILayout.BeginHorizontal();
            GUILayout.Space(25f);
            if(boutonName == Cooldown && cooldownPress)
                GUI.skin.button.normal.background = defaultButtonStylePress;
            else if (boutonName == MVCDesignPattern && MVCDesignPatternPress)
                GUI.skin.button.normal.background = defaultButtonStylePress;


            if (GUILayout.Button(boutonName, GUILayout.Width(750f), GUILayout.Height(30f)))
            {
                onClickAction(boutonName);
            }
            GUILayout.EndHorizontal();

             GUI.skin.button.normal.background = defaultButtonStyleNotPress;
        }
    }

    private void defLabelEnPlus()
    {
        labelEnPlus = false;
        if(cooldownPress || MVCDesignPatternPress)
        {
            labelEnPlus = true;
        }
    }

    private void DisplayLabelEnPlus()
    {
        if(labelEnPlus)
        {
            petitEspace();
            GUILayout.BeginHorizontal();
            if(cooldownPress)
                GUILayout.Label("Nom du Scipts : Cooldown_" + saveName, GUILayout.Width(400f));
            else if(MVCDesignPatternPress)
                GUILayout.Label("Nom du Scipts : " + saveName, GUILayout.Width(400f));
            saveName = GUILayout.TextArea(saveName);
            GUILayout.EndHorizontal();
        }
    }

    private void DisplayBoutonGenerate()
    {
        if (!string.IsNullOrEmpty(saveName))
        {
            petitEspace();
            GUILayout.BeginHorizontal();
            GUILayout.Space(100f);
            if (GUILayout.Button("Generate Scripts", GUILayout.Width(600f), GUILayout.Height(30f)))
            {
                if (cooldownPress)
                {
                    ScriptGenerator.GenerateScript(ScriptGenerator.GenerateCooldownScript, "", saveName);
                    cooldownPress = false;
                }
                else if (MVCDesignPatternPress)
                {
                    ScriptGenerator.GenerateScript(ScriptGenerator.GenerateMVCDesignePatternScript, "", saveName);
                    MVCDesignPatternPress = false;
                }
                saveName = "";
                
            }
            GUILayout.EndHorizontal();
        }
    }

    private void onClickAction(string boutonName)
    {
        if (boutonName == StateManager)
        {
            StateManagerGeneratorWindow.CreatWindow();
        }
        else if (boutonName == MVCDesignPattern)
        {
            MVCDesignPatternPress = !MVCDesignPatternPress;
            cooldownPress = false;
        }
        else if (boutonName == Cooldown)
        {
            cooldownPress = !cooldownPress;
            MVCDesignPatternPress = false;
        }
    }

    private void setAllBouton()
    {
        allBouton.Clear();
        allBouton.Add(StateManager);
        allBouton.Add(MVCDesignPattern);
        allBouton.Add(Cooldown);
    }
}
