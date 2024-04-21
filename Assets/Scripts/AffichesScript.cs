using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BackEnd.Model;
using UnityEngine;

public class AffichesScript : MonoBehaviour
{
    private List<Affiche> affichesList = new List<Affiche>();
    private List<Affiche> affichesLocalisation1 = new List<Affiche>();
    private List<Affiche> affichesLocalisation2 = new List<Affiche>();
    private List<Affiche> affichesLocalisation3 = new List<Affiche>();
    private GameObject[] affichesObjectsListLocalisation1;
    private GameObject[] adBannerObjectsListLocalisation2;
    private string afficheTag = "Affiche";
    private string adBannerTag = "AdBanner";

    [Obsolete("Obsolete")]
    void Start()
    {
        StartCoroutine(Main.Instance.Web.GetAffiches(OnAffichesReceived));
    }

    [Obsolete("Obsolete")]
    void OnAffichesReceived(List<Affiche> affiches)
    {
        affichesList = affiches;
        SeparateAffichesByLocalisation();
        
        this.affichesObjectsListLocalisation1 = GameObject.FindGameObjectsWithTag(afficheTag);
        this.adBannerObjectsListLocalisation2 = GameObject.FindGameObjectsWithTag(adBannerTag);
        int i = 0;
        int nb = affichesObjectsListLocalisation1.Length;
        //todo if the number of the affiches is less than nb you need to set one par default !
        foreach (GameObject afficheObject in affichesObjectsListLocalisation1)
        {
            Transform frontBanner = afficheObject.transform.Find("Root/Banner/Banner_0/frontBanner");
            Transform interactable = afficheObject.transform.Find("Position/Interaction");
            CurrentCanvaScript currentCanvaScript = interactable.GetComponent<CurrentCanvaScript>();
            if (frontBanner != null)
            {
                Renderer renderer = frontBanner.GetComponent<Renderer>();
                if (renderer != null && renderer.materials.Length > 0)
                {
                    Material material = renderer.materials[0];
                    if (material != null)
                    {
                        Action<Sprite> getAfficheCouvertureCallback = (downloadedSprite) =>
                        {
                            if (downloadedSprite != null)
                            {
                                Material newMaterial = new Material(material);
                                newMaterial.mainTexture = downloadedSprite.texture;
                                renderer.material = newMaterial;
                            }
                            else
                            {
                                Debug.LogError("Failed to get affiche image.");
                            }
                        };
                        StartCoroutine(Main.Instance.Web.GetAfficheImage(affichesLocalisation1[i].couverture, getAfficheCouvertureCallback));

                        currentCanvaScript.CurrentAffiche = affichesLocalisation1[i];
                        currentCanvaScript.typeObject = 1;
                        currentCanvaScript.typeAffiche = "Affiche";
                    } else
                    {
                        print("Material is null.");
                    }
               
                } else
                {
                    print("Renderer is null or has no materials.");
                }
            } else
            {
                print("frontBanner not found.");
            }

            i++;
        }

        i = 0;
        foreach (GameObject adBannerObject in adBannerObjectsListLocalisation2)
        {
            MeshRenderer meshRenderer = adBannerObject.GetComponent<MeshRenderer>();
            Transform interactable = adBannerObject.transform.Find("Position/Interaction");
            CurrentCanvaScript currentCanvaScript = interactable.GetComponent<CurrentCanvaScript>();

            if (meshRenderer != null && meshRenderer.materials.Length >= 3)
            {
                Material material = meshRenderer.materials[2];
                if (material != null)
                {
                    Action<Sprite> getAfficheCouvertureCallback = (downloadedSprite) =>
                    {
                        if (downloadedSprite != null)
                        {
                            material.mainTexture = downloadedSprite.texture;
                        }
                        else
                        {
                            Debug.LogError("Failed to get affiche image.");
                        }
                    };
                    StartCoroutine(Main.Instance.Web.GetAfficheImage(affichesLocalisation2[i].couverture, getAfficheCouvertureCallback));

                    currentCanvaScript.CurrentAffiche = affichesLocalisation2[i];
                    currentCanvaScript.typeObject = 1;
                    currentCanvaScript.typeAffiche = "Affiche";
                }
                else
                {
                    Debug.LogError("Material is null.");
                }
            }
            else
            {
                Debug.LogError("MeshRenderer is null or doesn't have enough materials.");
            }
        }
    }

    void SeparateAffichesByLocalisation()
    {
        foreach (Affiche affiche in affichesList)
        {
            if (affiche.localisationAffiche == 1)
                affichesLocalisation1.Add(affiche);
            else if (affiche.localisationAffiche == 2)
                affichesLocalisation2.Add(affiche);
            else if (affiche.localisationAffiche == 3)
                affichesLocalisation3.Add(affiche);
        }
    }

    
    [Obsolete("Obsolete")]
    IEnumerator ChangeMaterialTexture(Material material, string imagePath)
    {
        yield return Main.Instance.Web.GetAfficheImage(imagePath, (downloadedSprite) =>
        {
            if (downloadedSprite != null)
            {
                material.mainTexture = downloadedSprite.texture;
            }
            else
            {
                Debug.LogError("Failed to get affiche image.");
            }
        });
    }

    Texture2D LoadTextureFromFile(string imagePath)
    {
        byte[] fileData = File.ReadAllBytes(imagePath);
        if (fileData == null || fileData.Length == 0)
        {
            Debug.LogError("File data is empty or null for image: " + imagePath);
            return null;
        }

        Texture2D texture = new Texture2D(2, 2); // You might need to adjust the dimensions
        if (!ImageConversion.LoadImage(texture, fileData))
        {
            Debug.LogError("Failed to load image bytes into Texture2D for image: " + imagePath);
            return null;
        }

        return texture;
    }

    

}