using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public TextMeshProUGUI costText;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonClicked()
    {

    }
}
