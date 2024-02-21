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

    // Ouvre la fen�tre d'importation
    public static void WindowImport()
    {
        _window = CreateWindow(ref _window, "BlueLine ImportPackage");
        DefineToolBoxHierarchy();
    }

    // Dessine la fen�tre d'importation
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
        // Cr�ation de la hi�rarchie ToolBox
        toolBoxImportFolder = new MyDirectory("BlueLine", null);
        toolBoxImportFolder.foldout = true;

        // Chemin final pour sauvegarder la hi�rarchie sur le bureau
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string finalSavePath = desktopPath + "/"+ NameDesktopToolsImport + "/" + toolBoxImportFolder.name;

        // Recherche r�cursive des dossiers
        FindDirectoryRecursive(toolBoxImportFolder, finalSavePath);

        toolBoxPersoFolder = new MyDirectory("Dossier Perso", null);
        toolBoxPersoFolder.foldout = true;

        // Chemin final pour sauvegarder la hi�rarchie sur le bureau
        string finalSavePathPerso = desktopPath + "/" + NameDesktopToolsImport + "/BlueLinePerso";

        Debug.Log( "path perso : "+ finalSavePathPerso);
        // Recherche r�cursive des dossiers
        FindDirectoryRecursive(toolBoxPersoFolder, finalSavePathPerso);
    }

    public void DisplayAllDirectoriesRecursive(MyDirectory root, int count = 0, bool seeHim = true)
    {
        // Affichage r�cursif des fichier
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

    // Bouton d'importation de tous les packages s�lectionn�s
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
        // Importe le prochain package de la liste s�lectionn�e
        if (listOfSelection.Count > 0)
        {
            MyDirectory nextPackage = listOfSelection[0];
            listOfSelection.RemoveAt(0);

            // Importation du package
            AssetDatabase.ImportPackage(nextPackage.path, true);

            // V�rification si la fen�tre d'import est active
            EditorApplication.update += CheckIfImportWindowIsActive;
        }
        else
        {
            //Debug.Log("Tous les packages ont �t� import�s.");
        }
    }

    private void CheckIfImportWindowIsActive()
    {
        if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Import Unity Package")
        {
            timer += Time.deltaTime / 100;
            // Attendez que la fen�tre d'importation se ferme
            return;
        }
        else if (timer > 0.02f)
        {
            timer = 0;
            // Si la fen�tre d'importation est ferm�e, passez au prochain package
            EditorApplication.update -= CheckIfImportWindowIsActive;
            ImportNextPackage();
        }
    }

    // Changement de couleur de police pour la s�lection
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

    // R�cursivit� pour afficher tous les r�pertoires
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