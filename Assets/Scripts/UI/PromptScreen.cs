using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class PromptScreen : MonoBehaviour
    {
        [Header("Buttons")] 
        [SerializeField] private Button backToGameButton;

        [Header("Prompts")] 
        [Space]
        [SerializeField] private Button removeUnnecessaryButtons;
        [SerializeField] private Button openRandomLetter;
        [SerializeField] private Button seeVideo;
        [SerializeField] private Button friendHelp;

        private MainGameScreen.Factory _mainGameScreenFactory;
        
        [Inject]
        private void Construct(MainGameScreen.Factory mainGameScreenFactory)
        {
            _mainGameScreenFactory = mainGameScreenFactory;
            
            backToGameButton.onClick.AddListener(() =>
            {
                _mainGameScreenFactory.Create();
                Destroy(gameObject);
            });
        }
        
        public class Factory : UIFactory<PromptScreen> {}
    }
}
