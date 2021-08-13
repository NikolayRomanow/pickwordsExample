using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIFactory<T> : PlaceholderFactory<T> where T : Component
    {
        [Inject]
        private Canvas _root;

        public override T Create()
        {
            var instance = base.Create();
            instance.transform.SetParent(_root.transform, false);
            return instance;
        }
    }
}
