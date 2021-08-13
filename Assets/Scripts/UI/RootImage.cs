using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RootImage : MonoBehaviour
    {
        public enum ImageState
        {
            Blocked,
            ReadyToBeOpen,
            Opened
        }

        [SerializeField] private ImageState thisImageState;

        public ImageState ThisImageState
        {
            set
            {
                switch (value)
                {
                    case ImageState.Blocked:
                        _pictureButton.interactable = false;
                        _image.color = blocked;
                        lockIcon.gameObject.SetActive(true);
                        break;
                    case ImageState.ReadyToBeOpen:
                        _pictureButton.interactable = true;
                        _image.color = readyToBeOpened;
                        lockIcon.gameObject.SetActive(false);
                        buyNewPictureLabel.gameObject.SetActive(true);
                        break;
                }

                thisImageState = value;
            }
        }
        
        [Header("Colors")]
        [Space]
        [SerializeField] private Color blocked = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField] private Color readyToBeOpened = new Color(1f, 1f, 1f);

        [Header("Icons and Labels")] 
        [Space] 
        [SerializeField] private Image lockIcon;
        [SerializeField] private GameObject buyNewPictureLabel;
        
        private Image _image;
        private Sprite _picture;
        private Button _pictureButton;
        private GameScreen _gameScreen;
        private int _countOfThisPicture;
        
        private Picture.Factory _pictureFactory;

        [Inject]
        private void Construct(Picture.Factory pictureFactory, Transform root, int i, Sprite picture,
            GameScreen gameScreen)
        {
            transform.SetParent(root);

            switch (i)
            {
                case 0:
                    transform.localScale = ScalesForDifferentPositions.LeftUp;
                    buyNewPictureLabel.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 1:
                    transform.localScale = ScalesForDifferentPositions.RightUp;
                    break;
                case 2:
                    transform.localScale = ScalesForDifferentPositions.LeftDown;
                    lockIcon.transform.localScale = new Vector3(1, -1, 1);
                    buyNewPictureLabel.transform.localScale = new Vector3(-1, -1, 1);
                    break;
                case 3:
                    transform.localScale = ScalesForDifferentPositions.RightDown;
                    lockIcon.transform.localScale = new Vector3(1, -1, 1);
                    buyNewPictureLabel.transform.localScale = new Vector3(1, -1, 1);
                    break;
                default: break;
            }

            _countOfThisPicture = i;
            _picture = picture;
            _pictureButton = GetComponent<Button>();
            _image = GetComponent<Image>();
            _pictureButton.onClick.AddListener(ClickOnPicture);
            _pictureFactory = pictureFactory;
            _gameScreen = gameScreen;
        }

        private void ClickOnPicture()
        {
            switch (thisImageState)
            {
                case ImageState.Blocked:
                    return;
                case ImageState.ReadyToBeOpen:
                    _pictureFactory.Create(transform, _picture);
                    thisImageState = ImageState.Opened;
                    _gameScreen.GetReadyRootImageByIndex(transform.GetSiblingIndex() + 1);
                    break;
                case ImageState.Opened:
                    return;
            }
        }

        public class Factory : PlaceholderFactory<Transform, int, Sprite, GameScreen, RootImage> {}
    }

    public static class ScalesForDifferentPositions
    {
        public static Vector3 LeftUp = new Vector3(-1, 1, 1);
        public static Vector3 RightUp = new Vector3(1, 1, 1);
        public static Vector3 LeftDown = new Vector3(-1, -1, 1);
        public static Vector3 RightDown = new Vector3(1, -1, 1);
    }
}
