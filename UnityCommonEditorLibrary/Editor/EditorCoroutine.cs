using System.Collections;
using UnityEditor;

public class EditorCoroutine
{
    private readonly IEnumerator _routine;

    private EditorCoroutine(IEnumerator routine)
    {
        _routine = routine;
    }

    public static EditorCoroutine Start(IEnumerator routine)
    {
        var coroutine = new EditorCoroutine(routine);
        coroutine.Start();
        return coroutine;
    }

    public void Stop()
    {
        EditorApplication.update -= Update;
    }

    private void Start()
    {
        EditorApplication.update -= Update;
        EditorApplication.update += Update;
    }

    private void Update()
    {
        if (!_routine.MoveNext())
        {
            Stop();
        }
    }
}