﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AddressableReferences
{
	[System.Serializable]
	public class AddressableReference<T> where T : UnityEngine.Object
	{
		public UnLoadSetting SetLoadSetting = UnLoadSetting.KeepLoaded;
		public string Path = "";
		public AssetReference AssetReference = null;

		public bool IsNotValidKey => NotValidKey();
		public bool IsReadyLoaded => ReadyLoaded();

		#region InternalStuff

		private bool ReadyLoaded()
		{
			if (IsNotValidKey) return false;
			return AssetReference.Asset != null;
		}

		public bool NotValidKey()
		{
			if (AssetReference.RuntimeKey == null) return true;
			return false;
		}
		#endregion
		#region Externally accessible stuff

		/// <summary>
		/// If you manually want the asset to be ready on demand
		/// </summary>
		public void Preload()
		{
			if (IsNotValidKey) return;
			if (IsReadyLoaded) return;
			AssetReference.LoadAssetAsync<T>();
		}


		/// <summary>
		/// Assuming that you've loaded the asset allow you to access it instantly
		/// </summary>
		public T Retrieve()
		{
			if (IsNotValidKey) return null;
			if (IsReadyLoaded)
			{
				return (T) AssetReference.Asset;
			}
			else
			{
				Logger.LogError("Asset is not loaded", Category.Addressables);
				return null;
			}
		}

		/// <summary>
		/// Load asset
		/// </summary>
		public async Task<T> Load()
		{
			if (IsNotValidKey) return null;
			if (IsReadyLoaded)
			{
				return (T) AssetReference.Asset;
			}

			//Add to manager tracker
			await AssetReference.LoadAssetAsync<T>().Task;
			return (T) (AssetReference.Asset);
		}

		/// <summary>
		/// Load an asset and passes the handle to the AssetManager
		/// </summary>
		public async Task<T> LoadThroughAssetManager()
		{
			if (IsNotValidKey) return null;
			if (IsReadyLoaded)
			{
				return (T)AssetReference.Asset;
			}

			//Add to manager tracker
			var handle = AssetReference.LoadAssetAsync<T>();
			AssetManager.Instance.AddLoadingAssetHandle(handle, Path);
			await AssetReference.LoadAssetAsync<T>().Task;
			return (T)(AssetReference.Asset);
		}

		public void Unload()
		{
			if (IsNotValidKey) return;
			if (IsReadyLoaded)
			{
				//Check manager To see if it's implemented
				Logger.Log("Not implemented yet");
			}
		}
		#endregion
	}

	public enum UnLoadSetting
	{
		KeepLoaded, //Keep loaded until the game closes
		UnloadOnRoundEnd,//Unloads when the round ends
		When0Referenced //Unloads when there are zero references
	}

	public enum LoadSetting
	{
		PreLoad, //Preload on game start
		PreLoadScene,//Preload on Scene then Unload once Scene Has changed
		OnDemand //Load and unload when it's needed
	}

	[System.Serializable]
	public class AddressableSprite : AddressableReference<Sprite> { }
}