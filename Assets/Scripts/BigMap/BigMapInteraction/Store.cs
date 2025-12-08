
using System;
using System.Collections.Generic;
using UnityEngine;

public class Store : InteractionObject {

    [field: SerializeField] public List<string> Goods { get; private set; }

    protected override string GetName() {
        Vector3 pos = this.transform.position;
        return $"Store_{pos.x}_{pos.y}_{pos.z}";
    }

    protected override void Awake() {
        base.Awake();
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

