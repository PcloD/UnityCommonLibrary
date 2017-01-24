using UnityCommonLibrary.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityCommonEditorLibrary.Inspectors
{
	public static class SelectorInspector
	{
		[MenuItem("GameObject/UI/Selector", false)]
		public static void CreateSelector(MenuCommand command)
		{
			//Root obj
			var selector = CreateUIElementRoot("Selector", new Vector2(160f, 30f)).AddComponent<Selector>();
			selector.targetGraphic = selector.gameObject.AddComponent<Image>();

			//Label child obj
			var label = CreateUIObject("Label", selector.gameObject).AddComponent<Text>();
			selector.label = label;
			label.color = Color.black;
			label.alignment = TextAnchor.MiddleCenter;
			label.rectTransform.anchorMin = Vector2.zero;
			label.rectTransform.anchorMax = Vector2.one;
			label.rectTransform.sizeDelta = Vector2.zero;

			PlaceUIElementRoot(selector.gameObject, command);
		}

		private static GameObject CreateUIElementRoot(string name, Vector2 size)
		{
			var child = new GameObject(name);
			var rectTransform = child.AddComponent<RectTransform>();
			rectTransform.sizeDelta = size;
			return child;
		}

		private static GameObject CreateUIObject(string name, GameObject parent)
		{
			var go = new GameObject(name);
			go.AddComponent<RectTransform>();
			SetParentAndAlign(go, parent);
			return go;
		}

		private static void SetParentAndAlign(GameObject child, GameObject parent)
		{
			if(parent == null)
			{
				return;
			}
			child.transform.SetParent(parent.transform, false);
			SetLayerRecursively(child, parent.layer);
		}

		private static void SetLayerRecursively(GameObject go, int layer)
		{
			go.layer = layer;
			var t = go.transform;
			for(int i = 0; i < t.childCount; i++)
			{
				SetLayerRecursively(t.GetChild(i).gameObject, layer);
			}
		}

		private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
		{
			var parent = menuCommand.context as GameObject;
			if(parent == null || parent.GetComponentInParent<Canvas>() == null)
			{
				parent = GetOrCreateCanvasGameObject();
			}

			var uniqueName = UnityEditor.GameObjectUtility.GetUniqueNameForSibling(parent.transform, element.name);
			element.name = uniqueName;
			Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);
			Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
			UnityEditor.GameObjectUtility.SetParentAndAlign(element, parent);
			if(parent != menuCommand.context)
			{
				SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());
			}
			Selection.activeGameObject = element;
		}

		private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
		{
			// Find the best scene view
			var sceneView = SceneView.lastActiveSceneView;
			if(sceneView == null && SceneView.sceneViews.Count > 0)
			{
				sceneView = SceneView.sceneViews[0] as SceneView;
			}
			// Couldn't find a SceneView. Don't set position.
			if(sceneView == null || sceneView.camera == null)
			{
				return;
			}

			// Create world space Plane from canvas position.
			Vector2 localPlanePosition;
			var camera = sceneView.camera;
			var position = Vector3.zero;
			if(RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
			{
				// Adjust for canvas pivot
				localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
				localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

				localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
				localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

				// Adjust for anchoring
				position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
				position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

				Vector3 minLocalPosition;
				minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
				minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

				Vector3 maxLocalPosition;
				maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
				maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

				position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
				position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
			}

			itemTransform.anchoredPosition = position;
			itemTransform.localRotation = Quaternion.identity;
			itemTransform.localScale = Vector3.one;
		}

		public static GameObject GetOrCreateCanvasGameObject()
		{
			var selectedGo = Selection.activeGameObject;

			// Try to find a gameobject that is the selected GO or one if its parents.
			var canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
			if(canvas != null && canvas.gameObject.activeInHierarchy)
			{
				return canvas.gameObject;
			}

			// No canvas in selection or its parents? Then use just any canvas..
			canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
			if(canvas != null && canvas.gameObject.activeInHierarchy)
			{
				return canvas.gameObject;
			}

			// No canvas in the scene at all? Then create a new one.
			return CreateNewUI();
		}

		public static GameObject CreateNewUI()
		{
			// Root for the UI
			var root = new GameObject("Canvas");
			root.layer = LayerMask.NameToLayer("UI");
			var canvas = root.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			root.AddComponent<CanvasScaler>();
			root.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

			// if there is no event system add one...
			CreateEventSystem(false, null);
			return root;
		}

		private static void CreateEventSystem(bool select, GameObject parent)
		{
			var esys = Object.FindObjectOfType<EventSystem>();
			if(esys == null)
			{
				var eventSystem = new GameObject("EventSystem");
				UnityEditor.GameObjectUtility.SetParentAndAlign(eventSystem, parent);
				esys = eventSystem.AddComponent<EventSystem>();
				eventSystem.AddComponent<StandaloneInputModule>();

				Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
			}

			if(select && esys != null)
			{
				Selection.activeGameObject = esys.gameObject;
			}
		}
	}
}