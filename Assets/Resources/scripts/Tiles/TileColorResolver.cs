using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorResolver
{
    private Queue<Tuple<Unit, Color>> colorPerUnit;
    private ISet<Unit> unitRegistered;

    public TileColorResolver() { 
        colorPerUnit = new Queue<Tuple<Unit, Color>>();
        unitRegistered = new HashSet<Unit>();
    }
    public Color AddColor(Color color, Unit unit)
    {

        unitRegistered.Add(unit);
        colorPerUnit.Enqueue(Tuple.Create(unit, color));
       
        return color;
    }

    public Color RemoveHighLightFromCharacter(Unit unit)
    {
        if (!unitRegistered.Contains(unit))
        {
            return colorPerUnit.Count > 0 ? colorPerUnit.Peek().Item2 : Color.white;
        }
        
        List<Tuple<Unit,Color>> elements = new List<Tuple<Unit,Color>>();

        while (colorPerUnit.Count > 0)
        {
            Tuple<Unit, Color> unitColor =  colorPerUnit.Dequeue(); 
            if(unitColor.Item1.Equals(unit))
            {
                break;
            } else
            {
                elements.Add(unitColor);
            }
        }

        foreach(Tuple<Unit, Color> unitColor in elements)
        {
            colorPerUnit.Enqueue(unitColor);
        }


        return colorPerUnit.Count > 0 ? colorPerUnit.Peek().Item2 : Color.white;
    }

    public Color Reset()
    {
        colorPerUnit.Clear();
        unitRegistered.Clear();
        return Color.white;
    }
}
