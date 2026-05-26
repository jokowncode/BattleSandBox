
using UnityEngine;


public class DebuffAdditionalPassiveEntry : PassiveEntry {

    [SerializeField] private float AdditionalPercentage = 0.1f;
    [SerializeField] private int AdditionalTimes = 1;

    public override void Construct(Hero _) {
        BuffManager.Instance.SetDebuffAdditional(this.AdditionalPercentage, this.AdditionalTimes, true);
    }

    public override void Destruct(Hero _) {
        BuffManager.Instance.SetDebuffAdditional(this.AdditionalPercentage, this.AdditionalTimes, false);
    }
}


