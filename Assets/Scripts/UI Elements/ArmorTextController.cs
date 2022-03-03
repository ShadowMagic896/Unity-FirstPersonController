using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorTextController : MonoBehaviour
{
    [SerializeField] private Text ShownArmor;
    [SerializeField] private int StartingArmor;

    void Start() {
        SetArmor(StartingArmor);
    }

    public void SetArmor(int Armor) {
        ShownArmor.text = " ARMOR: " + Armor;
    }
}
