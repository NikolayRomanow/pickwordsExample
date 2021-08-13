using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public class RoundsList : MonoBehaviour
    {
        [SerializeField] private List<AssetReferenceRound> rounds;

        public int RoundsListSize => rounds.Count;

        public AssetReferenceRound LoadRound(int count)
        {
            return rounds[count];
        }
    }
}