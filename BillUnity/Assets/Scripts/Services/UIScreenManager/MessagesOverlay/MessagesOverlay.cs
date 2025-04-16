using Kborod.Services.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.Services.UIScreenManager
{

    public class MessagesOverlay : MonoBehaviour
    {
        [SerializeField] protected RectTransform _itemsRoot;
        [SerializeField] private MessageItem _prefabItem;

        [Header("Layout properties")]
        [SerializeField] private float _paddingTop;
        [SerializeField] private float _paddingSpace;

        private List<MessageItem> _items = new List<MessageItem>();

        [Inject] private DiContainer _diContainer;
        [Inject] private LocalizationService _localization;


        public void Add(string text, OverlayMessageType messageType = OverlayMessageType.Normal, IconType iconType = IconType.None)
        {
            var item = _diContainer.InstantiatePrefabForComponent<MessageItem>(_prefabItem, _itemsRoot);
            item.Init(text, messageType, iconType);
            item.RT.anchoredPosition = GetNextAvailablePosition(_items);
            item.OnDestroy += (arg) =>
            {
                _items.Remove(arg);
                UpdateItemsPositions();
            };

            _items.Add(item);

            //Обновляем позиции в следующем кадре, т.к. ContentSizeFitter отрабатывает не сразу
            StopAllCoroutines();
            StartCoroutine(UpdatePositionsInNextFrame());
        }

        private IEnumerator UpdatePositionsInNextFrame()
        {
            yield return null;
            UpdateItemsPositions();
        }

        public void AddByLocalizeKey(string key, OverlayMessageType messageType = OverlayMessageType.Normal, IconType iconType = IconType.None) =>
            Add(_localization.GetTranslationById(key), messageType, iconType);

        private Vector2 GetNextAvailablePosition(List<MessageItem> items)
        {
            float totalHeight = 0;

            totalHeight += _paddingTop;
            foreach (var item in items)
                totalHeight += item.RT.rect.height;
            totalHeight += _paddingSpace * items.Count;

            var result = new Vector2(0, -totalHeight);

            return result;
        }

        private void UpdateItemsPositions()
        {
            var tempItems = new List<MessageItem>();

            foreach (var item in _items)
            {
                item.ChangePosition(GetNextAvailablePosition(tempItems));
                tempItems.Add(item);
            }
        }

        [ContextMenu("==AddTestNotice==")]
        private void AddTestNotice()
        {
            Add("Внимание! Тестовое уведомление! Тестовое уведомление!");
        }
    }
}

/* Copyright: Made by Appfox */