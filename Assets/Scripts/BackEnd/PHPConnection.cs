using System;
using System.Collections;
using System.Collections.Generic;
using BackEnd.Model;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    
    private List<Affiche> affichesList = new List<Affiche>();
    
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

    
}