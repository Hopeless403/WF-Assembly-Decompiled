using System;
using UnityEngine;

namespace Deadpan.Enums.Engine.Components.Modding
{
	public abstract class DataFileBuilder<T, Y> where T : DataFile where Y : DataFileBuilder<T, Y>, new()
	{
		public delegate void AfterBuildDelegate(T d);

		internal T _data;

		public WildfrostMod Mod;

		public event AfterBuildDelegate AfterBuildEvent;

		public event AfterBuildDelegate AfterAllModBuildsEvent;

		protected virtual T BuildInstance()
		{
			return _data.InstantiateKeepName();
		}

		public T Build()
		{
			T val = BuildInstance();
			OnAfterBuildEvent(val);
			return val;
		}

		protected DataFileBuilder(WildfrostMod mod)
		{
			Mod = mod;
		}

		protected DataFileBuilder()
		{
		}

		public Y FreeModify(Action<T> action)
		{
			action?.Invoke(_data);
			return (Y)this;
		}

		public Y FreeModify<D>(Action<D> action) where D : T
		{
			action?.Invoke(_data as D);
			return (Y)this;
		}

		public virtual Y Create(string name)
		{
			if (Mod != null)
			{
				name = Extensions.PrefixGUID(name, Mod);
			}
			if (_data != null)
			{
				UnityEngine.Object.Destroy(_data);
			}
			_data = ScriptableObject.CreateInstance<T>();
			_data.name = name;
			return this as Y;
		}

		public virtual Y Create<X>(string name) where X : T
		{
			if (Mod != null)
			{
				name = Extensions.PrefixGUID(name, Mod);
			}
			if (_data != null)
			{
				UnityEngine.Object.Destroy(_data);
			}
			_data = (T)ScriptableObject.CreateInstance<X>();
			_data.name = name;
			return this as Y;
		}

		public static implicit operator T(DataFileBuilder<T, Y> t)
		{
			return t.Build();
		}

		public virtual Y SubscribeToBuildEvent(AfterBuildDelegate d)
		{
			AfterBuildEvent += d;
			return (Y)this;
		}

		public virtual Y UnsubscribeToBuildEvent(AfterBuildDelegate d)
		{
			AfterBuildEvent -= d;
			return (Y)this;
		}

		public virtual Y SubscribeToAfterAllBuildEvent(AfterBuildDelegate d)
		{
			AfterAllModBuildsEvent += d;
			return (Y)this;
		}

		public virtual Y UnubscribeToAfterAllBuildEvent(AfterBuildDelegate d)
		{
			AfterAllModBuildsEvent -= d;
			return (Y)this;
		}

		protected virtual void OnAfterBuildEvent(T d)
		{
			this.AfterBuildEvent?.Invoke(d);
		}

		internal virtual void OnAfterAllModBuildsEvent(T d)
		{
			this.AfterAllModBuildsEvent?.Invoke(d);
		}
	}
}
