using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenceScript : MonoBehaviour
{
    //persistence proto start
    public static PersistenceScript Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    //persistence proto end
}
