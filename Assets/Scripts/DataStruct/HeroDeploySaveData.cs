
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HeroDeployData {
    public string HeroName;
    public Vector3 HeroPosition;
}

[Serializable]
public struct HeroDeploySaveData {
    public List<HeroDeployData> Datas;
}
