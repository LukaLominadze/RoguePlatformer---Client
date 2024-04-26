using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.MergeScenes(SceneManager.GetActiveScene(), SceneManager.GetSceneByBuildIndex(0));
    }
}
