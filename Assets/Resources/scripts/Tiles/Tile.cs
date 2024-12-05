
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerClickHandler
{
    public int x, y;
    public float hoverAmount;
    public Unit unitPlaced;
    public Obstacles obstacles;

    private List<OnTileClickedStrategy> onTileClickedStrategies;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTileClickedStrategies = EngineDependencyInjector.getInstance().Resolve<List<OnTileClickedStrategy>>();
    }

    void OnMouseDown()
    {
        foreach (OnTileClickedStrategy onTileClickedStrategy in onTileClickedStrategies)
        {
            if (onTileClickedStrategy.IsApplicableStrategy(this))
            {
                onTileClickedStrategy.ExecuteStrategy(this);
                return;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }

        if(unitPlaced == null)
        {
            return;
        }


    }
}
