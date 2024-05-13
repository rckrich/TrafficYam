
using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "TrafficObject", menuName = "ScriptableObjects/SO_TrafficObject", order = 0)]
public class SO_TrafficObject : ScriptableObject
{
    public string m_name;
    public TrafficObjectType m_type;
    public Sprite m_sprite;
    public string m_audioSFX;
    public int m_amount;
    public bool m_usesAnimController;
    public AnimatorController m_animatorController;
}
