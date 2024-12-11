using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnEngine : MonoBehaviour
{

    public MapEngine mapEngine;

    public GameMaster master;

    public Image passTurnImage;

    public TextMeshProUGUI turnText;

    private Color passTurnImageColor;

    public bool enemyTurn;

    public void Start()
    {
        passTurnImageColor = passTurnImage.color;
    }

    public void EndTurnPlayer()
    {
        if(enemyTurn)
        {
            return;
        }

        enemyTurn = true;
        cleanTurn();

        passTurnImage.color = Color.grey;
        turnText.text = "ENEMY TURN";

        EnemyAI enemyAI = FindObjectOfType<EnemyAI>();
        enemyAI.OnTurnEnemyStarted();
    }

    public void EndTurnAI()
    {
        enemyTurn = false;
        turnText.text = "PLAYER TURN";

        cleanTurn();
        passTurnImage.color = Color.white;
    }

    private void cleanTurn()
    {
        master.selectedUnit = null;

        foreach (Unit unit in mapEngine.allyUnits)
        {
            unit.ResetUnit();
        }

        foreach (Unit unit in mapEngine.enemyUnits)
        {
            unit.ResetUnit();
        }
    }
}
