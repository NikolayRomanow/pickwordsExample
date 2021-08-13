using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class LetterButton : MonoBehaviour
    {
        public enum LetterState
        {
            AssignedByPlayer,
            AssignedByPrompt,
            WrongAssigned,
            NotAssigned,
            Empty
        }

        private LetterState _thisLetterState;

        public LetterState ThisLetterState
        {
            set
            {
                switch (value)
                {
                    case LetterState.NotAssigned:
                        thisImage.sprite = SpritesForLettersState.NotAssigned;
                        break;
                    case LetterState.WrongAssigned:
                        thisImage.sprite = SpritesForLettersState.WrongAssigned;
                        break;
                    case LetterState.AssignedByPlayer:
                        thisImage.sprite = SpritesForLettersState.AssignedByPlayer;
                        break;
                    case LetterState.AssignedByPrompt:
                        thisImage.sprite = SpritesForLettersState.AssignedByPrompt;
                        break;
                    case LetterState.Empty:
                        thisImage.sprite = SpritesForLettersState.Empty;
                        break;
                }

                _thisLetterState = value;
            }

            get => _thisLetterState;
        }

        public Image thisImage;

        public TextMeshProUGUI thisLetterLabel;

        private char _letterOnButton;
        public char LetterOnButton
        {
            set
            {
                _letterOnButton = value;
                thisLetterLabel.text = value.ToString();
            }

            get => _letterOnButton;
        }

        private GameScreen _gameScreen;
        private Button _button;
        public LetterButton hisTurnedLetterButton;
        
        [Inject]
        private void Construct(Transform parent, LetterState letterState, char letter, GameScreen gameScreen)
        {
            thisLetterLabel = GetComponentInChildren<TextMeshProUGUI>();
            _thisLetterState = LetterState.NotAssigned;

            thisImage = GetComponent<Image>();
            
            transform.SetParent(parent);
            _thisLetterState = letterState;

            switch (letterState)
            {
                case LetterState.NotAssigned:
                    thisImage.sprite = SpritesForLettersState.NotAssigned;
                    break;
                case LetterState.WrongAssigned:
                    thisImage.sprite = SpritesForLettersState.WrongAssigned;
                    break;
                case LetterState.AssignedByPlayer:
                    thisImage.sprite = SpritesForLettersState.AssignedByPlayer;
                    break;
                case LetterState.AssignedByPrompt:
                    thisImage.sprite = SpritesForLettersState.AssignedByPrompt;
                    break;
                case LetterState.Empty:
                    thisImage.sprite = SpritesForLettersState.Empty;
                    break;
            }

            _letterOnButton = letter;
            thisLetterLabel.text = letter.ToString();

            _gameScreen = gameScreen;
            _button = GetComponent<Button>();
            
            _button.onClick.AddListener(() =>
            {
                switch (letterState)
                {
                    case LetterState.NotAssigned: 
                        _gameScreen.SetLetter(_letterOnButton, transform.GetSiblingIndex(), LetterState.AssignedByPlayer, this);
                        return;
                }
                
                _gameScreen.SetLetter(_letterOnButton, transform.GetSiblingIndex(), LetterState.NotAssigned, this);
            });
            
            transform.localScale = Vector3.one;
        }

        public void TurnOn()
        {
            thisLetterLabel.color = new Color(1, 1, 1, 1f);
            _button.interactable = true;
        }
        
        public void TurnOff()
        {
            thisLetterLabel.color = new Color(1, 1, 1, 0.5f);
            _button.interactable = false;
        }

        public class Factory : PlaceholderFactory<Transform, LetterState, char, GameScreen, LetterButton> {}
    }

    public static class SpritesForLettersState
    {
        public static readonly Sprite AssignedByPlayer = Resources.Load<Sprite>("Sprites/OrangeLetterButton");
        public static readonly Sprite AssignedByPrompt = Resources.Load<Sprite>("Sprites/GreenLetterButton");
        public static readonly Sprite WrongAssigned = Resources.Load<Sprite>("Sprites/RedLetterButton");
        public static readonly Sprite NotAssigned = Resources.Load<Sprite>("Sprites/PurpleLetterButton");
        public static readonly Sprite Empty = Resources.Load<Sprite>("Sprites/WhiteLetterButton");
    }
}

