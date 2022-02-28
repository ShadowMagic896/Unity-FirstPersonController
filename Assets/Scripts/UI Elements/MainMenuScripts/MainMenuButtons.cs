using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject OptionsMenu;

    public void LoadGame() {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        ); //  Load next scene in queue
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void OpenOptionsMenu() {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu() {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
}
