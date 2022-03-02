using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    [SerializeField] Rigidbody player;
    [SerializeField] AudioSource audioSource;

    FunctionsList Funcs = Scripts.Functions.FunctionList.Functions;
    
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Funcs.Abs(player.velocity.x) > 1 || Funcs.Abs(player.velocity.z) > 1){
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }

    void FixedUpdate() {

    }
}
