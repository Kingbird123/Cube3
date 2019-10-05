using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBuffEnableAbility", menuName = "Data/Buffs/UnitBuffs/PlayerBuffEnableAbility", order = 1)]
public class PlayerBuffEnableAbility : UnitBuff
{
    public enum AbilityType { Run, Dash, Crouch, Jump, DoubleJump, WallJump, Climbing };
    public AbilityType ability;
    public bool enable;

    public override void ActivateBuff(Unit _unit, bool _activate)
    {
        var cont = _unit.GetComponent<PlayerController>();
        if (cont)
        {
            switch (ability)
            {
                case AbilityType.Run:
                    cont.RunEnabled = _activate == enable;
                    break;
                case AbilityType.Dash:
                    cont.DashEnabled = _activate == enable;
                    break;
                case AbilityType.Crouch:
                    cont.CrouchEnabled = _activate == enable;
                    break;
                case AbilityType.Jump:
                    cont.JumpEnabled = _activate == enable;
                    break;
                case AbilityType.DoubleJump:
                    cont.DoubleJumpEnabled = _activate == enable;
                    break;
                case AbilityType.WallJump:
                    //cont.WallJumpEnabled = _activate == enable;
                    break;
                case AbilityType.Climbing:
                    cont.ClimbingEnabled = _activate == enable;
                    break;
            }
        }
    }

}
