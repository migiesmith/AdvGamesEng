using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour {

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void changeScene(int index)
    {
        Fade f = new Fade();
        f.fadeOut(1.0f);
        SceneManager.LoadScene(index);
        f.fadeIn(1.0f);
    }
}
