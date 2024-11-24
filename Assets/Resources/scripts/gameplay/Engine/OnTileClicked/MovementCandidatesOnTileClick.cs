public class MovementCandidatesOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        tile.unitPlaced.ShowMovementCadidates();
        GameMaster gm = GameMaster.getInstance();

        if (tile.unitPlaced is AllyUnit)
        {
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
        return !gameMaster.isSystemBusy && gameMaster.selectedUnit == null && tile.unitPlaced != null && tile.unitPlaced.CanMove();
    }
}
