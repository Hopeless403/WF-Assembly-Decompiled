#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Profile", menuName = "Blood Profile")]
public class BloodProfile : ScriptableObject
{
	public bool variableColor;

	[HideIf("variableColor")]
	public Color color;

	[ShowIf("variableColor")]
	public Gradient colorRange;

	[Range(0f, 2f)]
	public float bleedFactor = 1f;

	public SplatterParticle splatterParticlePrefab;
}
