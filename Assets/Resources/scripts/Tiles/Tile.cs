
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public float hoverAmount;
    public Unit unitPlaced;
    public Obstacles obstacles;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void OnMouseDown()
    {
        GameMaster gameMaster = GameMaster.getInstance();
        List<OnTileClickedStrategy> clickedStrategies = new List<OnTileClickedStrategy>();

        clickedStrategies.Add(new AttackTile());
        clickedStrategies.Add(new MovementStrategyOnTileClick());
        clickedStrategies.Add(new MovementCandidatesOnTileClick());

        foreach (OnTileClickedStrategy onTileClickedStrategy in clickedStrategies)
        {
            if (onTileClickedStrategy.IsApplicableStrategy(this))
            {
                onTileClickedStrategy.ExecuteStrategy(this);
            }
        }
    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
    }

    void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
    }

    public bool IsClear()
    {
        return unitPlaced == null && obstacles == null;
    }

    public void Highlight(Color color)
    {
        spriteRenderer.color = color;
    }

    public void CleanHighLight()
    {
        spriteRenderer.color = Color.white;
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white;
    }
}
