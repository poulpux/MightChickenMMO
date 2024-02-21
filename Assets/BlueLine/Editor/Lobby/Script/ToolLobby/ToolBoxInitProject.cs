using System;
using UnityEditor;
using UnityEngine;

public partial class ToolBoxLobby 
{
    // Enumération pour les configurations
    private enum CONFIG
    {
        URP,
        BASICS
    }

    // Chemins vers les packages à importer
    private string InitialisationPathURP = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/"+ NameDesktopToolsImport + "/InitialingProject/InitialingURP.unitypackage";
    private string InitialisationPathURPWithMenu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/"+ NameDesktopToolsImport + "/InitialingProject/InitialingURPWithMenu.unitypackage";
    private string InitialisationPathBasics = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/"+ NameDesktopToolsImport + "/InitialingProject/InitialingBasics.unitypackage";
    private string InitialisationPathBasicsWithMenu = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/"+ NameDesktopToolsImport + "/InitialingProject/InitialingBasicsWithMenu.unitypackage";

    // Variables pour la configuration et le menu
    private CONFIG config = CONFIG.BASICS;
    private bool MenuOk = false;

    private void OnInitProjectVisu()
    {
        // Méthode principale pour initialiser l'interface utilisateur
        LabelInitProject();
        EnumPopumConfiguration();
        ToogleOkMenu();
        ImportButton();
    }

    // Affichage du libellé pour "Init Project"
    private void LabelInitProject()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("Init Project");
        GUILayout.EndHorizontal();
    }

    // Sélection de la configuration via EnumPopup
    private void EnumPopumConfiguration()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        config = (CONFIG)EditorGUILayout.EnumPopup("Configuration : ", config);
        GUILayout.EndHorizontal();
    }

    // Activation/désactivation du menu via Toggle
    private void ToogleOkMenu()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        MenuOk = GUILayout.Toggle(MenuOk, "Menu");
        GUILayout.EndHorizontal();
    }

    // Bouton d'import
    private void ImportButton()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUI.skin.button.normal.background = defaultButtonStyleNotPress;
        if (GUILayout.Button("Import", GUILayout.Width(490), GUILayout.Height(15)))
        {
            string pathToImport;
            // Sélection du chemin du package à importer en fonction de la configuration choisie
            if (config == CONFIG.BASICS)
                pathToImport = MenuOk ? InitialisationPathBasicsWithMenu : InitialisationPathBasics;
            else
                pathToImport = MenuOk ? InitialisationPathURPWithMenu : InitialisationPathURP;

            ImportPackage(pathToImport);
            initProjectActive = !initProjectActive;
            resizeWindow(_window, defaultWindowSize);
        }

        GUILayout.EndHorizontal();
    }

    // Importation du package en utilisant AssetDatabase
    private void ImportPackage(string path)
    {
        if (System.IO.File.Exists(path))
        {
            AssetDatabase.ImportPackage(path, true);
            Debug.Log("Package Imported from: " + path);
        }
        else
        {
            Debug.LogWarning("Package file not found at: " + path);
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