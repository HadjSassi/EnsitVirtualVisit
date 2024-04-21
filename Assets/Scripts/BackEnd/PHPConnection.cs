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
    private List<Avatar> npcAvatarList = new List<Avatar>();
    private List<Avatar> avatarList = new List<Avatar>();
    private Avatar _avatar = new Avatar();
    

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
                Avatar avatar = ParseJSONToAvatar(www.downloadHandler.text,url);
                callback?.Invoke(avatar);
            }
        }
    }

    
    
    Avatar ParseJSONToAvatar(string jsonData,string url)
    {
        string[] lines = jsonData.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] data = line.Split(new string[] { " || " }, StringSplitOptions.None);
            if (data.Length > 0) 
            {
                Avatar avatar = new Avatar();
                avatar.avatarName = data[0].Trim();
                avatar.description = data[1].Trim();
                avatar.jokes = data[2].Trim();
                avatar.existant = Int32.Parse(data[3].Trim()) == 1;
                avatar.sexe = data[4].Trim();
                avatar.mail = data[5].Trim();
                avatar.url = url;
                return avatar; // Return the created Avatar instance
            }
        }
        return null; // Return null if no valid avatar data found
    }

    
    [Obsolete("Obsolete")]
    public IEnumerator GetAvatarsLists(Action<List<Avatar>, List<Avatar>> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/avatar.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                ParseJSONToAvatarLists(www.downloadHandler.text);
                callback?.Invoke(avatarList, npcAvatarList);
            }
        }
    }
    
    
    void ParseJSONToAvatarLists(string jsonData)
    {
        string[] parts = jsonData.Split(new string[] { "-*-*-"}, StringSplitOptions.RemoveEmptyEntries);

        // Ensure that there are at least two parts (NPC list and avatar list)
        if (parts.Length < 2)
        {
            Console.WriteLine("Invalid data format: Missing NPC list or avatar list.");
            return;
        }

        // Process NPC list (second part)
        string[] npcLines = parts[1].Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in npcLines)
        {
            string[] data = line.Split(new string[] { "||" }, StringSplitOptions.None);
            if (data.Length >= 7)
            {
                Avatar avatar = CreateAvatar(data);
                npcAvatarList.Add(avatar);
            }
        }

        
        // Process avatar list (first part)
        string[] avatarLines = parts[0].Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in avatarLines)
        {
            string[] data = line.Split(new string[] { "||" }, StringSplitOptions.None);
            if (data.Length >= 7)
            {
                Avatar avatar = CreateAvatar(data);
                avatarList.Add(avatar);
            }
        }

    }

    static Avatar CreateAvatar(string[] data)
    {
        Avatar avatar = new Avatar();
        avatar.url = data[0].Trim();
        avatar.avatarName = data[1].Trim();
        avatar.description = data[2].Trim();
        avatar.jokes = data[3].Trim();
        avatar.existant = data[4].Trim() == "1";
        avatar.sexe = data[5].Trim();
        avatar.mail = data[6].Trim();

        return avatar;
    }

    [Obsolete("Obsolete")]
    public IEnumerator GetAfficheImage(string imageUrl, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/GetAfficheImage.php?image="+imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
            
                // Créer une Texture2D à partir des bytes
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(bytes))
                {
                    // Créer un Sprite à partir de la Texture2D
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                    callback(sprite);
                }
                else
                {
                    Debug.LogError("Failed to load image bytes into Texture2D.");
                }
            }
        }
    }
    [Obsolete("Obsolete")]
    public IEnumerator GetStandImage(string imageUrl, System.Action<Sprite> callback)
    {
        WWWForm form = new WWWForm();
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:1234/GetStandImage.php?image="+imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] bytes = www.downloadHandler.data;
            
                // Créer une Texture2D à partir des bytes
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(bytes))
                {
                    // Créer un Sprite à partir de la Texture2D
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                    callback(sprite);
                }
                else
                {
                    Debug.LogError("Failed to load image bytes into Texture2D.");
                }
            }
        }
    }

    
}