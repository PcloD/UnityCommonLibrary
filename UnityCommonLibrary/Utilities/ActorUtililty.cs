using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
	public static class ActorUtililty
	{
		/// <summary>
		/// Caching because Actors should never be removed
		/// </summary>
		private static Dictionary<GameObject, Actor> actorCache = new Dictionary<GameObject, Actor>();

		public static Actor GetActor(this Component component)
		{
			return GetActor<Actor>(component);
		}
		public static A GetActor<A>(this Component component) where A : Actor
		{
			if(!component)
			{
				return null;
			}
			return component.gameObject.GetActor<A>();
		}
		public static Actor GetActor(this GameObject gameObject)
		{
			return GetActor<Actor>(gameObject);
		}
		public static A GetActor<A>(this GameObject gameObject) where A : Actor
		{
			if(!gameObject)
			{
				return null;
			}
			Actor actor;
			if(!actorCache.TryGetValue(gameObject, out actor))
			{
				actor = gameObject.GetComponentInParent<A>();
				actorCache.Add(gameObject, actor);
			}
			return actor as A;
		}
		public static void RemoveActorFromCache(Actor actor)
		{
			actorCache.Remove(actor.gameObject);
		}
	}
}