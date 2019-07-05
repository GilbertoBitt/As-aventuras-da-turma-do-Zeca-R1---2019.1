using System;
using System.ComponentModel;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialSystem.Scripts
{
   public class DialogWindowComponent : MonoBehaviour
   {
      public TextMeshProUGUI dialogTextComponent;
      public TextMeshProUGUI characterNameTextComponent;
      public Button nextButton;
      public Button previousButton;
      public Button replayButton;

      [Sirenix.OdinInspector.ReadOnly]
      public DialogComponent dialogComponent;

      public void OnValidate()
      {
         if (dialogComponent == null)
         {
            dialogComponent = GetComponentInParent(typeof(DialogComponent))as DialogComponent;
         }

         if (dialogComponent != null && !dialogComponent.subscribedDialogWindowComponents.Contains(this))
         {
            dialogComponent.subscribedDialogWindowComponents.Add(this);
         }
      }
   }
}
