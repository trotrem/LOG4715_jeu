using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(Restart);
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}