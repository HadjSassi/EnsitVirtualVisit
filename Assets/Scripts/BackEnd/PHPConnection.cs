using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd.Model;
using UnityEngine;
using UnityEngine.Networking;
using Avatar = BackEnd.Model.Avatar;

public class Web : MonoBehaviour
{
    
    private List<Affiche> affichesList = new List<Affiche>();
    private List<Stand> standsList = new List<Stand>();
    private Avatar _avatar = new Avatar();
    
    private void Start()
    {
        // StartCoroutine(GetDate());
        // StartCoroutine(Login("testuser","123456"));
    }

    [Obsolete("Obsolete")]
    IEnumerator GetDate()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/test.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }
    
    
    [Obsolete("Obsolete")]
    IEnumerator GetUsers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/test.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                byte[] results = www.downloadHandler.data;
            }
        }
    }


    
    [Obsolete("Obsolete")]
    public IEnumerator Login(string username, string pass)
    {

        WWWForm form = new WWWForm();
        form.AddField("loginUser",username);
        form.AddField("loginPass",pass);
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:1234/login.php",form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    [Obsolete("Obsolete")]
    public IEnumerator GetAffiches(Action<List<Affiche>> callback)
    {
        using (WWW www = new WWW("http://localhost:1234/affiches.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Error fetching affiches: " + www.error);
            }
            else
            {
                ParseJSONToAffiches(www.text);
                callback?.Invoke(affichesList); 
            }
        }
    }
    
    void ParseJSONToAffiches(string jsonData)
    {
        string[] lines = jsonData.Split("<br>");
        foreach (string line in lines)
        {
            string[] data = line.Split(" || ");
            if (data.Length >= 8)
            {
                Affiche affiche = new Affiche();
                affiche.idAffiche = int.Parse(data[0].Trim());
                affiche.titre = data[1].Trim();
                affiche.sujet = data[2].Trim();
                affiche.description = data[3].Trim();
                affiche.localisationAffiche = int.Parse(data[4].Trim());
                affiche.image = data[5].Trim();
                affiche.couverture = data[6].Trim();
                affiche.prix = int.Parse(data[7].Trim());
                affiche.lien = data[8].Trim();
                affichesList.Add(affiche);
            }
        }
    }

    
    [Obsolete("Obsolete")]
    public IEnumerator GetStands(Action<List<Stand>> callback)
    {
        using (WWW www = new WWW("http://localhost:1234/stand.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Error fetching stands: " + www.error);
            }
            else
            {
                ParseJSONToStands(www.text);
                callback?.Invoke(standsList); 
            }
        }
    }
    
    void ParseJSONToStands(string jsonData)
    {
        string[] lines = jsonData.Split("<br>");
        foreach (string line in lines)
        {
            string[] data = line.Split(" || ");
            if (data.Length >= 8)
            {
                Stand obj = new Stand();
                obj.idStand = int.Parse(data[0].Trim());
                obj.nom = data[1].Trim();
                obj.sujet = data[2].Trim();
                obj.description = data[3].Trim();
                obj.standType = int.Parse(data[4].Trim());
                obj.image = data[5].Trim();
                obj.prix = int.Parse(data[6].Trim());
                obj.lien = data[7].Trim();
                standsList.Add(obj);
            }
        }
    }
    
    
    
    [Obsolete("Obsolete")]
    public IEnumerator GetAvatar(string url, Action<Avatar> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/avatar.php?url=" + url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Avatar avatar = ParseJSONToAvatar(www.downloadHandler.text);
                callback?.Invoke(avatar);
            }
        }
    }

    Avatar ParseJSONToAvatar2(string jsonData)
    {
        string[] lines = jsonData.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] data = line.Split(new string[] { " || " }, StringSplitOptions.None);
            if (data.Length >= 8)
            {
                Avatar avatar = new Avatar();
                avatar.avatarName = data[0].Trim();
                avatar.description = data[1].Trim();
                avatar.jokes = data[2].Trim();
                avatar.existant = bool.Parse(data[3].Trim());
                avatar.npc = bool.Parse(data[4].Trim());
                avatar.sexe = data[5].Trim();
                avatar.mail = data[6].Trim();
                return avatar; // Return the created Avatar instance
            }
        }

        return null; // Return null if no valid avatar data found
    }
    
    
    Avatar ParseJSONToAvatar(string jsonData)
    {
        string[] lines = jsonData.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] data = line.Split(new string[] { " || " }, StringSplitOptions.None);
            if (data.Length >= 7) // Ensure that data array has at least 7 elements
            {
                Avatar avatar = new Avatar();
                avatar.avatarName = data[0].Trim();
                avatar.description = data[1].Trim();
                avatar.jokes = data[2].Trim();
                avatar.existant = Int32.Parse(data[3].Trim()) == 1;
                avatar.npc = Int32.Parse(data[4].Trim()) == 1;
                avatar.sexe = data[5].Trim();
                avatar.mail = data[6].Trim();
        
                // Log parsed avatar data
                // Add logging for other avatar attributes
                return avatar; // Return the created Avatar instance
            }
        }
        return null; // Return null if no valid avatar data found
    }



    
}