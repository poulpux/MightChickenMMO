using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ImportWindow : ToolsGF
{
    static ImportWindow _window;
    protected override Vector2 defaultWindowSize { get { return new Vector2(500f, 800f); } }
    static MyDirectory toolBoxImportFolder;
    static MyDirectory toolBoxPersoFolder;
    private List<MyDirectory> listOfSelection = new List<MyDirectory>();
    private float timer;

    // Ouvre la fenêtre d'importation
    public static void WindowImport()
    {
        _window = CreateWindow(ref _window, "BlueLine ImportPackage");
        DefineToolBoxHierarchy();
    }

    // Dessine la fenêtre d'importation
    protected override void OnGUI()
    {
        if (_window == null)
        {
            _window = CreateWindow(ref _window, "BlueLine ImportPackage");
            DefineToolBoxHierarchy();
            Start = true;
        }
        if (_window != null && !EditorApplication.isPlaying)
        {
            base.OnGUI();
            StartOnGUI(_window, defaultWindowSize);

            listOfSelection.Clear();
            DisplayAllDirectoriesRecursive(toolBoxImportFolder, 0);
            DisplayAllDirectoriesRecursive(toolBoxPersoFolder, 0);
            DisplayImportButton();
        }
    }
    private static void DefineToolBoxHierarchy()
    {
        // Création de la hiérarchie ToolBox
        toolBoxImportFolder = new MyDirectory("BlueLine", null);
        toolBoxImportFolder.foldout = true;

        // Chemin final pour sauvegarder la hiérarchie sur le bureau
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string finalSavePath = desktopPath + "/"+ NameDesktopToolsImport + "/" + toolBoxImportFolder.name;

        // Recherche récursive des dossiers
        FindDirectoryRecursive(toolBoxImportFolder, finalSavePath);

        toolBoxPersoFolder = new MyDirectory("Dossier Perso", null);
        toolBoxPersoFolder.foldout = true;

        // Chemin final pour sauvegarder la hiérarchie sur le bureau
        string finalSavePathPerso = desktopPath + "/" + NameDesktopToolsImport + "/BlueLinePerso";

        Debug.Log( "path perso : "+ finalSavePathPerso);
        // Recherche récursive des dossiers
        FindDirectoryRecursive(toolBoxPersoFolder, finalSavePathPerso);
    }

    public void DisplayAllDirectoriesRecursive(MyDirectory root, int count = 0, bool seeHim = true)
    {
        // Affichage récursif des fichier
        SelectedGestion(root);

        if (seeHim)
        {
            GUILayout.BeginHorizontal();
            ChangeFontColorSelected(root);
            HorizontalOffset(count);
            PackageToggle(ref root);
            FoldoutAndSpace(ref root, count);
            GUILayout.Label(root.name);
            GUI.skin.label.normal.textColor = AllColorOfBlueLine.fontColor;

            GUILayout.EndHorizontal();
        }
        TheRecursivity(root, count, seeHim);
    }

    // Bouton d'importation de tous les packages sélectionnés
    public void DisplayImportButton()
    {
        if (listOfSelection.Count > 0)
        {
            if (GUILayout.Button("Import All Packages", GUILayout.Width(490), GUILayout.Height(15)))
            {
                ImportNextPackage();
                _window.Close();
            }
        }
    }

    private void ImportNextPackage()
    {
        // Importe le prochain package de la liste sélectionnée
        if (listOfSelection.Count > 0)
        {
            MyDirectory nextPackage = listOfSelection[0];
            listOfSelection.RemoveAt(0);

            // Importation du package
            AssetDatabase.ImportPackage(nextPackage.path, true);

            // Vérification si la fenêtre d'import est active
            EditorApplication.update += CheckIfImportWindowIsActive;
        }
        else
        {
            //Debug.Log("Tous les packages ont été importés.");
        }
    }

    private void CheckIfImportWindowIsActive()
    {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Import Unity Package")
        {
            timer += Time.deltaTime / 100;
            // Attendez que la fenêtre d'importation se ferme
            return;
        }
        else if (timer > 0.02f)
        {
            timer = 0;
            // Si la fenêtre d'importation est fermée, passez au prochain package
            EditorApplication.update -= CheckIfImportWindowIsActive;
            ImportNextPackage();
        }
    }

    // Changement de couleur de police pour la sélection
    private void ChangeFontColorSelected(MyDirectory root)
    {
        if (root.selected)
        {
            GUI.skin.label.normal.textColor = AllColorOfBlueLine.fontSelected;
        }
    }

    private void SelectedGestion(MyDirectory root)
    {
        if(root.selected)
        {
            listOfSelection.Add(root);
        }
    }

    private void HorizontalOffset(int count)
    {
        GUILayout.Space(count * 10f + 30f);
    }

    private void PackageToggle(ref MyDirectory root)
    {
        if (root.isPackage)
            root.selected = GUILayout.Toggle(root.selected, "", GUILayout.Width(10f));
    }

    private void FoldoutAndSpace(ref MyDirectory root, int count)
    {
        if (root.children.Count > 0)
        {
            var style = new GUIStyle(EditorStyles.foldout);
            style.margin = new RectOffset(0, 0, 0, 0);
            root.foldout = EditorGUILayout.Foldout(root.foldout, "", style);
            GUILayout.Space(-455f + count * 10f + root.name.Length * 5f);
        }
        else
        {
            GUILayout.Space(20f);
        }
    }

    // Récursivité pour afficher tous les répertoires
    private void TheRecursivity(MyDirectory root, int count, bool seeHim = true)
    {
        if (root.children.Count > 0)
        {
            foreach (MyDirectory child in root.children)
            {
                if (root.foldout && seeHim)
                {
                    DisplayAllDirectoriesRecursive(child, count + 1);
                }
                else
                {
                    DisplayAllDirectoriesRecursive(child, count + 1, false);
                }
               
            }
        }
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