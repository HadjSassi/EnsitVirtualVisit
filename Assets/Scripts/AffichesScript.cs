using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffichesScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Obsolete("Obsolete")]
    void Start()
    {
        StartCoroutine(Main.Instance.Web.GetAffiches());
        //todo store here the list of the affiches
        //todo get the all affiches
        //for each affiche modify the picture with the proper one
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
