using UnityEngine;

public static class AIUtils
{
    public enum MovementState
    {
        IsStanding,
        IsMovingForward,
        IsFollowingTarget,
        IsGoingToPlace,
        IsRetreating
    }
    public enum AIState
    {
        MoveOnly,
        CheckGeneralSkills,
        CheckDistSkills,
        CheckShoot,
        Attacking
    }
    public enum HealthState
    {
        FullHP,
        Healthy, 
        PartiallyDamaged,
        Hurt
    }
}