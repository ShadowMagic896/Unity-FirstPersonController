using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorController : MonoBehaviour
{
    private int Armor = 100;
    [SerializeField] private Text ShownArmor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShownArmor.text = " Armor: " + Armor;

        if (Input.GetKeyDown(KeyCode.G)){
            Armor--;
        }
        if (Input.GetKeyDown(KeyCode.H)){
            Armor++;
        }
    }
}
