using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.PackageManager.UI;
using Unity.VisualScripting;

public partial class ToolBoxLobby : ToolsGF
{
    private int numberOfButtons = 3;
    protected override Vector2 defaultWindowSize { get { return new Vector2(500f, 55f * numberOfButtons); } }

    // Booléens pour activer/désactiver les différents boutons
    private bool importFolderActive = false, exportFolderActive = false, initProjectActive = false;

    // Noms des boutons
    private string importFolderName = "Import Folder", exportFolderName = "Export Folder", initProjectName = "Init Project";

    // Fenêtre
    public static ToolBoxLobby _window;



    [MenuItem("BlueLine/Lobby",false,0)]
    public static void WindowLobby()
    {
        _window = CreateWindow(ref _window, "BlueLine Lobby");
    }

    // Méthode pour afficher l'interface utilisateur
    protected override void OnGUI()
    {
        if (_window == null)
        {
            _window = CreateWindow(ref _window, "BlueLine Lobby");
            Start = false;
        }
        if (_window != null && !EditorApplication.isPlaying)
        {
            if(!Start)
                base.OnGUI();
            StartOnGUI(_window, defaultWindowSize);
            if (Start)
            {
                AllButtonVisual();
                AllButtonUpdate();
            }
        } 
    }

    // Affiche tous les boutons visuels
    private void AllButtonVisual()
    {
        ButtonMenuVisual(importFolderName, ref importFolderActive);
        ButtonMenuVisual(exportFolderName, ref exportFolderActive);
        ButtonMenuVisual(initProjectName, ref initProjectActive);
    }

    // Affiche le visuel du bouton et gère son interaction
    private void ButtonMenuVisual(string buttonName, ref bool buttonActive)
    {
        if (buttonActive && buttonName == initProjectName)
        {
            resizeWindow(_window, new Vector2(defaultWindowSize.x, defaultWindowSize.y + 90f));
            GUI.skin.button.normal.background = defaultButtonStylePress;
        }
        else
            GUI.skin.button.normal.background = defaultButtonStyleNotPress;

        DrawButtonAndOnClick(buttonName, ref buttonActive);
    }

    // Dessine le bouton et gère l'interaction lorsqu'il est cliqué
    private void DrawButtonAndOnClick(string buttonName, ref bool buttonActive)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Space(25);
        if (GUILayout.Button(buttonName, GUILayout.Width(450), GUILayout.Height(50)))
        {
            buttonActive = !buttonActive;
            resizeWindow(_window, defaultWindowSize);
        }
        GUILayout.EndHorizontal();
    }

    // Met à jour tous les boutons
    private void AllButtonUpdate()
    {
        OnButtonUpdate(importFolderName, importFolderActive);
        OnButtonUpdate(exportFolderName, exportFolderActive);
        OnButtonUpdate(initProjectName, initProjectActive);
    }

    // Met à jour un bouton spécifique
    private void OnButtonUpdate(string buttonName, bool buttonActive)
    {
        if (!buttonActive)
            return;

        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

        if (importFolderName == buttonName)
        {
            importFolderActive = !importFolderActive;
            ImportWindow.WindowImport();
        }
        else if (exportFolderName == buttonName)
        {
            exportFolderActive = !exportFolderActive;
            ExportWindowUnity();;
        }
        else if (initProjectName == buttonName)
        {
            OnInitProjectVisu();
        }
    }

    private void OnDestroy()
    {
        Start = false;
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
