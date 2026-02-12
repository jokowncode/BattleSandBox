
using System;
using UnityEngine;

public class PassiveEntrySynthPanel : MonoBehaviour {

    [SerializeField] private DetailButton WaitSynthButton;
    [SerializeField] private DetailButton AfterSynthButton;

    private PassiveEntry CurrentPassiveEntry;

    public Action<string, int> OnReturnPassiveEntry;
    
    private void Awake() {
        this.WaitSynthButton.OnButtonClicked += OnWaitSynthButtonClicked;
    }

    private void OnWaitSynthButtonClicked(string pName, int pCount) {
        if (!this.CurrentPassiveEntry) {
            AudioManager.Instance.PlayErrorSfx();
            return;
        }

        this.GoBackToNormal();
        OnReturnPassiveEntry?.Invoke(pName, pCount);
    }

    public void GoBackToNormal() {
        this.CurrentPassiveEntry = null;
        this.WaitSynthButton.SetData("", "", 0, false, GoodsType.None);
        this.AfterSynthButton.SetData("", "", 0, false, GoodsType.None);
    }

    public void Synth() {
        if (!this.CurrentPassiveEntry) {
            AudioManager.Instance.PlayErrorSfx();
            return;
        }

        if (!PassiveEntryWarehouseManager.Instance.UpgradePassiveEntry(this.CurrentPassiveEntry.Data.Name)) {
            AudioManager.Instance.PlayErrorSfx();
            return;
        }
        this.GoBackToNormal();
    }

    public bool ChoosePassiveEntry(string pName, int pCount) {
        if (this.CurrentPassiveEntry) {
            AudioManager.Instance.PlayErrorSfx();
            return false;
        }

        PassiveEntry passiveEntry = PassiveEntryWarehouseManager.Instance.GetPassiveEntryByName(pName);
        if (!passiveEntry || !passiveEntry.UpgradePassiveEntry) {
            SceneChangeManager.Instance.AddGameTip("当前词条已是满级");
            AudioManager.Instance.PlayErrorSfx();
            return false;
        }
        this.CurrentPassiveEntry = passiveEntry;
        PassiveEntry upgradePassiveEntry = passiveEntry.UpgradePassiveEntry;
        this.WaitSynthButton.SetData(passiveEntry.Data.Description, pName, pCount,
            true, (GoodsType)((int)passiveEntry.Data.Rare));
        this.AfterSynthButton.SetData(upgradePassiveEntry.Data.Description, upgradePassiveEntry.Data.Name, 1,
            false, (GoodsType)((int)upgradePassiveEntry.Data.Rare));
        return true;
    }
}



