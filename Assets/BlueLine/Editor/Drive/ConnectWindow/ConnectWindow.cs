using Firebase.Auth;
using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public  partial class ConnectWindow : ToolsGF
{
    static public bool Connected;

    private bool SignUp;
    protected override Vector2 defaultWindowSize { get { return new Vector2(300f, 55f * 4f); } }
    // Fenêtre
    public static ConnectWindow _window;

    private string pseudo, password;

    string visuPassword;
    string savePasseWord;

    [MenuItem("BlueLine/Connecting", false, 1)]
    public static void WindowConnect()
    {
        _window = CreateWindow(ref _window, "Connecting");
    }

    protected override void OnGUI()
    {
        if (_window == null)
        {
            _window = CreateWindow(ref _window, "Connecting");
            Start = false;
            Connected = false;
        }
        if (_window != null && !EditorApplication.isPlaying)
        {
            if (!Start)
            {
                base.OnGUI();
            }
            StartOnGUI(_window, defaultWindowSize);
            if (Start)
            {
                DisplayInterface();
            }
        }
    }

    private void DisplayInterface()
    {
        if(!Connected)
        {
            DisplayInterfaceNotConnectedAsync();
        }
        else
        {
            DisplayInterfaceConnected();
        }
    }

    private void DisplayInterfaceNotConnectedAsync()
    {

        GUILayout.Label("Email :");
        pseudo = GUILayout.TextField(pseudo);
        GUILayout.Label("Password :");
        if (!SignUp)
            password = EditorGUILayout.PasswordField(visuPassword);
        else
            savePasseWord = GUILayout.TextField(savePasseWord);

        if (!string.IsNullOrEmpty(password))
        {
            if (password[0] != '*')
            {
                savePasseWord = password;
            }
        }

        if (!SignUp)
        {
            if (GUILayout.Button("Connect"))
            {
                LoginWithEmailAndGetDrive(pseudo, savePasseWord);
            }
        }
        else
        {
            DisplayAcceptButton();
        }
        UpdatePassword();


        DisplaySignUpButton();
        PassWordHelp();
    }
    private void DisplaySignUpButton()
    {
        if (SignUp)
        {
            GUI.skin.button.normal.background = defaultButtonStylePress;
        }
        else
            GUI.skin.button.normal.background = defaultButtonStyleNotPress;
        if (GUILayout.Button("SignUp"))
        {
            SignUp = !SignUp;
            password = "";
            savePasseWord = "";
        }
        GUI.skin.button.normal.background = defaultButtonStyleNotPress;
    }

    private void PassWordHelp()
    {
        if(SignUp)
        {
            GUILayout.Label("The password must contain at least 8 characters");
        }
    }

    private void UpdatePassword()
    {
        char a = '*';
        visuPassword = "";

        if (!string.IsNullOrEmpty(password))
        {
            for (int i = 0; i < password.Length; i++)
            {
                visuPassword += a;
            }
        }
    }

    private void DisplayAcceptButton()
    {
        if (GUILayout.Button("Accept"))
        {
            CheckEmailExistsAndSignUp(pseudo, savePasseWord);
            File.Delete(pathBlueLinePerso);
            LoginWithEmailAndGetDrive(pseudo, savePasseWord);
            //SetNewBlueLinePerso();
        }
    }

    private void DisplayInterfaceConnected()
    {
        GUILayout.Label("Email :");
        GUILayout.Label(pseudo);
        if (GUILayout.Button("SendToFirebasePerso"))
        {
            SetNewBlueLinePerso();
        }
        if (auth.CurrentUser.Email == "ambroise.marquet@gmail.com")
        {
            if (GUILayout.Button("SendToFirebaseImport") )
            {
                SetNewBlueLineImport();
            }
        }
        if (GUILayout.Button("Disconnect"))
        {
            Connected = false;
            storage = FirebaseStorage.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        }
    }
    private void OnDestroy()
    {
        Start = false;
    }
}
