using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerTextController : MonoBehaviour
{
    [SerializeField] private Text ShownEnergy;
    [SerializeField] private int StartingEnergy;

    void Start() {
        SetEnergy(StartingEnergy);
    }
    
    public void SetEnergy(int Energy) {
        ShownEnergy.text = " ENERGY: " + Energy;
    }
}
