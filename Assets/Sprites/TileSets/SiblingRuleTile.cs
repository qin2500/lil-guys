using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SiblingRuleTile : RuleTile
{

    public enum SibingGroup
    {
        Outer,
        Inner,
    }
    public SibingGroup sibingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return (other is SiblingRuleTile
                        && ((other as SiblingRuleTile).sibingGroup == this.sibingGroup)) || (this.sibingGroup == SibingGroup.Outer && other is SiblingRuleTile && (other as SiblingRuleTile).sibingGroup == SibingGroup.Inner);
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !((other is SiblingRuleTile
                        && (other as SiblingRuleTile).sibingGroup == this.sibingGroup) || (this.sibingGroup == SibingGroup.Outer && other is SiblingRuleTile && (other as SiblingRuleTile).sibingGroup == SibingGroup.Inner));
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}