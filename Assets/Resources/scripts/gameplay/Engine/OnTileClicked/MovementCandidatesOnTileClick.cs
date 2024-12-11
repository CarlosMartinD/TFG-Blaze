public class MovementCandidatesOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        tile.unitPlaced.ShowMovementCadidates();
        GameMaster gm = GameMaster.getInstance();



        if (tile.unitPlaced is AllyUnit)
        {
            if (gm.selectedUnit != null)
            {
                gm.selectedUnit.RemoveMovementCandidates();
            }

            gm.selectedUnit = tile.unitPlaced;
        }
        else
        {
            gm.enemySelectedUnits.Add(tile.unitPlaced);
        }
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        return !gameMaster.isSystemBusy && tile.unitPlaced != null && !tile.unitPlaced.Equals(gameMaster.selectedUnit) && tile.unitPlaced.CanMove();
    }
}
