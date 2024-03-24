using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PHPConnection : MonoBehaviour
{
    private void Start()
    {
        // StartCoroutine(GetDate());
        StartCoroutine(Login("testuser","123456"));
    }

    IEnumerator GetDate()
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


    
    IEnumerator Login(string username, string pass)
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


    
}