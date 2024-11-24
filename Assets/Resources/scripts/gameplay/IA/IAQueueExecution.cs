using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAQueueExecution : MonoBehaviour
{
    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

    private bool isProcessing = false;

    private GameMaster gameMaster;

    void Start()
    {
        gameMaster = EngineDependencyInjector.getInstance().Resolve<GameMaster>();
    }

    public void Enqueue(IEnumerator coroutine)
    {
        coroutineQueue.Enqueue(coroutine);
        if(isProcessing)
        {
            return;
        }

        StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isProcessing = true;
        yield return new WaitForSecondsRealtime(0.2f);

        while (coroutineQueue.Count > 0)
        {
            IEnumerator current = coroutineQueue.Dequeue();
            yield return StartCoroutine(current);
        }

        yield return new WaitUntil(() => !gameMaster.isSystemBusy);
        yield return new WaitForSecondsRealtime(0.2f);
    }
}