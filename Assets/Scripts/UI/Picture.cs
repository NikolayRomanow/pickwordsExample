using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class Picture : MonoBehaviour
    {
        private Image _image;

        [Inject]
        private void Construct(Transform parent, Sprite sprite)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;

            if (parent.localScale.y < 0)
                transform.localScale = new Vector3(1, -1, 1);
            else
                transform.localScale = Vector3.one;

            _image = GetComponent<Image>();
            _image.sprite = sprite;
        }

        public class Factory : PlaceholderFactory<Transform, Sprite, Picture> {}
    }
}
