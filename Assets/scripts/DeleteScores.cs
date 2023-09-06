using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteScores : MonoBehaviour
{

    [SerializeField] Button resetButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetScores()
    {
        PlayerPrefs.DeleteAll();
    }

}
