using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextController : MonoBehaviour
{
    [SerializeField] private Text ShownHealth;
    [SerializeField] private int StartingHealth;

    void Start() {
        SetHealth(StartingHealth);
    }

    public void SetHealth(int Health) {
        ShownHealth.text = " HEALTH: " + Health;
    }
}
