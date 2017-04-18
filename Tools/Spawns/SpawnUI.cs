﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;
using System.Text;
using ModdersToolkit.UIElements;
using ModdersToolkit.Tools;

namespace ModdersToolkit.Tools.Spawns
{
	class SpawnUI : UIState
	{
		internal UIPanel mainPanel;
		public UIList checklistList;
		private UserInterface userInterface;

		public SpawnUI(UserInterface userInterface)
		{
			this.userInterface = userInterface;
		}

		public override void OnInitialize()
		{
			mainPanel = new UIPanel();
			mainPanel.SetPadding(6);
			mainPanel.Left.Set(-250f, 1f);
			mainPanel.Top.Set(-420f, 1f);
			mainPanel.Width.Set(200f, 0f);
			mainPanel.Height.Set(350f, 0f);
			mainPanel.BackgroundColor = new Color(173, 94, 171);

			UIText text = new UIText("NPC Spawns:", 0.85f);
		//	text.Top.Set(12f, 0f);
		//	text.Left.Set(12f, 0f);
			mainPanel.Append(text);

			int top = 20;

			UITextPanel<string> calculateButton = new UITextPanel<string>("Calculate");
			calculateButton.SetPadding(4);
			calculateButton.Width.Set(-10, 0.5f);
			calculateButton.Top.Set(top, 0f);
		//	calculateButton.Left.Set(6, 0f);
			calculateButton.OnClick += CaluculateButton_OnClick; ;
			mainPanel.Append(calculateButton);

			top += 28;

			checklistList = new UIList();
		//	checklistList.SetPadding(6);
			checklistList.Top.Pixels = top;
			checklistList.Width.Set(-25f, 1f);
			checklistList.Height.Set(-top, 1f);
			checklistList.ListPadding = 6f;
			mainPanel.Append(checklistList);

			var checklistListScrollbar = new UIElements.FixedUIScrollbar(userInterface);
			checklistListScrollbar.SetView(100f, 1000f);
			//checklistListScrollbar.Height.Set(0f, 1f);
			checklistListScrollbar.Top.Pixels = top;
			checklistListScrollbar.Height.Set(-top, 1f);
			checklistListScrollbar.Left.Set(-20, 1f);
			//checklistListScrollbar.HAlign = 1f;
			mainPanel.Append(checklistListScrollbar);
			checklistList.SetScrollbar(checklistListScrollbar);

			Append(mainPanel);
		}

		private void CaluculateButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.NewText("Calculating....");

			SpawnTool.CalculateSpawns();

			checklistList.Clear();

			float total = 0;
			foreach (var spawn in SpawnTool.spawns)
			{
				total += spawn.Value;
			}

			if (total > 0)
			{
				foreach (var spawn in SpawnTool.spawns)
				{
					UINPCSpawnInfo spawnInfo = new UINPCSpawnInfo(spawn.Key, spawn.Value / total);
					checklistList.Add(spawnInfo);
				}
			}

			Main.NewText($"spawnRate: {SpawnTool.spawnRate}");
			Main.NewText($"maxSpawns: {SpawnTool.maxSpawns}");
			//Main.NewText($"activeNPCs: {Main.LocalPlayer.activeNPCs}");
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (mainPanel.ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}
}
