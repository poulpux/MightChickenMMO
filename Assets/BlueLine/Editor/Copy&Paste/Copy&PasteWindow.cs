using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

struct Bouton
{
    public string name, content;
    public Bouton(string name, string content)
    {
        this.name = name;
        this.content = content; 
    }
}

public class CopyPasteWindow : ToolsGF
{
    private string safeTxt;
    private static List<Bouton> listBouton = new List<Bouton>();
    protected  override Vector2 defaultWindowSize { get { return new Vector2(300f, 1500f ); } }
    public static CopyPasteWindow _window;
    private bool enabled;

    [MenuItem("BlueLine/BlueGlue", false, 12)]
    public static void OpenScript()
    {
         _window = CreateWindow(ref _window, "BlueGlue");
         _window.position = new Rect(1650f, 0f, 10f, 10f);      
    }
    protected override void OnGUI()
    {
        if (_window == null)
        {
            _window = CreateWindow(ref _window, "BlueGlue");
            _window.position = new Rect(1650f, 0f, 10f, 10f);
        }
        if (_window != null && !EditorApplication.isPlaying)
        {
            if (!Start)
                setAllBouton();
            base.OnGUI();
            StartOnGUI(_window, defaultWindowSize);
            displayAllCopyBouton();
            displayTxtArea();
            displayAddBouton();

        }
         
    }

    private void displayAllCopyBouton()
    {
        foreach (var bouton in listBouton)
        {
            if (EditorGUIUtility.systemCopyBuffer == bouton.content)
                GUI.skin.button.normal.background = defaultButtonStylePress;
            else
                GUI.skin.button.normal.background = defaultButtonStyleNotPress;

            if (GUILayout.Button(bouton.name, GUILayout.Width(300f), GUILayout.Height(30f)))
            {
                EditorGUIUtility.systemCopyBuffer = bouton.content;
            }
        }
    }

    private void displayTxtArea()
    {
        safeTxt = GUILayout.TextArea(safeTxt);
    }

    private void displayAddBouton()
    {
        GUI.skin.button.normal.background = defaultButtonStyleNotPress;
        if (GUILayout.Button(("+"), GUILayout.Width(300f), GUILayout.Height(30f))&&!string.IsNullOrEmpty(safeTxt))
        {
            string singletonContent = safeTxt;
            Bouton newBouton = new Bouton(singletonContent, singletonContent);
            listBouton.Add(newBouton);
            safeTxt = "";
        }
    }
    
    private static void  setAllBouton()
    {
        listBouton.Clear();
        string singletonContent = 
            "private static YourClassName instance;\r\n    public static YourClassName Instance { get { return instance; } }\r\n    " +
            "private void Awake()\r\n    {\r\n        if (instance != null && instance != this)\r\n        {\r\n            " +
            "Destroy(this.gameObject);\r\n        }\r\n        else\r\n        {\r\n            instance = this;" +
            "\r\n            DontDestroyOnLoad(this.gameObject);\r\n        }\r\n    }";
        Bouton Singleton = new Bouton("Singleton", singletonContent);

        string eventContent = "YourEvent.AddListener(() =>\r\n        {\r\n            \r\n        });";
        Bouton evente = new Bouton("Add Listener Event", eventContent);

        string serializedFieldContent = "[SerializeField] \r\nprivate";
        Bouton serializedField = new Bouton("[SerializeField] private", serializedFieldContent);
        
        string headerContent = "[Header(\"======YourHeaderName======\")]";
        Bouton header = new Bouton("Header", headerContent);
        
        string toolTipsContent = "[Tooltip (\"\")]";
        Bouton toolTips = new Bouton("Tooltips", toolTipsContent);
        
        string textAreaContent = "[TextArea]\r\nprivate string ";
        Bouton textArea = new Bouton("TextArea", textAreaContent);
        
        string requireContent = "[RequireComponent(typeof(YourNeededComponant))]";
        Bouton require = new Bouton("Require Componant", requireContent);
        
        string editModeContent = "[ExecuteInEditMode]";
        Bouton editMode = new Bouton("Edit Mode Only", editModeContent);
        
        string firstSpaceContent = "//%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%";
        Bouton firstSpace = new Bouton("Limite épaisse", firstSpaceContent);
        
        string secondSpaceContent = "//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
        Bouton secondSpace = new Bouton("Limite fine", secondSpaceContent);

        listBouton.Add(Singleton);
        listBouton.Add(evente);
        listBouton.Add(serializedField);
        listBouton.Add(header);
        listBouton.Add(toolTips);
        listBouton.Add(textArea);
        listBouton.Add(require);
        listBouton.Add(editMode);
        listBouton.Add(firstSpace);
        listBouton.Add(secondSpace);
    }
}
