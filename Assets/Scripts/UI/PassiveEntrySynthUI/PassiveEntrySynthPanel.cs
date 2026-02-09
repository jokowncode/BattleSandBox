
using System;
using UnityEngine;

public class PassiveEntrySynthPanel : MonoBehaviour {

    [SerializeField] private AudioClip ErrorSfx;
    [SerializeField] private DetailGoodsButton WaitSynthButton;
    [SerializeField] private DetailGoodsButton AfterSynthButton;

    private PassiveEntry CurrentPassiveEntry;

    public Action<string, int> OnReturnPassiveEntry;
    
    private void Awake() {
        this.WaitSynthButton.OnButtonClicked += OnWaitSynthButtonClicked;
    }

    private void OnWaitSynthButtonClicked(string pName, int pCount) {
        if (!this.CurrentPassiveEntry) {
            PlayErrorSfx();
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

    private void PlayErrorSfx() {
        if (this.ErrorSfx) {
            AudioManager.Instance.PlaySfxAtPoint(this.transform.position, this.ErrorSfx);
        }
    }

    public void Synth() {
        if (!this.CurrentPassiveEntry) {
            PlayErrorSfx();
            return;
        }

        if (!PassiveEntryWarehouseManager.Instance.UpgradePassiveEntry(this.CurrentPassiveEntry.Data.Name)) {
            PlayErrorSfx();
            return;
        }
        this.GoBackToNormal();
    }

    public bool ChoosePassiveEntry(string pName, int pCount) {
        if (this.CurrentPassiveEntry) {
            PlayErrorSfx();
            return false;
        }

        PassiveEntry passiveEntry = PassiveEntryWarehouseManager.Instance.GetPassiveEntryByName(pName);
        if (!passiveEntry || !passiveEntry.UpgradePassiveEntry) {
            PlayErrorSfx();
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



