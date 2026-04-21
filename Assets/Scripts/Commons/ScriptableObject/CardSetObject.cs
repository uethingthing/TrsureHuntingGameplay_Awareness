using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardSet
{
    public string text;
    public Sprite sprite;
    public string info;
    public Sprite bigSprite;
}

[CreateAssetMenu(fileName = "CardSetObject", menuName = "Scriptable Objects/CardSetObject")]
public class CardSetObject : ScriptableObject
{
    public List<CardSet> Cards;
}
