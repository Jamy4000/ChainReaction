using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string _sceneToLoad = "BackgroundCity";
    
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
}
