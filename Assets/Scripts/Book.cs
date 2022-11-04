using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Item
{
  public override void SetColor(string generatedColor)
  {
    Debug.Log("Book calling setColor method");
    base.SetColor(generatedColor);
  }
}
