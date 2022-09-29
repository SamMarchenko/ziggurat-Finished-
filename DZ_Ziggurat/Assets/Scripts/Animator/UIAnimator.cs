using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
   [SerializeField] private Animator _animator;

   public void PlayOpened()
   {
      if (!_animator.enabled)
      {
         _animator.enabled = true;
      }
      this._animator.SetBool("IsOpened", true);
   }

   public void PlayClosed()
   {
      this._animator.SetBool("IsOpened", false);
   }
}
