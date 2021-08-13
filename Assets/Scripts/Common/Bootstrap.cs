using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

namespace Common
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private StartScreen.Factory _startScreenFactory;

        private void Awake()
        {
            _startScreenFactory.Create();
        }
    }
}
