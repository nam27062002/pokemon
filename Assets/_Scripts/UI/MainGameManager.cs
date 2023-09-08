using System;
using _Scripts.Grid;
using _Scripts.Singleton;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class MainGameManager : Singleton<MainGameManager>
    {
        [SerializeField] private Button shuffleButton;
        [SerializeField] private Button suggestButton;
        [SerializeField] private GameObject arrow;
        private void Start()
        {
            shuffleButton.onClick.AddListener(() =>
            {
                GridManager.Instance.ShuffleGridHandle();
            });
            suggestButton.onClick.AddListener(() =>
            {
                GameManager.Instance.SuggestDirection();
            });
            
        }

        public void SetArrowActive()
        {
            arrow.SetActive(true);
        }
    }
}
