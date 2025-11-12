
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
            case BattleTacticType.ChangePosition:
                tactic = new ChangePositionBattleTactic();
                break;
            case BattleTacticType.ForceConcentration:
                tactic = new ForceConcentrationBattleTactic();
                break;
            case  BattleTacticType.DamageShare:
                tactic = new DamageShareBattleTactic();
                break;
            case BattleTacticType.RevengeVow:
                tactic = new RevengeVowBattleTactic();
                break;
        }
        if (tactic != null) Tactics.Add(type, tactic);
        return tactic;
    }
}


