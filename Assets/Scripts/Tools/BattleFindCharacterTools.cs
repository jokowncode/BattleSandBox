
using UnityEngine;
using System.Collections;

public static class BattleFindCharacterTools {
    private static List<Fighter> GetFightersByType(TargetType type) {
        if (!BattleManager.Instance) return null;
        return new List<Fighter>(type == TargetType.Hero ? BattleManager.Instance.HeroesInBattle : BattleManager.Instance.EnemiesInBattle);
    }

    private static List<Fighter> GetSortedFightersByDistance(Fighter self) {
        List<Fighter> result = GetFightersByType(self.AttackTargetType);
        if (result == null) return null;
        result.Sort((Fighter f1, Fighter f2) => {
            float d1 = (self.transform.position - f1.transform.position).sqrMagnitude;
            float d2 = (self.transform.position - f2.transform.position).sqrMagnitude;
            return d1 > d2 ? 1 : (d1 < d2 ? -1 : 0);
        });
        return result;
    }

    public static bool HasBeDamagedTarget(TargetType type) {
        List<Fighter> fighters = GetFightersByType(type);
        if (fighters == null) return false;
        foreach (Fighter f in fighters){
            if (f.HealthPercentage < 1.0f) {
                return true;
            }
        }
        return false;
    }

    public static Fighter FindMinHealthPercentageHero(TargetType type){
        List<Fighter> fighters = GetFightersByType(type);
        if (fighters == null) return null;

        Fighter result = null;
        float minPercentage = 1.0f;
        foreach (Fighter f in fighters){
            if (f.HealthPercentage < minPercentage){
                minPercentage = f.HealthPercentage;
                result = f;
            }
        }
        return result;
    }

    public static Fighter FindFurthestTarget(TargetType type, Vector3 position) {
        List<Fighter> fighters = GetFightersByType(type);
        if (fighters == null) return null;

        float maxDistance = -1.0f;
        Fighter result = null;
        foreach (Fighter f in fighters) {
            float distance = (position - f.transform.position).sqrMagnitude;
            if (distance > maxDistance) {
                maxDistance = distance;
                result = f;
            }
        }
        return result;
    }

    public static Fighter GetNearestFighter(Fighter selfFighter, Func<Fighter, bool> condition = null) {
        List<Fighter> sortedFighter = GetSortedFightersByDistance(selfFighter);
        if (sortedFighter == null) return null;

        if (condition == null) return sortedFighter[0];
        foreach (Fighter f in sortedFighter) {
            if (condition(f)) {
                return f;
            }
        }
        return null;
    }

    public static List<Fighter> GetRandomCountFighter(TargetType type, int count) {
        int size = type == TargetType.Hero ? this.HeroesInBattle.Count : this.EnemiesInBattle.Count;
        List<Fighter> result = new List<Fighter>();
        if (count >= size) {
            result.AddRange(type == TargetType.Hero ? this.HeroesInBattle : this.EnemiesInBattle);
            return result;
        }

        List<int> container = new List<int>();
        for (int i = 0; i < size; i++) {
            container.Add(i);
        }

        int k = container.Count - 1;
        for (int j = 1; j <= count; j++) {
            int randomIndex = Random.Range(0, k);
            (container[randomIndex], container[k]) = (container[k], container[randomIndex]);
            int index = container[k];
            result.Add(type == TargetType.Hero ? this.HeroesInBattle[index] : this.EnemiesInBattle[index]);
            k--;
        }
        return result;
    }

    public static Fighter GetRandomFighter(TargetType type, Func<Fighter, bool> condition = null) {
        if (!BattleManager.Instance || BattleManager.Instance.IsGameOver) return null;
        List<Fighter> fighters = GetFightersByType(type);
        if (fighters == null) return null;
        int randomIndex = UnityEngine.Random.Range(0, fighters.Count);
        if (condition == null || condition(fighters[randomIndex])) return fighters[randomIndex];
        
        int index = randomIndex + 1;
        Fighter fighter = fighters[index % fighters.Count];
        while (index % fighters.Count != randomIndex && !condition(fighter)){
            index++;
            fighter = fighters[index % fighters.Count];
        }
        if (index % fighters.Count == randomIndex) return null;
        return fighter;
    }
}

