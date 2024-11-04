
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public float hoverAmount;
    public Unit unitPlaced;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void OnMouseDown()
    {
        GameMaster gameMaster = GameMaster.getInstance();
        List<OnTileClickedStrategy> clickedStrategies = new List<OnTileClickedStrategy>();
        clickedStrategies.Add(new MovementCandidatesOnTileClick());
        clickedStrategies.Add(new MovementStrategyOnTileClick());

        clickedStrategies.ForEach(strategy => { 
            if(strategy.IsApplicableStrategy(this))
            {
                strategy.ExecuteStrategy(this);
            }
        });
        
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
        return true;
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
