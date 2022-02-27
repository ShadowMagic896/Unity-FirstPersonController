using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorTextController : MonoBehaviour
{
    [SerializeField] private Text ShownArmor;

    void Start() {
        SetArmor(100);
    }

    public void SetArmor(int Armor) {
        ShownArmor.text = " ARMOR: " + Armor;
    }
}
