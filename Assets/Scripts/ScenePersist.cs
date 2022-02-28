using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    private int _numScenePersist;

    private void Awake()
    {   
        _numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (_numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
