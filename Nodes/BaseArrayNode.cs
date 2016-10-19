﻿namespace ReClassNET.Nodes
{
	abstract class BaseArrayNode : BaseReferenceNode
	{
		public int CurrentIndex { get; set; }
		public int Count { get; set; } = 1;

		public int Draw(ViewInfo view, int x, int y, string type, HotSpotType exchange)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Array, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name);
			x = AddText(view, x, y, Program.Settings.IndexColor, HotSpot.NoneId, "[");
			x = AddText(view, x, y, Program.Settings.IndexColor, 0, Count.ToString());
			x = AddText(view, x, y, Program.Settings.IndexColor, HotSpot.NoneId, "]");

			x = AddIcon(view, x, y, Icons.LeftArrow, 2, HotSpotType.Click);
			x = AddText(view, x, y, Program.Settings.IndexColor, HotSpot.NoneId, "(");
			x = AddText(view, x, y, Program.Settings.IndexColor, 1, CurrentIndex.ToString());
			x = AddText(view, x, y, Program.Settings.IndexColor, HotSpot.NoneId, ")");
			x = AddIcon(view, x, y, Icons.RightArrow, 3, HotSpotType.Click);

			x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name} Size={MemorySize}>");
			x = AddIcon(view, x + 2, y, Icons.Change, 4, exchange);

			x += view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;

			if (levelsOpen[view.Level])
			{
				y = DrawChild(view, tx, y);
			}

			return y;
		}

		protected abstract int DrawChild(ViewInfo view, int x, int y);

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				int value;
				if (int.TryParse(spot.Text, out value))
				{
					if (spot.Id == 0)
					{
						if (value != 0)
						{
							Count = value;

							(ParentNode as ClassNode)?.NotifyMemorySizeChanged();
						}
					}
					else
					{
						if (value < Count)
						{
							CurrentIndex = value;
						}
					}
				}
			}
			else if (spot.Id == 2)
			{
				if (CurrentIndex > 0)
				{
					--CurrentIndex;
				}
			}
			else if (spot.Id == 3)
			{
				if (CurrentIndex < Count - 1)
				{
					++CurrentIndex;
				}
			}
		}
	}
}