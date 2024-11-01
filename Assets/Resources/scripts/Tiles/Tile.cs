
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
        bool selectableMoveSquare = gameMaster.selectedUnit == null && unitPlaced != null && unitPlaced.CanMove();

        if (selectableMoveSquare)
        {
            MovementEngine.GetInstance().GetWalkableTiles(this, unitPlaced);
             gameMaster.selectedUnit = unitPlaced;
        } else if(gameMaster.selectedUnit != null)
        {
            gameMaster.selectedUnit.Move(this);
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
        return true;
    }

    public void Highlight()
    {
        spriteRenderer.color = Color.red;
    }

    public void Reset()
    {
        spriteRenderer.color = Color.white;
    }
}
