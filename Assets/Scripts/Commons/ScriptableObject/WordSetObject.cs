using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordSetObject", menuName = "Scriptable Objects/WordSetObject")]
public class WordSetObject : ScriptableObject
{
    public List<string> Words;
}
