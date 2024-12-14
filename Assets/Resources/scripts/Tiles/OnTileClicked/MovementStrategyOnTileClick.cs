public class MovementStrategyOnTileClick : OnTileClickedStrategy
{
    void OnTileClickedStrategy.ExecuteStrategy(Tile tile)
    {
        Unit unitToMove = SystemOperatorEngine.getInstance().selectedUnit;
        unitToMove.StartCoroutine(unitToMove.Move(tile));
    }

    bool OnTileClickedStrategy.IsApplicableStrategy(Tile tile)
    {
        SystemOperatorEngine gameMaster = SystemOperatorEngine.getInstance();
        TurnEngine turnEngine = EngineDependencyInjector.getInstance().Resolve<TurnEngine>();

        return !turnEngine.IsEnemyTurn() && !gameMaster.isSystemBusy && gameMaster.selectedUnit != null && gameMaster.selectedUnit.CanMove();
    }
}
