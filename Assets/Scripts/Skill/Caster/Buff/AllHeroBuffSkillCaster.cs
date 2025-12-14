using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHeroBuffSkillCaster : AddBuffSkillCaster {
    [Header("Prefab Settings")]
    [SerializeField] private GameObject prefabToCreate;
    [SerializeField] private float prefabDuration = 3f;
    
    private List<GameObject> createdPrefabs = new List<GameObject>();
    
    protected override void Cast(Transform attackTarget) {
        ClearExistingPrefabs();
        
        foreach(Hero hero in BattleManager.Instance.HeroesInBattle) {
            AddBuff(hero);
            CreatePrefabOnHero(hero);
        }
        
        StartCoroutine(ClearPrefabsAfterDelay());
    }
    
    private void CreatePrefabOnHero(Hero hero)
    {
        if (prefabToCreate == null || hero == null)
            return;
        
        GameObject instantiatedPrefab = Instantiate(prefabToCreate, hero.transform);
        instantiatedPrefab.transform.localPosition = Vector3.zero;
        instantiatedPrefab.transform.rotation = prefabToCreate.transform.rotation;
        createdPrefabs.Add(instantiatedPrefab);
    }
    
    private IEnumerator ClearPrefabsAfterDelay()
    {
        yield return new WaitForSeconds(prefabDuration);
        ClearExistingPrefabs();
    }
    
    private void ClearExistingPrefabs()
    {
        foreach(GameObject prefab in createdPrefabs)
        {
            if (prefab != null)
                Destroy(prefab);
        }
        createdPrefabs.Clear();
    }
    
    [ContextMenu("Test Create Prefabs on All Heroes")]
    private void TestCreatePrefabs()
    {
        if (BattleManager.Instance != null && BattleManager.Instance.HeroesInBattle != null)
        {
            foreach(Hero hero in BattleManager.Instance.HeroesInBattle)
            {
                CreatePrefabOnHero(hero);
            }
            StartCoroutine(ClearPrefabsAfterDelay());
        }
    }
}

