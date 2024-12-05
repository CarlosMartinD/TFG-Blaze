using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class VictoryCondition : MonoBehaviour
{
    public GameObject victory;

    public GameObject defeat;

    public Object scene;

    public void Update()
    {
        checkVictoryCondition();
    }

    public void checkVictoryCondition()
    {
        if(AllyVictoryCondition())
        {
            victory.SetActive(true);
        } else if (EnemyVictoryCondition())
        {
            victory.SetActive(true);
        }
    }

    public void nextScene()
    {
        SceneManager.LoadScene(scene.name);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected abstract bool AllyVictoryCondition();

    protected abstract bool EnemyVictoryCondition();
}
