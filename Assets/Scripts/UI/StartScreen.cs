using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button bonusGameButton;

        private MainGameScreen.Factory _mainGameScreenFactory;
        private BonusGameScreen.Factory _bonusGameScreenFactory;
        
        [Inject]
        private void Construct(MainGameScreen.Factory mainGameScreenFactory, BonusGameScreen.Factory bonusGameScreenFactory)
        {
            _mainGameScreenFactory = mainGameScreenFactory;
            _bonusGameScreenFactory = bonusGameScreenFactory;
        }
        
        private void Start()
        {
            startGameButton.onClick.AddListener(() =>
            {
                _mainGameScreenFactory.Create();
                Destroy(gameObject);
            });
            bonusGameButton.onClick.AddListener(() =>
            {
                _bonusGameScreenFactory.Create();
                Destroy(gameObject);
            });
        }
        
        public class Factory : UIFactory<StartScreen> {}
    }
}