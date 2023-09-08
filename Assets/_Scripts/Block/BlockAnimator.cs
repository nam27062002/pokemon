using System;
using UnityEngine;

namespace _Scripts.Block
{
    public class BlockAnimator : MonoBehaviour
    {
        private Animator animator;
        private const string IS_SUGGEST = "IsSuggest";
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetAnimator(bool active)
        {
            animator.SetBool(IS_SUGGEST,active);
        }
        
    }
}
