using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Runtime.InteropServices;

public class ActiveCharacterManager : MonoBehaviour
{
    public static ActiveCharacterManager Instance;
    public CharacterBehaviour ActiveCharacter;
    private CharacterBehaviour DefaultActiveCharacter;
    private void Awake()
    {
        Instance = this;
    }
    public void Init(CharacterBehaviour defaultCharacter)
    {
        this.DefaultActiveCharacter = defaultCharacter;
        ActiveCharacter = defaultCharacter;
    }
    public void ChangeActiveCharacter(CharacterBehaviour newActiveCharacter)
    {
        ActiveCharacter.ToggleInteractive();
        if (ActiveCharacter == DefaultActiveCharacter)
        {
            ActiveCharacter.animator.SetFloat("moveX", 0);
            ActiveCharacter.animator.SetFloat("moveZ", 0);
            ActiveCharacter.aIHandler.LockAI(Mathf.Infinity);
            ActiveCharacter.aIHandler.movementAI.LockMovementAI();
        }
        ActiveCharacter = newActiveCharacter;
        ActiveCharacter.ToggleInteractive();
        CameraController.Instance.SetNewTarget(ActiveCharacter.gameObject);
    }
    public void ResetActiveCharacter()
    {
        ActiveCharacter?.ToggleInteractive();
        ActiveCharacter = DefaultActiveCharacter;
        ActiveCharacter.ToggleInteractive();
        ActiveCharacter.aIHandler.UnlockAI();
        ActiveCharacter.aIHandler.movementAI.UnlockMovementAI();
        CameraController.Instance.SetTargetToDefault();
    }
}