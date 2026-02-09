
public class RevengeVowBattleTactic : BattleTactic {
    
    public void CastTactic(Hero hero1, Hero hero2) {
        hero1.StartRevengeVow(hero2);
        hero2.StartRevengeVow(hero1);
    }

    public void StopTactic(Hero hero1, Hero hero2) {
        if (hero1) hero1.StopRevengeVow(hero2);
        if (hero2) hero2.StopRevengeVow(hero1);
    }

    public string GetDescription() {
        return "若在一定时间内角色死去，另一角色获得复仇加成";
    }
}

