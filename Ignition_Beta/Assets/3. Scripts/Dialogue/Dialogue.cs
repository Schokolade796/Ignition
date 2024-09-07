using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Text/Dialogue", order = int.MaxValue)]
public class Dialogue : ScriptableObject
{
    [SerializeField, Tooltip("���⿡ �� ���� ��")]
    string[] sentences;
    public string[] Sentences { get { return sentences; } }
}
