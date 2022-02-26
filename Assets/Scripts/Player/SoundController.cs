using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    public static float Abs(float x){
        if (x < 0){
            return -x;
        } return x;
    }

    [SerializeField] Rigidbody player;
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Abs(player.velocity.x) > 1 || Abs(player.velocity.z) > 1){
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }

    void FixedUpdate() {

    }
}
