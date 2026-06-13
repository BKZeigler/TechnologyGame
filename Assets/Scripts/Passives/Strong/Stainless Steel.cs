using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Stainless Steel")]
public class StainlessSteelPassive : PassiveData
{
    private void OnEnable()
    {
        passiveName = "Stainless Steel";
    }

    public override bool AllowBuff(RobotInstance robot, Buff buff)
    {
        if (buff.buffType == BuffType.Debuff)
        {
            Debug.Log($"{robot.data.name} is immune to debuffs! {buff.buffName} blocked.");
            return false;
        }

        return true;
    }
}
