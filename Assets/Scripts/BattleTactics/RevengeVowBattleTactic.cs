
public class RevengeVowBattleTactic : BattleTactic {
    
    public void CastTactic(Hero hero1, Hero hero2) {
        hero1.StartRevengeVow(hero2);
    }

    public void StopTactic(Hero hero1, Hero hero2) {
        if (!hero1) return;
        hero1.StopRevengeVow(hero2);
    }
}

