
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
    private StatsPanel statsPanel;
    private TileColorResolver tileColorResolver;
    private void Start()
    {
        tileColorResolver = new TileColorResolver();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onTileClickedStrategies = EngineDependencyInjector.getInstance().Resolve<List<OnTileClickedStrategy>>();
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        foreach (OnTileClickedStrategy onTileClickedStrategy in onTileClickedStrategies)
        {
            if (onTileClickedStrategy.IsApplicableStrategy(this))
            {
                onTileClickedStrategy.ExecuteStrategy(this);
                return;
            }
        }
    }

    void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Input.GetMouseButtonDown(1))
        {
            if(unitPlaced == null)
            {
                return;
            }

            if(statsPanel == null)
            {
                statsPanel = FindAnyObjectByType<StatsPanel>();
            }

            statsPanel.SubscribeTo(unitPlaced.stats);
        }
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        transform.localScale += Vector3.one * hoverAmount;
    }

    void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        transform.localScale -= Vector3.one * hoverAmount;
    }

 
    public bool IsClear()
    {
        return unitPlaced == null && obstacles == null;
    }

    public void Highlight(Unit unit, Color color)
    {
        spriteRenderer.color = tileColorResolver.AddColor(color, unit);
    }

    public void CleanHighLight(Unit unit)
    {
        spriteRenderer.color = tileColorResolver.RemoveHighLightFromCharacter(unit);
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white;
        tileColorResolver.Reset();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

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
