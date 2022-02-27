using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {
    public void LoadGame() {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        ); //  Load next scene in queue
        SceneMagager.
    }
}
