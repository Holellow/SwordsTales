using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
   private bool _isGrounded;

   public bool IsGrounded
   {
      get => _isGrounded;
      set => _isGrounded = value;
   }
   private Collider2D[] _checkCollider2D;
   public bool CheckGrounded()
   {

      _checkCollider2D = Physics2D.OverlapCircleAll(transform.position, 0.3f);
      if (_checkCollider2D.Length > 2)
      {
         _isGrounded = true;
      }
      else
      {
         _isGrounded = false;
      }

      return _isGrounded;
   }
}
