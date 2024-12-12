using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnTileClickedStrategy
{
    bool IsApplicableStrategy(Tile tile);

    void ExecuteStrategy(Tile tile);
}
