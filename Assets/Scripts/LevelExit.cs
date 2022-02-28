using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;

    private int _nextSceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (_nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            _nextSceneIndex = 0;
        }

        SceneManager.LoadScene(_nextSceneIndex);
    }
}
