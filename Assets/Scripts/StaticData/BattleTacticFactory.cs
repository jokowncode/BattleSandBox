
using System;
using System.Collections.Generic;

public static class BattleTacticFactory {
    
    private static readonly Dictionary<BattleTacticType, BattleTactic> Tactics = new Dictionary<BattleTacticType, BattleTactic>();
    
    public static BattleTactic CreateBattleTactic(BattleTacticType type) {
        if (Tactics.TryGetValue(type, out BattleTactic battleTactic)) {
            return battleTactic;
        } 
        
        BattleTactic tactic = null;
        switch (type) {
            case BattleTacticType.移形换位:
                tactic = new ChangePositionBattleTactic();
                break;
            case BattleTacticType.集中火力:
                tactic = new ForceConcentrationBattleTactic();
                break;
            case  BattleTacticType.伤害共享:
                tactic = new DamageShareBattleTactic();
                break;
            case BattleTacticType.复仇誓言:
                tactic = new RevengeVowBattleTactic();
                break;
        }
        if (tactic != null) Tactics.Add(type, tactic);
        return tactic;
    }

    public static string GetBattleTacticDescription(BattleTacticType type) {
        BattleTactic tactic = CreateBattleTactic(type);
        return tactic == null ? "" : tactic.GetDescription();
    }
}


