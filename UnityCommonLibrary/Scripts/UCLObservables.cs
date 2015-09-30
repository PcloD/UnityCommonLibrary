using UnityEngine;

namespace UnityCommonLibrary.Observables {
    public class PrefabRetrievedFromPool : Observable<PrefabRetrievedFromPool, GameObject> { }
    public class PrefabReturnedToPool : Observable<PrefabReturnedToPool, GameObject> { }
}