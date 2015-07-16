using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    [RequireComponent(typeof(VerticalLayoutGroup))]
    public class UIList : UCScript {

        public delegate void OnSelectionChanged(ListItem oldItem, ListItem newItem);

        public delegate void OnRefreshComplete();

        public delegate void OnClearComplete();

        [SerializeField, Header("Text Settings")]
        int fontSize;

        [SerializeField]
        FontStyle fontStyle;

        [SerializeField]
        Font font;

        [SerializeField]
        bool supportRichText = false;

        [SerializeField]
        float top, left, right, bottom;

        [SerializeField, Header("Background Settings")]
        Color backgroundColor;

        [SerializeField]
        Color alternateColor;

        [SerializeField]
        bool alternateColors;

        [Header("Items")]
        public List<string> elements = new List<string>();

        public ListItem selected { get; private set; }

        List<ListItem> cells = new List<ListItem>();
        bool refreshing;
        bool clearing;

        public event OnSelectionChanged SelectionChanged;

        public event OnClearComplete ClearComplete;

        public event OnRefreshComplete RefreshComplete;

        public void ClearList() {
            clearing = true;
            elements.Clear();
            StartCoroutine(_RefreshList());
        }

        void Update() {
            if(elements.Count != cells.Count) {
                StartCoroutine(_RefreshList());
            }
            if(!refreshing) {
                for(int i = 0;i < elements.Count;i++) {
                    if(elements.Count != cells.Count) {
                        //safety check for refreshing
                        break;
                    }
                    var li = cells[i];
                    li.text.text = elements[i];
                    li.text.fontSize = fontSize;
                    li.text.fontStyle = fontStyle;
                    li.text.supportRichText = supportRichText;
                    li.background.color = (alternateColors && i % 2 != 0) ? alternateColor : backgroundColor;
                    li.rt.anchorMin = Vector2.zero;
                    li.rt.anchorMax = Vector2.one;
                    li.rt.offsetMax = new Vector2(right, top);
                    li.rt.offsetMin = new Vector2(left, bottom);
                }
            }
        }

        IEnumerator _RefreshList() {
            if(!refreshing) {
                refreshing = true;

                while(elements.Count > cells.Count) {
                    cells.Add(CreateListItem());
                    yield return null;
                }
                while(elements.Count < cells.Count) {
                    DestroyLastListItem();
                    yield return null;
                }
            }
            if(RefreshComplete != null) {
                RefreshComplete();
            }
            if(clearing && ClearComplete != null) {
                ClearComplete();
            }
            clearing = false;
            refreshing = false;
        }

        void DestroyLastListItem() {
            var last = cells[cells.Count - 1];
            cells.RemoveAt(cells.Count - 1);
            if(Application.isPlaying) {
                Destroy(last.gameObj);
            }
            else {
                DestroyImmediate(last.gameObj);
            }
        }

        ListItem CreateListItem() {
            var newChild = new GameObject("ListItem");
            var newText = new GameObject("ListItemText");

            //Set parents and scale
            newText.transform.parent = newChild.transform;
            newChild.transform.parent = transform;
            ResetTransform(newText.transform);
            ResetTransform(newChild.transform);

            var textRect = newText.AddComponent<RectTransform>();

            //Create necessary components
            var background = newChild.AddComponent<Image>();
            var text = newText.AddComponent<Text>();
            var button = newText.AddComponent<Button>();
            var inferrer = newChild.AddComponent<InferSize>();
            var trigger = newChild.AddComponent<EventTrigger>();

            text.font = font;
            button.targetGraphic = text;
            inferrer.targetGraphic = text;
            var listItem = new ListItem(newChild, text, textRect, background, button);
            button.onClick.AddListener(() => {
                FireSelectionChanged(listItem);
            });
            return listItem;
        }

        void FireSelectionChanged(ListItem newItem) {
            var oldItem = selected;
            selected = newItem;
            if(SelectionChanged != null) {
                SelectionChanged(oldItem, newItem);
            }
        }

        void ResetTransform(Transform t) {
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
    }

    [Serializable]
    public class ListItem {
        public GameObject gameObj;
        public Text text;
        public RectTransform rt;
        public Image background;
        public Button button;

        public ListItem(GameObject gameObj, Text text, RectTransform rt, Image background, Button button) {
            this.gameObj = gameObj;
            this.text = text;
            this.rt = rt;
            this.background = background;
            this.button = button;
        }
    }
}