using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SetNewBlueLineImport : MonoBehaviour
{
    [SerializeField] string email, password;

    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    FirebaseStorage storage;
    FirebaseAuth auth;
    MyDirectory toolBoxImportFolder;
    private string BlueLineImportAdress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport"; // Chemin du dossier de destination

    protected static string NameDesktopToolsImport = "BlueLineImport";
    string txtImportContent;
    bool pasRep;
    // Start is called before the first frame update
    void Start()
    {
        LoginWithEmail(email, password);
    }

    // Update is called once per frame
    void Update()
    {
        if(!pasRep)
            SetBlueLineImport();
    }
    public void LoginWithEmail(string email, string password)
    {
        storage = FirebaseStorage.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        // Connexion avec l'adresse e-mail et le mot de passe
        var signInTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        signInTask.ContinueWithOnMainThread(task =>
        {

            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Connexion �chou�e : " + task.Exception);
                return;
            }

            // Connexion r�ussie, r�cup�ration de l'utilisateur depuis la t�che
            FirebaseUser user = auth.CurrentUser;
            Debug.Log("Utilisateur connect� : " + user.Email);
        });
    }

    private void SetBlueLineImport()
    {
        if (auth.CurrentUser != null && !pasRep && Time.time > 1f)
        {
            WritePathImport();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string ImportationFire = "BlueLineImport";
            string PersoFire = "BlueLinePerso/" + auth.CurrentUser.UserId + "/";
            ExportPersonnalFolder(desktopPath + "/" + "BlueLineImport", ImportationFire);
            pasRep = true;
        }
    }
    private void WritePathImport()
    {
        txtImportContent = string.Empty;
        DefineToolBoxHierarchy(ref txtImportContent);
        WriteTextToFile(desktopPath + "/" + NameDesktopToolsImport + "/FireStoreStorage/BlueLineImport.txt", txtImportContent);
        RemplacerCaractere(desktopPath + "/" + NameDesktopToolsImport + "/FireStoreStorage/BlueLineImport.txt");
    }

    private void ExportPersonnalFolder(string computerPath, string fireStoragePath)
    {
        // Chemin du dossier local que tu souhaites uploader
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string finalSavePath = desktopPath + "/" + "BlueLineImport";
        // string localFolderPath = Application.dataPath + "/BlueLine"; // Change le nom du dossier ici

        string folderPath = "BlueLineImport";
        //string folderPath = "BlueLinePerso/" + auth.CurrentUser.UserId + "/";
        Task.Run(async () =>
        {
            bool folderExists = await CheckIfFolderExists(folderPath);

            if (!folderExists)
            {
                // Le dossier n'existe pas, donc tu peux le cr�er
                await CreateFolder(folderPath);
                Debug.Log("Le dossier a �t� cr�� avec succ�s : " + folderPath);
            }
            else
            {
                Debug.Log("Le dossier existe d�j� : " + folderPath);
                // Appel de la m�thode pour uploader le dossier
            }
            await UploadFolder(computerPath, fireStoragePath); // Change le chemin dans Firebase Storage ici
        });
    }

    private async Task CreateFolder(string folderPath)
    {
        try
        {
            // Cr�e une r�f�rence au dossier sp�cifi� dans Firebase Storage
            StorageReference folderRef = storage.GetReference(folderPath);

            // Cr�e le dossier
            await folderRef.PutBytesAsync(new byte[] { 0 });
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Erreur lors de la cr�ation du dossier : " + ex.Message);
        }
    }

    async Task UploadFile(string localFilePath, string storagePath)
    {
        try
        {
            // Lecture du fichier local en tableau de bytes
            byte[] fileData = File.ReadAllBytes(localFilePath);

            // Nom du fichier dans Firebase Storage (peut �tre le m�me que le fichier local)
            string fileName = Path.GetFileName(localFilePath);

            // Chemin complet dans Firebase Storage pour le fichier
            string storageFilePath = storagePath + "/" + fileName;

            // Cr�ation de la r�f�rence au fichier dans Firebase Storage
            var storageReference = storage.GetReference(storageFilePath);

            // Cr�ation de la t�che d'upload en utilisant PutBytesAsync
            var uploadTask = storageReference.PutBytesAsync(fileData);

            // Attente de la fin de l'upload
            await uploadTask;

            if (uploadTask.IsCompleted && !uploadTask.IsFaulted)
            {
                Debug.Log("Fichier " + fileName + " upload� avec succ�s !");
            }
            else
            {
                Debug.LogError("Erreur lors de l'upload du fichier " + fileName + " : " + uploadTask.Exception);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur lors de l'upload du fichier : " + e.Message);
        }
    }

    async Task UploadFolder
        (string localFolderPath, string storagePath)
    {
        try
        {
            // Liste des fichiers dans le dossier local

            string[] files = Directory.GetFiles(localFolderPath);
            foreach (string file in files)
            {
                await UploadFile(file, storagePath);
            }

            string[] directories = Directory.GetDirectories(localFolderPath);
            foreach (string directory in directories)
            {
                // Nom du sous-dossier dans Firebase Storage (peut �tre le m�me que le sous-dossier local)
                string directoryName = Path.GetFileName(directory);

                // V�rifie si le dossier appartient au r�pertoire .git, si oui, ignore-le
                if (!directoryName.Contains(".git"))
                {
                    // Chemin complet dans Firebase Storage pour le sous-dossier
                    string storageDirectoryPath = storagePath + "/" + directoryName;

                    // Appel r�cursif pour uploader le sous-dossier
                    await UploadFolder(directory, storageDirectoryPath);
                }
            }

            Debug.Log("Tous les fichiers et dossiers du dossier ont �t� upload�s !");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erreur lors de l'upload du dossier : " + e.Message);
        }
    }

    private async Task<bool> CheckIfFolderExists(string folderPath)
    {
        try
        {
            // R�cup�re une r�f�rence au dossier sp�cifi� dans Firebase Storage
            StorageReference folderRef = storage.GetReference(folderPath);

            // V�rifie si le dossier existe en r�cup�rant ses m�tadonn�es
            var metadata = await folderRef.GetMetadataAsync();

            // Si les m�tadonn�es sont r�cup�r�es avec succ�s, le dossier existe
            return metadata != null;
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Erreur lors de la v�rification du dossier : " + ex.Message);
            return false;
        }
    }

    private void DefineToolBoxHierarchy(ref string txt)
    {
        // Cr�ation de la hi�rarchie ToolBox
        toolBoxImportFolder = new MyDirectory("BlueLine", null);

        // Chemin final pour sauvegarder la hi�rarchie sur le bureau
        string finalSavePath = desktopPath + "/" + NameDesktopToolsImport;

        // Recherche r�cursive des dossiers
        FindDirectoryRecursive(toolBoxImportFolder, finalSavePath, ref txt);
    }

    public void FindDirectoryRecursive(MyDirectory root, string path, ref string txt)
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
                    txt += newOne.path.Substring(Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Length + 1 + "/BlueLineImport".Length) + "\n";
                    newOne.isPackage = true;
                    root.children.Add(newOne);

                    root.pathPackage.Add(packagePath);
                }

            }
        }

        //On cherche les folders
        string[] directories = Directory.GetDirectories(path);
        if (directories.Length == 0)
            return;

        foreach (string dir in directories)
        {
            string relativePath = dir.Replace(path + "\\", "");
            MyDirectory newOne = new MyDirectory(relativePath, root);
            if (root.name == "BlueLine")
                root.path = path;

            newOne.path = root.path + "/" + newOne.name;
            txt += newOne.path.Substring(Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Length + 1 + "/BlueLineImport".Length) + "/\n";
            root.children.Add(newOne);
            if (newOne.name != ".git")
                FindDirectoryRecursive(newOne, dir, ref txt);
        }
    }

    void WriteTextToFile(string path, string content)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine(content);
        }
    }

    static void RemplacerCaractere(string cheminFichierSource)
    {
        try
        {
            // Lire toutes les lignes du fichier source
            string[] lignes = File.ReadAllLines(cheminFichierSource);

            // Remplacer les '\' par '/'
            for (int i = 0; i < lignes.Length; i++)
            {
                lignes[i] = lignes[i].Replace('\\', '/');
            }

            // �crire le contenu modifi� dans un nouveau fichier
            File.WriteAllLines(cheminFichierSource, lignes);
            Console.WriteLine("Op�ration termin�e : les '\\' ont �t� remplac�s par '/' dans le fichier.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Une erreur s'est produite : " + e.Message);
        }
    }

}
