
using UnityEngine;

public class ChangePositionBattleTactic : BattleTactic{
    public void CastTactic(Hero hero1, Hero hero2) {
        Vector3 hero1Pos = hero1.transform.position;
        hero1.ChangePositionWithTrail(hero2.transform.position);
        hero2.ChangePositionWithTrail(hero1Pos);
        
        hero1.OnDisappear?.Invoke();
        hero2.OnDisappear?.Invoke();
    }
}

