using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Balance Data", menuName = "Character Balance Data", order = 51)]
public class CharacterBalanceData : ScriptableObject
{
    public float Damage = 1.0f;
    public float Health = 1.0f;
}
