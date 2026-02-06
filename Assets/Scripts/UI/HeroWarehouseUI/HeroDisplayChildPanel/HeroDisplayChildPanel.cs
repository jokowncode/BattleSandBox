
using System;
using UnityEngine;

public abstract class HeroDisplayChildPanel : MonoBehaviour {

    public void Show(Hero hero) {
        if (this.gameObject.activeSelf) return;
        ShowData(hero);
        this.gameObject.SetActive(true);
    }

    protected abstract void ShowData(Hero hero);

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}


