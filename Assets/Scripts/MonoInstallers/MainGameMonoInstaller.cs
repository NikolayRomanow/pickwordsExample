using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace MonoInstallers
{
    public class MainGameMonoInstaller : MonoInstaller
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Image uiBackground;
        [SerializeField] private StartScreen uiStartScreen;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private MainGameScreen uiMainGameScreen;
        [SerializeField] private BonusGameScreen uiBonusGameScreen;
        [SerializeField] private PromptScreen promptScreen;
        [SerializeField] private RootImage rootImage;
        [SerializeField] private LetterButton letterButton;
        [SerializeField] private RoundsList roundsList;
        [SerializeField] private Picture picture;
        
        public override void InstallBindings()
        {
            BindPromptScreen();
            BindPicture();
            BindRoundsList();
            BindLetterButton();
            BindRootImage();
            BindBonusGameScreen();
            BindMainGameScreen();
            BindStartScreen();
            BindUICanvas();
            BindEventSystem();
        }

        private void BindPromptScreen()
        {
            Container
                .BindFactory<PromptScreen, PromptScreen.Factory>()
                .FromComponentInNewPrefab(promptScreen)
                .AsSingle();
        }
        
        private void BindPicture()
        {
            Container
                .BindFactory<Transform, Sprite, Picture, Picture.Factory>()
                .FromComponentInNewPrefab(picture)
                .AsSingle();
        }
        
        private void BindRoundsList()
        {
            Container
                .Bind<RoundsList>()
                .FromComponentInNewPrefab(roundsList)
                .AsSingle();
        }
        
        private void BindLetterButton()
        {
            Container
                .BindFactory<Transform, LetterButton.LetterState, char, GameScreen, LetterButton, LetterButton.Factory>()
                .FromComponentInNewPrefab(letterButton)
                .AsSingle();
        }

        private void BindUICanvas()
        {
            var uiCanvasOnScene =
                Container.InstantiatePrefabForComponent<Canvas>(uiCanvas);

            Container
                .Bind<Canvas>()
                .FromInstance(uiCanvasOnScene)
                .AsSingle();

            Container.InstantiatePrefabForComponent<Image>(uiBackground, uiCanvasOnScene.transform);
        }

        private void BindEventSystem()
        {
            var eventSystemOnScene = Container.InstantiatePrefabForComponent<EventSystem>(eventSystem);

            Container
                .Bind<EventSystem>()
                .FromInstance(eventSystemOnScene)
                .AsSingle();
        }

        private void BindStartScreen()
        {
            Container
                .BindFactory<StartScreen, StartScreen.Factory>()
                .FromComponentInNewPrefab(uiStartScreen)
                .AsSingle();
        }

        private void BindMainGameScreen()
        {
            Container
                .BindFactory<MainGameScreen, MainGameScreen.Factory>()
                .FromComponentInNewPrefab(uiMainGameScreen)
                .AsSingle();
        }
        
        private void BindBonusGameScreen()
        {
            Container
                .BindFactory<BonusGameScreen, BonusGameScreen.Factory>()
                .FromComponentInNewPrefab(uiBonusGameScreen)
                .AsSingle();
        }

        private void BindRootImage()
        {
            Container
                .BindFactory<Transform, int, Sprite, GameScreen, RootImage, RootImage.Factory>()
                .FromComponentInNewPrefab(rootImage)
                .AsSingle();
        }
    }
}