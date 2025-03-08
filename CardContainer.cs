#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class CardContainer : MonoBehaviourRect, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IList<Entity>, ICollection<Entity>, IEnumerable<Entity>, IEnumerable
{
	[Required(null)]
	public RectTransform holder;

	public UINavigationItem nav;

	public List<CardContainer> shoveTo = new List<CardContainer>();

	public CardContainer _group;

	public readonly List<Entity> entities = new List<Entity>();

	public Character owner;

	public bool canBePlacedOn;

	public bool canPlayOn;

	public bool canHover = true;

	public int max;

	public Vector3 gap;

	public Vector3 childHoverOffset;

	[Header("Movement Tween")]
	public Vector2 movementDurRand = new Vector2(0.3f, 0.4f);

	public LeanTweenType movementEase = LeanTweenType.easeOutQuart;

	[Header("Scale Tween")]
	public Vector2 scaleDurRand = new Vector2(0.2f, 0.2f);

	public LeanTweenType scaleEase = LeanTweenType.easeOutQuart;

	[Header("Events")]
	public UnityEventEntity onAdd;

	public UnityEventEntity onRemove;

	public CardController _cc;

	[SerializeField]
	public bool poolCardsOnDestroy = true;

	public const int MaxTweens = 100;

	public virtual CardContainer Group
	{
		get
		{
			if (!_group)
			{
				return this;
			}

			return _group;
		}
		set
		{
			_group = value;
		}
	}

	public virtual int Count { get; set; }

	public virtual float CardScale => holder.sizeDelta.y / 4.5f * 1f;

	public bool Empty => Count <= 0;

	public CardController cc
	{
		get
		{
			if (!_cc)
			{
				_cc = CardController.Find(base.gameObject);
			}

			return _cc;
		}
	}

	public bool IsReadOnly => false;

	public virtual int ChildCount => holder.childCount;

	public virtual Entity this[int index]
	{
		get
		{
			if (entities.Count <= index)
			{
				return null;
			}

			return entities[index];
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public static CardContainer[] FindAll()
	{
		CardContainer[] array = UnityEngine.Object.FindObjectsOfType<CardContainer>();
		HashSet<CardContainer> hashSet = GenericPool<HashSet<CardContainer>>.Get();
		hashSet.Clear();
		CardContainer[] array2 = array;
		foreach (CardContainer cardContainer in array2)
		{
			if (cardContainer.Group != cardContainer)
			{
				hashSet.AddIfNotNull(cardContainer);
			}
		}

		CardContainer[] result = hashSet.ToArray();
		GenericPool<HashSet<CardContainer>>.Release(hashSet);
		return result;
	}

	public CardContainer[] GetSecondaryContainers(Entity entity)
	{
		List<CardContainer> list = GenericPool<List<CardContainer>>.Get();
		list.Clear();
		CardContainer[] array = FindAll();
		foreach (CardContainer cardContainer in array)
		{
			if (cardContainer != this && cardContainer.Contains(entity))
			{
				list.Add(cardContainer);
			}
		}

		CardContainer[] result = list.ToArray();
		GenericPool<List<CardContainer>>.Release(list);
		return result;
	}

	public virtual void AssignController(CardController controller)
	{
		_cc = controller;
	}

	public void Start()
	{
		nav = null;
		CheckForNavigationItem(base.transform);
	}

	public void CheckForNavigationItem(Transform inTransform)
	{
		if ((bool)inTransform.GetComponent<UINavigationItem>())
		{
			nav = inTransform.GetComponent<UINavigationItem>();
			return;
		}

		for (int i = 0; i < inTransform.childCount; i++)
		{
			CheckForNavigationItem(inTransform.GetChild(i));
		}
	}

	public void OnDestroy()
	{
		if (!GameManager.End && poolCardsOnDestroy)
		{
			for (int num = entities.Count - 1; num >= 0; num--)
			{
				CardManager.ReturnToPool(entities[num]);
			}
		}
	}

	public virtual void SetSize(int size, float cardScale)
	{
		max = size;
	}

	public virtual void Add(Entity entity)
	{
		entity.transform.SetParent(holder);
		entity.AddTo(this);
		entities.Add(entity);
		Count++;
		CardAdded(entity);
		onAdd.Invoke(entity);
	}

	public virtual void Insert(int index, Entity entity)
	{
		entity.transform.SetParent(holder);
		entity.AddTo(this);
		entities.Insert(index, entity);
		Count++;
		entity.transform.SetSiblingIndex(index);
		CardAdded(entity);
		onAdd.Invoke(entity);
	}

	public virtual bool PushForwards(int fromIndex)
	{
		throw new NotImplementedException();
	}

	public virtual bool PushBackwards(int fromIndex)
	{
		throw new NotImplementedException();
	}

	public virtual void MoveChildrenForward()
	{
		throw new NotImplementedException();
	}

	public virtual void Remove(Entity entity)
	{
		if (!entity.inCardPool)
		{
			entity.transform.SetParent(null);
		}

		entity.RemoveFrom(this);
		entities.Remove(entity);
		Count--;
		CardRemoved(entity);
		onRemove.Invoke(entity);
		Debug.Log("[" + entity.name + "] Removed From [" + base.name + "]");
	}

	public virtual void RemoveAt(int index)
	{
		Remove(this[index]);
	}

	public virtual Entity GetTop()
	{
		if (entities.Count <= 0)
		{
			return null;
		}

		return entities[entities.Count - 1];
	}

	public virtual Vector3 GetChildPosition(Entity child)
	{
		return Vector3.zero;
	}

	public virtual Vector3 GetChildScale(Entity child)
	{
		return Vector3.one * CardScale;
	}

	public virtual Vector3 GetChildRotation(Entity child)
	{
		return Vector3.zero;
	}

	public virtual int GetChildDrawOrder(Entity child)
	{
		return 0;
	}

	public virtual void CardAdded(Entity entity)
	{
	}

	public virtual void CardRemoved(Entity entity)
	{
	}

	public bool IsPrimaryContainer(Entity entity)
	{
		if ((bool)entity && entity.actualContainers.Count > 0)
		{
			return entity.actualContainers[0] == this;
		}

		return false;
	}

	[Button(null, EButtonEnableMode.Always)]
	public virtual void TweenChildPositions()
	{
		if (Count >= 100)
		{
			SetChildPositions();
			return;
		}

		foreach (Entity item in this.Where((Entity a) => a.alive))
		{
			item.TweenToContainer();
		}
	}

	public virtual void SetChildPositions()
	{
		using (IEnumerator<Entity> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Entity current = enumerator.Current;
				SetChildPosition(current);
			}
		}
	}

	public virtual void TweenChildPosition(Entity child)
	{
		child.TweenToContainer();
	}

	public virtual void SetChildPosition(Entity child)
	{
		child.transform.localPosition = GetChildPosition(child);
		child.transform.localScale = GetChildScale(child);
		child.transform.localEulerAngles = GetChildRotation(child);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (canHover)
		{
			Hover();
		}
	}

	public virtual void Hover()
	{
		if ((bool)cc)
		{
			cc.HoverContainer(this);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (canHover)
		{
			UnHover();
		}
	}

	public virtual void UnHover()
	{
		if ((bool)cc && cc.hoverContainer == this)
		{
			cc.UnHoverContainer();
		}
	}

	public virtual int IndexOf(Entity item)
	{
		return entities.IndexOf(item);
	}

	public virtual void Clear()
	{
		for (int num = Count - 1; num >= 0; num--)
		{
			RemoveAt(num);
		}
	}

	public void DestroyAll()
	{
		using (IEnumerator<Entity> enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CardManager.ReturnToPool(enumerator.Current);
			}
		}
	}

	public void ClearAndDestroyAllImmediately()
	{
		Entity[] array = ToArray();
		Clear();
		Entity[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].gameObject.DestroyImmediate();
		}
	}

	public virtual bool Contains(Entity item)
	{
		return entities.Contains(item);
	}

	public virtual Entity[] ToArray()
	{
		return entities.ToArray();
	}

	public void CopyTo(Entity[] array, int arrayIndex)
	{
		entities.CopyTo(array, arrayIndex);
	}

	bool ICollection<Entity>.Remove(Entity item)
	{
		throw new NotImplementedException();
	}

	public virtual IEnumerator<Entity> GetEnumerator()
	{
		return entities.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
