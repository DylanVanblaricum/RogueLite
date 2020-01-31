using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{

    public CoreData coreDataObject;
    public static CoreData coreData;


    // Start is called before the first frame update
    void Start()
    {
        coreData = coreDataObject;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
