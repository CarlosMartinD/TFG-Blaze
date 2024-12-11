using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAQueueExecution : MonoBehaviour
{
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    private TurnEngine turnEngine;

    void Start()
    {
        turnEngine = EngineDependencyInjector.getInstance().Resolve<TurnEngine>();
    }

    public void Enqueue(IEnumerator coroutine)
    {
        coroutineQueue.Enqueue(coroutine);
    }

    public void Enqueue(IEnumerator[] coroutines)
    {
        foreach(IEnumerator coroutine in coroutines)
        {
            Enqueue(coroutine);
        }
    }

    public IEnumerator ProcessQueue()
    {
        while (coroutineQueue.Count > 0)
        {
            IEnumerator current = coroutineQueue.Dequeue();
            yield return StartCoroutine(current);
            yield return WaitForFrames.wait(30);
        }

        turnEngine.EndTurnAI();
    }
}
