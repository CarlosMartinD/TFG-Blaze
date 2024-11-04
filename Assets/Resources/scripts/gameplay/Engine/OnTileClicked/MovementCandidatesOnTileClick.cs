public class MovementCandidatesOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        tile.unitPlaced.ShowMovementCadidates();
        GameMaster.getInstance().selectedUnit = tile.unitPlaced;
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        GameMaster gameMaster = GameMaster.getInstance();
        return gameMaster.selectedUnit == null && tile.unitPlaced != null && tile.unitPlaced.CanMove();
    }
}
