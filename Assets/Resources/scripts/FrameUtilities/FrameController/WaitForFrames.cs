using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitForFrames
{
    public static IEnumerator wait(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
