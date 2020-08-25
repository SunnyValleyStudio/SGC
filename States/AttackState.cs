using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        controllerReference.movement.StopMovement();
        controllerReference.agentAnimations.OnFinishedAttacking += TransitionBack;
        controllerReference.agentAnimations.TriggerAttackAnimation();
        controllerReference.detectionSystem.OnAttackSuccessful += PerformHit;

    }

    private void TransitionBack()
    {
        controllerReference.agentAnimations.OnFinishedAttacking -= TransitionBack;
        controllerReference.detectionSystem.OnAttackSuccessful -= PerformHit;
        controllerReference.TransitionToState(controllerReference.movementState);
    }

    public void PerformHit(Collider hitObject, Vector3 hitPosition)
    {
        var hittable = hitObject.GetComponent<IHIttable>();
        if (hittable != null)
        {
            var equippedItem = ItemDataManager.instance.GetItemData(controllerReference.inventorySystem.EquippedWeaponId);
            hittable.GetHit((WeaponItemSO)equippedItem, hitPosition);
        }
    }
}
