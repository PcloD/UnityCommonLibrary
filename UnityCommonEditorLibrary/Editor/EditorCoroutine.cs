using System.Collections;
using UnityEditor;

public class EditorCoroutine
{
    readonly IEnumerator routine;

    public static EditorCoroutine Start(IEnumerator routine)
    {
        var coroutine = new EditorCoroutine(routine);
        coroutine.Start();
        return coroutine;
    }

    private EditorCoroutine(IEnumerator routine)
    {
        this.routine = routine;
    }

    private void Start()
    {
        EditorApplication.update += Update;
    }

    public void Stop()
    {
        EditorApplication.update -= Update;
    }

    void Update()
    {
        if (!routine.MoveNext())
        {
            Stop();
        }
    }

}