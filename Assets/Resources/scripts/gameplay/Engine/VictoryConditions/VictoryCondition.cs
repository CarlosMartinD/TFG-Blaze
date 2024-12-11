using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class VictoryCondition : MonoBehaviour
{
    [SerializeField]
    private GameObject victory;

    [SerializeField]
    private GameObject defeat;

    [SerializeField]
    private string scene;

    [SerializeField]
    private GameObject battleUI;

    private bool gameOver = false;

    public void Update()
    {
        checkVictoryCondition();
    }

    public void checkVictoryCondition()
    {
        if(gameOver)
        {
            return;
        }

        if(AllyVictoryCondition())
        {
            victory.SetActive(true);
            battleUI.SetActive(false);
            gameOver = true;
        }
        else if (EnemyVictoryCondition())
        {
            defeat.SetActive(true);
            battleUI.SetActive(false);
            gameOver = true;

        }

    }

    public void nextScene()
    {
        Scene[] scenes = SceneManager.GetAllScenes();
        Debug.Log("entering");
        Debug.Log(scene.ToString());
        SceneManager.LoadScene(scene);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected abstract bool AllyVictoryCondition();

    protected abstract bool EnemyVictoryCondition();
}
