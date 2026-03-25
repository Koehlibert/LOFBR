using UnityEngine;

public static class AIUtils
{
    public enum MovementState
    {
        IsStanding,
        IsMovingForward,
        IsFollowingTarget,
        IsGoingToPlace,
        IsRetreating,
        IsCircling
    }
    public enum AIState
    {
        MoveOnly,
        CheckGeneralSkills,
        CheckDistSkills,
        CheckShoot,
        Attacking,
        Retreating
    }
    public enum HealthState
    {
        FullHP,
        Healthy, 
        PartiallyDamaged,
        Hurt
    }
}