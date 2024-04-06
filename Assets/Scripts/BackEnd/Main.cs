using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    public Web Web;
    public static Main Instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Web = GetComponent<Web>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
