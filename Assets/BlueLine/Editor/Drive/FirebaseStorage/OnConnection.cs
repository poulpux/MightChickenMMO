using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public partial class ConnectWindow 
{
    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    FirebaseStorage storage;
    FirebaseAuth auth;
    MyDirectory toolBoxImportFolder;
    private string BlueLineImportAdress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport"; // Chemin du dossier de destination

    string txtImportContent;

    ///////////////////////////////////////////Permet de signUp
    private async void CheckEmailExistsAndSignUp(string email, string password)
    {
        storage = FirebaseStorage.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        try
        {
            // Tente de créer l'utilisateur avec l'e-mail et le mot de passe fournis
            var signUpTask = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            // L'utilisateur est inscrit avec succès
            FirebaseUser newUser = signUpTask.User;
            Debug.Log("Inscription réussie pour l'utilisateur : " + newUser.Email);
        }
        catch (AggregateException ex)
        {
            foreach (var innerException in ex.InnerExceptions)
            {
                if (innerException is FirebaseException firebaseEx)
                {
                    if (firebaseEx.Message.Contains("EMAIL_EXISTS"))
                    {
                        Debug.LogWarning("L'adresse e-mail existe déjà dans Firebase Authentication.");
                        break;
                    }
                }
            }
        }
    }

    public void LoginWithEmailAndGetDrive(string email, string password)
    {
        storage = FirebaseStorage.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        // Connexion avec l'adresse e-mail et le mot de passe
        var signInTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        signInTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Connexion échouée : " + task.Exception);
                return;
            }

            // Connexion réussie, récupération de l'utilisateur depuis la tâche
            Connected = true;
            FirebaseUser user = auth.CurrentUser;
            Debug.Log("Utilisateur connecté : " + user.Email);
            GetDriveAsync();
        });
    }

    private async void GetDriveAsync()
    {
        await GetBlueLineImportAsync();
        await GetBlueLinePerso();
    }

    private async Task GetBlueLineImportAsync()
    {
        Directory.CreateDirectory(BlueLineImportAdress);
        Directory.CreateDirectory(BlueLineImportAdress+ "/FireStoreStorage");
        await ExploreFirebaseStorageAsync("BlueLineImport/FireStoreStorage/BlueLineImport.txt");
        List<string> links = LireFichierTexte(BlueLineImportAdress + "/FireStoreStorage/BlueLineImport.txt");
        foreach (var item in links)
        {
            await ExploreFirebaseStorageAsync("BlueLineImport/" + item);
        }
    }

    private async Task GetBlueLinePerso()
    {
        Directory.CreateDirectory(BlueLineImportAdress + "/BlueLinePerso");
        await ExploreFirebaseStorageAsync("BlueLinePerso/"+auth.CurrentUser.UserId+"/BlueLinePerso.txt", true);
        List<string> links = LireFichierTexte(BlueLineImportAdress + "/FireStoreStorage/BlueLinePerso.txt");
        foreach (var item in links)
        {
            if (item != null)
                await ExploreFirebaseStorageAsync("BlueLinePerso/" + auth.CurrentUser.UserId + "/" + item, true);
        }
    }

    private void SetNewBlueLineImport()
    {
        if (auth.CurrentUser != null)
        {
            WritePathImport();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string ImportationFire = "BlueLineImport";
            ExportPersonnalFolder(desktopPath + "/" + "BlueLineImport", ImportationFire);
        }
    }

    private void SetNewBlueLinePerso()
    {
        WritePathPerso();
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string PersoFire = "BlueLinePerso/" + auth.CurrentUser.UserId ;
        ExportPersonnalFolder(desktopPath + "/" + "BlueLineImport/BlueLinePerso/", PersoFire, true);
        ExportPersonnalFolder(desktopPath + "/" + "BlueLineImport/FireStoreStorage/", PersoFire, true);
    }
}
