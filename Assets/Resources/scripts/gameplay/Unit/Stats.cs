using System;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour, IObservable<Stats>
{
    public int life;

    public int attack;

    public int deffense;

    public int velocity;

    private List<IObserver<Stats>> observers = new();

    public bool LifePointsVariation(int lifeVariation)
    {
        life -= lifeVariation;
        NotifyObservers();
        return life > 0;
    }

    public IDisposable Subscribe(IObserver<Stats> observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
            observer.OnNext(this);
        }

        return new Unsubscriber(observers, observer);
    }

    private void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.OnNext(this);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<Stats>> _observers;
        private readonly IObserver<Stats> _observer;

        public Unsubscriber(List<IObserver<Stats>> observers, IObserver<Stats> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}
