using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace UI
{
    public class GameScreen : MonoBehaviour
    {
        [Header("Labels")] [SerializeField] private TextMeshProUGUI numOfCurrentRoundLabel;

        [Header("Buttons")] [Space] [SerializeField]
        private Button backButton;

        [SerializeField] private Button resetLettersButton;
        [SerializeField] private Button promptButton;

        [Header("Pictures Root")] [Space] [SerializeField]
        private Transform parentForPictures;

        [Header("Roots")] [Space] [SerializeField]
        private Transform rootOutput;

        [SerializeField] private Transform rootInput;

        private StartScreen.Factory _startScreenFactory;
        private RootImage.Factory _rootImageFactory;
        private LetterButton.Factory _letterButtonFactory;
        private PromptScreen.Factory _promptScreenFactory;

        private RoundsList _roundsList;

        private List<RootImage> _rootImages = new List<RootImage>();
        private List<char> _hiddenWord = new List<char>();
        private List<char> _inputWord = new List<char>();
        private List<LetterButton> _outputLetters = new List<LetterButton>();
        private List<LetterButton> _inputLetters = new List<LetterButton>();

        private int _currentLevel;
        [HideInInspector] public int cursor;

        private AsyncOperationHandle<Round> _handle;

        [Inject]
        private void Construct(StartScreen.Factory startScreenFactory, RootImage.Factory rootImageFactory,
            LetterButton.Factory letterButtonFactory, RoundsList roundsList, PromptScreen.Factory promptScreenFactory)
        {
            _startScreenFactory = startScreenFactory;
            _rootImageFactory = rootImageFactory;
            _letterButtonFactory = letterButtonFactory;
            _promptScreenFactory = promptScreenFactory;
            
            _roundsList = roundsList;
        }

        private void Awake()
        {
            resetLettersButton.onClick.AddListener(ResetOutputLetterButtons);
            promptButton.onClick.AddListener(() =>
            {
                Addressables.Release(_handle);
                _promptScreenFactory.Create();
                Destroy(gameObject);
            });
            
            backButton.onClick.AddListener(() =>
            {
                Addressables.Release(_handle);
                
                _startScreenFactory.Create();
                Destroy(gameObject);
            });

            SetRound();
        }

        private void GetReadyLetterButtons()
        {
            for (int i = 0; i < _hiddenWord.Count; i++)
            {
                var outputLetter = _letterButtonFactory.Create(rootOutput, LetterButton.LetterState.Empty, '\0', this);
                _outputLetters.Add(outputLetter);
            }

            for (int i = 0; i < 12; i++)
            {
                var inputLetter =
                    _letterButtonFactory.Create(rootInput, LetterButton.LetterState.NotAssigned, '\0', this);
                _inputLetters.Add(inputLetter);
            }
                
            var lettersToOutputLetterButtons = new List<char>(_hiddenWord);
            int shortOfGameAlphabet = 12 - lettersToOutputLetterButtons.Count;

            var missingLetters = 
                Alphabet.Russian
                    .ToList()
                    .GetRange(0, shortOfGameAlphabet);

            lettersToOutputLetterButtons.AddRange(missingLetters);
                
            for (int i = 0; i < 12; i++)
            {
                var letter = lettersToOutputLetterButtons[Random.Range(0, lettersToOutputLetterButtons.Count)];

                _inputLetters[i].LetterOnButton = letter;

                lettersToOutputLetterButtons.Remove(letter);
            }
        }

        private void GetReadyPictures(List<Sprite> sprites)
        {
            for (int i = 0; i < 4; i++)
            {
                var rootImage = _rootImageFactory.Create(parentForPictures, i, sprites[i], this);
                _rootImages.Add(rootImage);
            }

            _rootImages[0].ThisImageState = RootImage.ImageState.ReadyToBeOpen;

            for (int i = 1; i < _rootImages.Count; i++)
                _rootImages[i].ThisImageState = RootImage.ImageState.Blocked;
        }
        
        private void SetRound()
        {
            var roundReference = _roundsList.LoadRound(_currentLevel++);

            roundReference.LoadAssetAsync().Completed += handle =>
            {
                _handle = handle;
                
                var round = _handle.Result;

                _hiddenWord = round.Word.ToList();

                GetReadyPictures(round.Images);

                GetReadyLetterButtons();
            };
        }

        public void GetReadyRootImageByIndex(int index)
        {
            if (index < _rootImages.Count)
                _rootImages[index].ThisImageState = RootImage.ImageState.ReadyToBeOpen;
        }

        private void ResetOutputLetterButtons()
        {
            var letterButtons = _outputLetters.FindAll(x =>
                x.hisTurnedLetterButton != null && x.ThisLetterState == LetterButton.LetterState.AssignedByPlayer);

            foreach (var letterButton in letterButtons)
            {
                letterButton.hisTurnedLetterButton.TurnOn();
                letterButton.LetterOnButton = '\0';
                letterButton.ThisLetterState = LetterButton.LetterState.Empty;
            }
        }
        
        public void SetLetter(char letter, int siblingIndex, LetterButton.LetterState state, LetterButton letterButton)
        {
            cursor = 0;

            switch (letterButton.ThisLetterState)
            {
                case LetterButton.LetterState.NotAssigned:

                    if (_outputLetters.Any(x => x.LetterOnButton == '\0'))
                        while (_outputLetters[cursor].LetterOnButton != '\0')
                        {
                            if (cursor + 1 < _outputLetters.Count)
                                cursor++;
                            else
                            {
                                cursor = 0;
                            }
                        }

                    else
                        return;

                    _inputLetters[siblingIndex].TurnOff();

                    _outputLetters[cursor].LetterOnButton = letter;
                    _outputLetters[cursor].hisTurnedLetterButton = letterButton;
                    _outputLetters[cursor++].ThisLetterState = state;
                    return;

                case LetterButton.LetterState.Empty:
                    cursor = siblingIndex;
                    return;
            }

            cursor = siblingIndex;
            _outputLetters[siblingIndex].hisTurnedLetterButton.TurnOn();
            _outputLetters[siblingIndex].LetterOnButton = '\0';
            _outputLetters[siblingIndex].ThisLetterState = LetterButton.LetterState.Empty;
        }
    }
    
    public static class Alphabet
    {
        public static readonly string Russian = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
    }
}