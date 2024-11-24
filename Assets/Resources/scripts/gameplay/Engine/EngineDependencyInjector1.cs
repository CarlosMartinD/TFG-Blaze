using System;
using System.Collections.Generic;
using UnityEngine;

public class EngineDependencyInjector : MonoBehaviour
{
    private Dictionary<Type, object> _dependencies = new Dictionary<Type, object>();

    private static EngineDependencyInjector instance;

    public EngineDependencyInjector()
    {
        instance = this;
    }

    public static EngineDependencyInjector getInstance()
    {
        return instance;
    }

    void Start()
    {
        CreateTileStrategies();
        CreatePathFinder();
    }

    private void CreateTileStrategies()
    {
        List<OnTileClickedStrategy> clickedStrategies = new List<OnTileClickedStrategy>();

        clickedStrategies.Add(new AttackTile());
        clickedStrategies.Add(new UnselectUnitTileStrategy());
        clickedStrategies.Add(new MovementStrategyOnTileClick());
        clickedStrategies.Add(new MovementCandidatesOnTileClick());

        Register(clickedStrategies);
    }

    private void CreatePathFinder()
    {
        Register(new PathFinder());
        Register(new MovementEngine());
    }

    public T Resolve<T>()
    {
        var type = typeof(T);

        if (_dependencies.TryGetValue(type, out var instance))
        {
            return (T)instance;
        }

        if (typeof(MonoBehaviour).IsAssignableFrom(type))
        {
            if (FindObjectOfType(type) is T foundObject)
            {
                Register(foundObject);
                return foundObject;
            }
        }

        throw new Exception($"Dependency of type {type} not found or cannot be resolved.");
    }


    private void Register<T>(T instance)
    {
        var type = typeof(T);
        if (!_dependencies.ContainsKey(type))
        {
            _dependencies[type] = instance;
        }
    }
}
