using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ToolsGF :  EditorWindow
{
    protected static string NameDesktopToolsImport = "BlueLineImport";
    //protected static MyDirectory assetsFolder;
    private float grosEspaceValue = 20f, petitEspaceValue = 10f;

    protected static bool Start = false;
    protected Texture2D defaultButtonStylePress, defaultButtonStyleNotPress;
    protected abstract Vector2 defaultWindowSize { get; }

    protected static string pathTest;
    public static T CreateWindow<T>(ref T windowReference, string name) where T : EditorWindow
    {
        T window = GetWindow<T>();
        window.titleContent = new GUIContent(name);
        Start = false;
        return window;
    }

    protected virtual void OnGUI()
    {
    }

    protected virtual void InitWindow()
    {

    }

    protected virtual void StartOnGUI(EditorWindow window, Vector2 defaultWindowSize)
    {
        StartDuReste(window, defaultWindowSize);    
        StartChangeCouleur();
        drawBackGround(window);
    }

    private void drawBackGround(EditorWindow window)
    {
        EditorGUI.DrawRect(new Rect(0, 0, window.position.width, window.position.height), AllColorOfBlueLine.backgroundColor);
    }

    protected static void StartDuReste(EditorWindow window, Vector2 defaultWindowSize)
    {
        if (!Start)
        {
            resizeWindow(window, defaultWindowSize);
            GUI.skin.textField.normal.textColor = AllColorOfBlueLine.fontColor;
            GUI.skin.textArea.normal.textColor = AllColorOfBlueLine.fontColor;
            GUI.skin.label.normal.textColor = AllColorOfBlueLine.fontColor;
        }
    }

    public static void StartOnGUI(bool startBool,ref ToolBoxLobby window, Vector2 defaultSize )
    {
        if (!startBool)
        {
            resizeWindow(window,defaultSize);
        }
    }

    public static void resizeWindow(EditorWindow window, Vector2 defaultSize)
    {
        if (window != null && defaultSize != null)
        {
            window.minSize = defaultSize;
            setMaxWindow(window);
        }
    }

    private static void setMaxWindow(EditorWindow window)
    {
        window.maxSize = window.minSize + new Vector2(0.1f, 0.1f);
    }

    public  void StartChangeCouleur()
    {
        if (!Start)
        {
            defaultButtonStylePress = MakeTex(200, 30, AllColorOfBlueLine.buttonColorPress);
            defaultButtonStyleNotPress = MakeTex(200, 30, AllColorOfBlueLine.buttonColorNotPress);

            Start = true;
        }
    }
    // Création d'une texture pour le fond du bouton
    public static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    protected void grosEspace()
    {
        GUILayout.Space(grosEspaceValue);
    }
    
    protected void petitEspace()
    {
        GUILayout.Space(petitEspaceValue);
    }

    public static void FindDirectoryRecursive(MyDirectory root, string path)
    {

        if (!root.name.Equals(NameDesktopToolsImport, StringComparison.OrdinalIgnoreCase))
        {
            string fullPath = Path.Combine(Application.dataPath, path);
            string[] unityPackageFiles = Directory.GetFiles(fullPath, "*.unitypackage");

            if (unityPackageFiles.Length > 0)
            {
                foreach (string packagePath in unityPackageFiles)
                {
                    string packageName = Path.GetFileName(packagePath);
                    //Debug.Log("Found Unity Package: " + packageName);
                    //Debug.Log("path : " + packagePath);
                    MyDirectory newOne = new MyDirectory(packageName, root);
                    newOne.path = packagePath;
                    newOne.isPackage = true;
                    root.children.Add(newOne);

                    root.pathPackage.Add (packagePath);
                }

            }
        }
        string[] directories = Directory.GetDirectories(path);
        if (directories.Length == 0)
            return;

        foreach (string dir in directories)
        {
            string relativePath = dir.Replace(path + "\\", "");
            MyDirectory newOne = new MyDirectory(relativePath, root);
            if (root.name == "Assets")
                root.path = root.name;
            else if (root.name == "BlueLine")
                root.path = path;
            else if (root.name == "Dossier Perso")
                root.path = path;

            newOne.path = root.path + "/" + newOne.name;

            root.children.Add(newOne);
            FindDirectoryRecursive(newOne, dir);
        }
    }

    protected void ExportWindowUnity()
    {
        ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>("Assets"));
        EditorApplication.ExecuteMenuItem("Assets/Export Package...");
    }
}



//                                                                  %%%%#(#%%%%                         
//                                                               %%*            %%                      
//                                                             (%.          ..... %%                    
//                                               .....        %%        ...........#%                   
//                                                ...      *%%           ...........%%                  
//                                                       %%               ..........%%                  
//                                      %%%             %%                .........%%                   
//                                  (%%                 %#             ..........#%(                    
//                                #%                   #%          ........../%%%.                      
//                               (%   %%%/  .%%%%     %%         ......#%%%#... *%%%%,      ,%%         
//                                %(%(             #%%    ...    .....%%%.  ,%%         ..... .%        
//                                 %%%,      .#%%%*                        %%%%         .....  %/       
//                                %                (%%                          %%#           #%        
//                          /   ,%,             /%%                         ...    %% %%     %%         
//                            *,      ..../%%%%(       (%.    #%            .....   %%  .%%%            
//                                .../%%/             .%#.     %.            ....    %                  
//                               ..%%                 %%..     (%                    %*        /%       
//                              ..%%.               #%....      /%                   %%       *%        
//                               .#%.              %%             %%%                 %%    %%#         
//                                  #%%(          ,%                  %%%                               
//                                      *%%        %                     *%%                            
//                                        %%       (%                      ,%.                          
//                                       %%          %%                     ,%                          
//                                                     %%(                  %%                          
//                                                        %%              %%*                           
//                                                         %%           *                               
//      