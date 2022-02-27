using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTextController : MonoBehaviour
{
    [SerializeField] private Text ShownHealth;

    void Start() {
        SetHealth(100);
    }

    public void SetHealth(int Health) {
        ShownHealth.text = " HEALTH: " + Health;
    }
}
