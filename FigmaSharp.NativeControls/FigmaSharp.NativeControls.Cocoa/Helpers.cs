using System;
using FigmaSharp.Models;
using LiteForms;
using LiteForms.Cocoa;
using LocalFile.Cocoa;

namespace FigmaSharp.NativeControls.Cocoa
{
	public class TransitionHelper
	{
		public static IButton CreateButtonFromFigmaNode(FNSButton button, FigmaNode currentNode)
		{
			IButton btn = null;
			if (currentNode is FigmaFrameEntity figmaFrameEntity)
			{
				if (!string.IsNullOrEmpty(figmaFrameEntity.transitionNodeID))
				{
					btn = new FigmaTransitionButton(button)
					{
						TransitionDuration = figmaFrameEntity.transitionDuration,
						TransitionEasing = figmaFrameEntity.transitionEasing,
						TransitionNodeID = figmaFrameEntity.transitionNodeID,
					};
				}
			}
			if (btn == null)
				btn = new Button(button);
			return btn;
		}

		public static IButton CreateButtonFromFigmaNode(FigmaNode currentNode)
		{
			IButton btn = null;
			if (currentNode is FigmaFrameEntity figmaFrameEntity)
			{
				if (!string.IsNullOrEmpty(figmaFrameEntity.transitionNodeID))
				{
					btn = new FigmaTransitionButton()
					{
						TransitionDuration = figmaFrameEntity.transitionDuration,
						TransitionEasing = figmaFrameEntity.transitionEasing,
						TransitionNodeID = figmaFrameEntity.transitionNodeID,
					};
				}
			}
			if (btn == null)
				btn = new Button();
			return btn;
		}

		public static IButton CreateImageButtonFromFigmaNode(FNSButton button, FigmaNode currentNode)
		{
			IButton btn = null;
			if (currentNode is FigmaFrameEntity figmaFrameEntity)
			{
				if (!string.IsNullOrEmpty(figmaFrameEntity.transitionNodeID))
				{
					btn = new FigmaTransitionImageButton(button)
					{
						TransitionDuration = figmaFrameEntity.transitionDuration,
						TransitionEasing = figmaFrameEntity.transitionEasing,
						TransitionNodeID = figmaFrameEntity.transitionNodeID,
					};
				}
			}
			if (btn == null)
				btn = new Button(button);
			return btn;
		}

		public static IButton CreateImageButtonFromFigmaNode(FigmaNode currentNode)
		{
			IButton btn = null;
			if (currentNode is FigmaFrameEntity figmaFrameEntity)
			{
				if (!string.IsNullOrEmpty(figmaFrameEntity.transitionNodeID))
				{
					btn = new FigmaTransitionImageButton()
					{
						TransitionDuration = figmaFrameEntity.transitionDuration,
						TransitionEasing = figmaFrameEntity.transitionEasing,
						TransitionNodeID = figmaFrameEntity.transitionNodeID,
					};
				}
			}
			if (btn == null)
				btn = new Button();
			return btn;
		}

	}
}
