using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "Round Scriptable Object", fileName = "Round")]
    public class Round : ScriptableObject
    {
        [Header("Pictures")]
        [SerializeField] private List<Sprite> images = new List<Sprite>();
        [SerializeField] private List<string> labelsForImages = new List<string>();
        
        [Header("Hidden Word")]
        [Space(20f)]
        [SerializeField] private string word;

        public List<Sprite> Images => images;
        public List<string> LabelsForImages => labelsForImages;

        public string Word => word;
    }

    [Serializable]
    public class AssetReferenceRound : AssetReferenceT<Round>
    {
        public AssetReferenceRound(string guid) : base(guid) {}
    }
}
