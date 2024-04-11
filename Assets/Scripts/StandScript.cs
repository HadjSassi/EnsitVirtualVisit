using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BackEnd.Model;
using UnityEngine;

public class StandScript : MonoBehaviour
{
    private List<Stand> standsList = new List<Stand>();
    private List<Stand> standsLocalisation1 = new List<Stand>();
    private List<Stand> standsLocalisation2 = new List<Stand>();
    private GameObject[] standsObjectsListLocalisation1;
    private GameObject[] standsObjectsListLocalisation2;
    private string standTag1 = "stand1";
    private string standTag2 = "stand2";

    [Obsolete("Obsolete")]
    void Start()
    {
        StartCoroutine(Main.Instance.Web.GetStands(OnStandsReceived));
    }

    [Obsolete("Obsolete")]
    void OnStandsReceived(List<Stand> stands)
    {
        standsList = stands;
        SeparateStandsByLocalisation();
        
        this.standsObjectsListLocalisation1 = GameObject.FindGameObjectsWithTag(standTag1);
        this.standsObjectsListLocalisation2 = GameObject.FindGameObjectsWithTag(standTag2);
        int i = 0;
        int nb = standsObjectsListLocalisation1.Length;
        //todo if the number of the affiches is less than nb you need to set one par default !
        foreach (GameObject obj in standsObjectsListLocalisation1)
        {
            Transform frontBanner = obj.transform.Find("Layer0_001/Quad");
            Transform interactable = obj.transform.Find("Position/Interaction");
            CurrentCanvaScript currentCanvaScript = interactable.GetComponent<CurrentCanvaScript>();
            if (frontBanner != null)
            {
                Renderer renderer = frontBanner.GetComponent<Renderer>();
                if (renderer != null && renderer.materials.Length > 0)
                {
                    Material material = renderer.materials[0];
                    if (material != null)
                    {
                        StartCoroutine(ChangeMaterialTexture(material, standsLocalisation1[i].image));
                        currentCanvaScript.CurrentStand = standsLocalisation1[i];
                        currentCanvaScript.typeObject = 2;
                        currentCanvaScript.typeAffiche = "Stand";
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
        /*foreach (GameObject obj in standsObjectsListLocalisation2)
        {
            Transform frontBanner = obj.transform.Find("Component#33_001/Quad");
            MeshRenderer meshRenderer = frontBanner.GetComponent<MeshRenderer>();
            Transform interactable = obj.transform.Find("Position/Interaction");
            CurrentCanvaScript currentCanvaScript = interactable.GetComponent<CurrentCanvaScript>();

            if (meshRenderer != null && meshRenderer.materials.Length >= 3)
            {
                Material material = meshRenderer.materials[2];
                if (material != null)
                {
                    StartCoroutine(ChangeMaterialTexture(material, standsLocalisation2[i].image)); 
                    currentCanvaScript.CurrentStand = standsLocalisation2[i];
                    currentCanvaScript.typeObject = 2;
                    currentCanvaScript.typeAffiche = "Stands";
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
        }*/
        foreach (GameObject obj in standsObjectsListLocalisation2)
        {
            Transform frontBanner = obj.transform.Find("Component#33_001/Quad");
            Transform interactable = obj.transform.Find("Position/Interaction");
            CurrentCanvaScript currentCanvaScript = interactable.GetComponent<CurrentCanvaScript>();
            if (frontBanner != null)
            {
                Renderer renderer = frontBanner.GetComponent<Renderer>();
                if (renderer != null && renderer.materials.Length > 0)
                {
                    Material material = renderer.materials[0];
                    if (material != null)
                    {
                        StartCoroutine(ChangeMaterialTexture(material, standsLocalisation2[i].image));
                        currentCanvaScript.CurrentStand = standsLocalisation2[i];
                        currentCanvaScript.typeObject = 2;
                        currentCanvaScript.typeAffiche = "Stand";
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
    }

    void SeparateStandsByLocalisation()
    {
        foreach (Stand obj in standsList)
        {
            if (obj.standType == 1)
                standsLocalisation1.Add(obj);
            else if (obj.standType == 2)
                standsLocalisation2.Add(obj);
        }
    }

    
    [Obsolete("Obsolete")]
    IEnumerator ChangeMaterialTexture(Material material, string imagePath)
    {
        Texture2D texture = LoadTextureFromFile(imagePath);
        if (texture != null)
        {
            material.mainTexture = texture;
            material.SetFloat("_Mode", 1);
        }
        else
        {
            Debug.LogError("Failed to load texture from path: " + imagePath);
        }

        yield return null;
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