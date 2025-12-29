
using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [field: SerializeField] public List<string> Goods { get; private set; }

    protected override void Awake() {
        base.Awake();
        this.IsEndCanEnableInteraction = true;
    }

    protected override string GetName() {
        Vector3 pos = this.transform.position;
        string dungeonName = SceneChangeManager.Instance.CurrentDungeonName;
        return $"{dungeonName}_Store_{pos.x}_{pos.y}_{pos.z}";
    }

    protected override void LoadBigMapData() {
        // TODO: TEMP -> IN DEMO, Store Goods Are Fixed -> Not Reset When Restart Dungeon
        /*if (!this.IsEnd) {
            if(PlayerPrefs.HasKey(GetName())) PlayerPrefs.DeleteKey(GetName());
            this.EndInteraction();
        }*/

        if (PlayerPrefs.HasKey(GetName())) {
            string json = PlayerPrefs.GetString(GetName());
            this.Goods = JsonUtility.FromJson<Serialization<string>>(json).ToList();
        }
    }

    private void OnDestroy() {
        string json = JsonUtility.ToJson(new Serialization<string>(this.Goods));
        PlayerPrefs.SetString(GetName(), json);
    }

    protected override void Interaction() {
        ShowStore();
    }

    public void ShowStore() {
        if (BigMapUIManager.Instance.IsOpenStore) return;
        StoreUI.Instance.ShowStoreUI(this);
    }

    public void RemoveGoods(string goodsName) {
        if(this.Goods.Contains(goodsName)) this.Goods.Remove(goodsName);
    }
}

