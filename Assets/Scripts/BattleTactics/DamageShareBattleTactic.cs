
public class DamageShareBattleTactic : BattleTactic {
    public void CastTactic(Hero hero1, Hero hero2) {
        hero1.ShareDamage(hero2);
    }

    public void StopTactic(Hero hero1, Hero hero2) {
        if (!hero1) return;
        hero1.ShareDamage(null);
    }

    public string GetDescription() {
        return "两个英雄平分在一定时间内所受的伤害";
    }
}

