using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public partial class ConnectWindow
{
    string pathBlueLinePerso = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport/FireStoreStorage/BlueLinePerso.txt";
    private async Task ExploreFirebaseStorageAsync(string fileStoragePath, bool perso = false)
    {
        // Obtiens une référence au fichier dans Firebase Storage
        StorageReference fileRef = storage.GetReference(fileStoragePath);
        if (fileStoragePath[fileStoragePath.Length - 1] != '/')
        {
            try
            {
                if (fileStoragePath == "BlueLineImport/FireStoreStorage/BlueLineImport.txt")
                {
                    var downloadUrl = await fileRef.GetDownloadUrlAsync();
                    int counter = NombreCaracteresAvantSlash(fileStoragePath);
                    await DownloadFile(downloadUrl.ToString(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileStoragePath.Remove(fileStoragePath.Length - counter - 1), perso);
                }

                if ((!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileStoragePath)&& !perso )|| !File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport/BlueLinePerso/" + fileStoragePath.Substring("BlueLinePerso//".Length + auth.CurrentUser.UserId.Length)) && perso)
                {
                    // Obtient l'URL de téléchargement pour le fichier
                    var downloadUrl = await fileRef.GetDownloadUrlAsync();
                    int counter = NombreCaracteresAvantSlash(fileStoragePath);
                    string a = fileStoragePath[fileStoragePath.Length - 1].ToString();
                    if (!perso)
                    {
                        await DownloadFile(downloadUrl.ToString(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + fileStoragePath.Remove(fileStoragePath.Length - counter - 1), perso);
                    }
                    else
                    {
                        string realPath = fileStoragePath.Substring("BlueLinePerso//".Length + auth.CurrentUser.UserId.Length);

                        if (fileStoragePath.EndsWith("BlueLinePerso.txt"))
                            await DownloadFile(downloadUrl.ToString(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport/FireStoreStorage");
                        else
                            await DownloadFile(downloadUrl.ToString(), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BlueLineImport/BlueLinePerso/" + fileStoragePath.Substring("BlueLinePerso//".Length + auth.CurrentUser.UserId.Length), perso);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("erreur : " + fileStoragePath);
                Debug.LogError("Erreur lors de la récupération de l'URL de téléchargement : " + ex.Message);
            }
        }
        else
        {
            if (!perso)
            {
                Directory.CreateDirectory(BlueLineImportAdress + "/" + fileStoragePath.Substring("BlueLineImport/".Length));
            }
            else
            {
                Directory.CreateDirectory(BlueLineImportAdress + "/BlueLinePerso/" + fileStoragePath.Substring("BlueLinePerso/".Length  + auth.CurrentUser.UserId.Length + 1));
            }
        }
    }

    static int NombreCaracteresAvantSlash(string texte)
    {
        int count = 0;

        // Parcours de la chaîne en commençant par la fin
        for (int i = texte.Length - 1; i >= 0; i--)
        {
            if (texte[i] != '/')
            {
                count++;
            }
            else
            {
                break; // Sortir de la boucle si on rencontre '/'
            }
        }

        return count;
    }

    private async Task DownloadFile(string url, string destinationFolder, bool perso = false)
    {
        try
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                await webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // Récupère le nom du fichier depuis l'URL et élimine les paramètres d'URL
                    Uri uri = new Uri(url);
                    string fileName = Path.GetFileName(uri.LocalPath);
                    string filePath = "";
                    if (perso)
                    {
                        Debug.Log("real file path : " + destinationFolder.Substring(0, destinationFolder.Length - fileName.Length));
                        filePath = Path.Combine(destinationFolder.Substring(0, destinationFolder.Length - fileName.Length), fileName); // Chemin complet du fichier de destination
                    }
                    else
                       filePath = Path.Combine(destinationFolder, fileName); // Chemin complet du fichier de destination
                    File.WriteAllBytes(filePath, webRequest.downloadHandler.data);

                    Debug.Log("Téléchargement du fichier terminé : " + filePath);
                }
                else
                {
                    Debug.LogError("Erreur lors du téléchargement du fichier : " + webRequest.error);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur : " + ex.Message);
        }
    }

    List<string> LireFichierTexte(string cheminFichier)
    {
        List<string> lignes = new List<string>();
        // Vérifier si le fichier existe
        if (File.Exists(cheminFichier))
        {
            // Lire toutes les lignes du fichier et les ajouter à la liste
            using (StreamReader sr = new StreamReader(cheminFichier))
            {
                string ligne;
                while ((ligne = sr.ReadLine()) != null)
                {
                    lignes.Add(ligne);
                }
            }
        }
        else
        {
            Debug.LogWarning("Le fichier n'existe pas.");
        }
        return lignes;
    }
}
