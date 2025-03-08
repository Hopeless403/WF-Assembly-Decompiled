#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Sparkle : MonoBehaviourRect
{
	public Vector2 moveSpeed = new Vector2(0.1f, -1f);

	public Vector2 moveSway = new Vector2(0.6f, 0.3f);

	public Vector2 moveSwayTime = new Vector2(0.62f, 0.49f);

	public Vector3 rotation = new Vector3(0f, 0f, 0f);

	public float time;

	public Image _image;

	public float _size;

	public Image image
	{
		get
		{
			if (_image == null)
			{
				_image = GetComponent<Image>();
			}

			return _image;
		}
	}

	public float size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
			base.rectTransform.sizeDelta = new Vector2(_size, _size);
		}
	}

	public Color color
	{
		get
		{
			return image.color;
		}
		set
		{
			image.color = value;
		}
	}

	public Sprite sprite
	{
		get
		{
			return image.sprite;
		}
		set
		{
			image.sprite = value;
		}
	}

	public void Start()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.scale(base.gameObject, Vector3.one * 0.5f, Random.Range(1.5f, 1.75f)).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
	}

	public void Update()
	{
		time += Time.deltaTime;
		Vector2 vector = moveSway * moveSwayTime * Mathf.Sin(time);
		Vector2 add = (moveSpeed + vector) * size * Time.deltaTime;
		base.transform.localPosition = base.transform.localPosition.Add(add);
		Vector3 add2 = rotation * Time.deltaTime;
		base.transform.localEulerAngles = base.transform.localEulerAngles.Add(add2);
	}
}
