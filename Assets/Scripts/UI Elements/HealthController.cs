using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private int Health = 100;
    [SerializeField] private Text ShownHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)){
            Health--;
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            Health++;
        }

        ShownHealth.text = " HEALTH: " + Health;
    }
}
