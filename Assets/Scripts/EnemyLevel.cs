using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EnemyLevel
{
    public int Level;

    [SerializeField]
    public List<EnemyPrefab> EnemyPrefabs;
}

[Serializable]
public class EnemyPrefab
{
    public int Weight = 1;

    public GameObject Prefab;
}