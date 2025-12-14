using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance;

    [Header("目标标签")]
    public string enemyTag = "Enemy";
    public string allyTag = "Ally";

    private List<Transform> enemyTargets = new List<Transform>();
    private List<Transform> allyTargets = new List<Transform>();

    // 记录每个目标被哪些技能使用（使用HashSet来记录使用该目标的技能物体实例ID）
    private Dictionary<Transform, HashSet<int>> enemyTargetUsers = new Dictionary<Transform, HashSet<int>>();
    private Dictionary<Transform, HashSet<int>> allyTargetUsers = new Dictionary<Transform, HashSet<int>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RefreshTargets();
    }

    public void RefreshTargets()
    {
        // 查找所有敌方和己方目标
        enemyTargets.Clear();
        allyTargets.Clear();

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] allyObjects = GameObject.FindGameObjectsWithTag(allyTag);

        enemyTargets.AddRange(enemyObjects.Select(go => go.transform));
        allyTargets.AddRange(allyObjects.Select(go => go.transform));

        // 初始化使用记录
        foreach (var target in enemyTargets)
        {
            if (!enemyTargetUsers.ContainsKey(target))
            {
                enemyTargetUsers[target] = new HashSet<int>();
            }
        }

        foreach (var target in allyTargets)
        {
            if (!allyTargetUsers.ContainsKey(target))
            {
                allyTargetUsers[target] = new HashSet<int>();
            }
        }
    }

    // 请求一个敌方目标，传入请求者的实例ID
    public Transform RequestEnemyTarget(int requesterInstanceID)
    {
        // 找出未被使用的敌方目标，或者已经被当前请求者使用的目标（允许同一个请求者多次请求同一个目标？但我们要求不同技能不同目标，所以一个请求者只能请求一个目标？）
        // 但是注意，一个技能物体可能只有一个请求者，所以我们可以先找出未被任何技能使用的目标，如果都已被使用，则返回null？或者返回最近的一个？
        // 这里我们要求每个技能物体只能有一个目标，所以一个技能物体再次请求时，会先释放之前的目标。

        // 我们先找出未被使用的目标
        var availableTargets = enemyTargets.Where(t => !enemyTargetUsers[t].Any()).ToList();

        if (availableTargets.Count == 0)
        {
            // 没有可用的敌方目标，返回null
            return null;
        }

        // 选择最近的目标（以第一个请求者的位置为参考？但是每个技能物体的位置不同，所以不能以管理器的位置为准）
        // 我们需要请求者的位置，但是这里没有传入位置。所以我们改为返回第一个可用的目标？或者随机？
        // 由于没有请求者的位置，我们暂时返回第一个可用的目标。
        Transform target = availableTargets[0];

        // 记录该目标被此请求者使用
        if (!enemyTargetUsers[target].Contains(requesterInstanceID))
        {
            enemyTargetUsers[target].Add(requesterInstanceID);
        }

        return target;
    }

    // 请求一个己方目标
    public Transform RequestAllyTarget(int requesterInstanceID)
    {
        var availableTargets = allyTargets.Where(t => !allyTargetUsers[t].Any()).ToList();

        if (availableTargets.Count == 0)
        {
            return null;
        }

        Transform target = availableTargets[0];

        if (!allyTargetUsers[target].Contains(requesterInstanceID))
        {
            allyTargetUsers[target].Add(requesterInstanceID);
        }

        return target;
    }

    // 释放目标（当技能物体不再需要目标时）
    public void ReleaseEnemyTarget(Transform target, int requesterInstanceID)
    {
        if (enemyTargetUsers.ContainsKey(target))
        {
            enemyTargetUsers[target].Remove(requesterInstanceID);
        }
    }

    public void ReleaseAllyTarget(Transform target, int requesterInstanceID)
    {
        if (allyTargetUsers.ContainsKey(target))
        {
            allyTargetUsers[target].Remove(requesterInstanceID);
        }
    }

    // 当目标被销毁时，从列表中移除
    public void RemoveEnemyTarget(Transform target)
    {
        if (enemyTargets.Contains(target))
        {
            enemyTargets.Remove(target);
            enemyTargetUsers.Remove(target);
        }
    }

    public void RemoveAllyTarget(Transform target)
    {
        if (allyTargets.Contains(target))
        {
            allyTargets.Remove(target);
            allyTargetUsers.Remove(target);
        }
    }
}