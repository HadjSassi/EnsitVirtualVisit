using System.Collections;
using System.Collections.Generic;
using BackEnd.Model;
using UnityEngine;
using UnityEngine.Serialization;
using Avatar = BackEnd.Model.Avatar;

[System.Serializable]
public class CurrentCanvaScript : MonoBehaviour
{
    // Serialize the field to make it visible in the Inspector
    [SerializeField]
    private Affiche currentAffiche;
    
    [SerializeField]
    private Stand currentStand;
    
    [SerializeField]
    private Avatar currentAvatar;

    public int typeObject;// 1 for poster, 2 for stands and 3 for avatars
    public string typeAffiche;

    // Getter and setter methods for currentAffiche
    public Affiche CurrentAffiche
    {
        get { return currentAffiche; }
        set { currentAffiche = value; }
    }
     public Avatar CurrentAvatar
    {
        get { return currentAvatar; }
        set { currentAvatar = value; }
    }
    
    public Stand CurrentStand
    {
        get { return currentStand; }
        set { currentStand = value; }
    }
    
    //todo same for Avatars
}