using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStrategyOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        GameMaster.getInstance().selectedUnit.Move(tile);
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        return gameMaster.selectedUnit != null && gameMaster.selectedUnit.CanMove();
    }
}
