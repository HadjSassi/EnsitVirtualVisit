using System.Collections;
using System.Collections.Generic;
using BackEnd.Model;
using UnityEngine;

[System.Serializable]
public class CurrentAfficheScript : MonoBehaviour
{
    // Serialize the field to make it visible in the Inspector
    [SerializeField]
    private Affiche currentAffiche;

    public int idAffiche;
    public string typeAffiche;

    // Getter and setter methods for currentAffiche
    public Affiche CurrentAffiche
    {
        get { return currentAffiche; }
        set { currentAffiche = value; }
    }
}