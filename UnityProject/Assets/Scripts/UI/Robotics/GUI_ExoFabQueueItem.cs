﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_ExoFabQueueItem : DynamicEntry
{
	private GUI_ExosuitFabricator ExoFabMasterTab
	{
		get => MasterTab as GUI_ExosuitFabricator;
	}

	private MachineProduct product;

	public MachineProduct Product
	{
		get => product;
		set
		{
			product = value;
		}
	}

	private int numberInQueue;

	public int NumberInQueue
	{
		get => numberInQueue;
		set
		{
			numberInQueue = value;
		}
	}

	private GUI_ExoFabButton upButton;
	public GUI_ExoFabButton UpButton { get => upButton; }
	private GUI_ExoFabButton downButton;
	public GUI_ExoFabButton DownButton { get => downButton; }

	public void ForwardInQueue()
	{
		if (ExoFabMasterTab == null) { MasterTab.GetComponent<GUI_ExosuitFabricator>().OnProductAddClicked.Invoke(Product); }
		else { ExoFabMasterTab?.OnRemoveProductClicked.Invoke(NumberInQueue); }
	}

	public void BackwardsInQueue()
	{
		if (ExoFabMasterTab == null) { MasterTab.GetComponent<GUI_ExosuitFabricator>().OnProductAddClicked.Invoke(Product); }
		else { ExoFabMasterTab?.OnUpQueueClicked.Invoke(NumberInQueue); }
	}

	public void RemoveFromQueue()
	{
		if (ExoFabMasterTab == null) { MasterTab.GetComponent<GUI_ExosuitFabricator>().OnProductAddClicked.Invoke(Product); }
		else { ExoFabMasterTab?.OnDownQueueClicked.Invoke(NumberInQueue); }
	}

	public void ReInit()
	{
		if (product == null)
		{
			Logger.Log("ExoFab Product not found");
			return;
		}
		foreach (var element in Elements)
		{
			string nameBeforeIndex = element.name.Split('~')[0];
			switch (nameBeforeIndex)
			{
				case "QueueNumber":
					element.SetValue = NumberInQueue.ToString();
					break;

				case "ProductName":
					element.SetValue = Product.Name;
					break;

				case "UpButton":
					upButton = element as GUI_ExoFabButton;
					upButton.SetValue = "true";
					break;

				case "DownButton":
					downButton = element as GUI_ExoFabButton;
					downButton.SetValue = "true";
					break;
			}
		}
	}
}