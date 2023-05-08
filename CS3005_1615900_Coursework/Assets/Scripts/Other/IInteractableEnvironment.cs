using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableEnvironment
{
    public void Interaction(Object obj);
    public void ShowInteractionUI();
    public void HideInteractionUI();
}
