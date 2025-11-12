
using UnityEngine;

public class ForceConcentrationBattleTactic : BattleTactic {
    public void CastTactic(Hero hero1, Hero hero2) {
        Vector3 pos = hero1.transform.position + Vector3.left * 1.5f;
        hero2.ChangePositionWithTrail(pos);
        hero2.OnDisappear?.Invoke();
    }

    public void StopTactic(Hero hero1, Hero hero2) { }
}


