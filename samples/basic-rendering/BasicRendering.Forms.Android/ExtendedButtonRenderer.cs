﻿using System.ComponentModel;
using Android.Views;
using FigmaSharp.Forms;
using LiteForms.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;

[assembly: ExportRenderer(typeof(ExtendedButton), typeof(ExtendedButtonRenderer))]
namespace LiteForms.Forms
{
	[Obsolete ("This should be updated")]
	public class ExtendedButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer
	{
		public new ExtendedButton Element
		{
			get
			{
				return (ExtendedButton)base.Element;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				return;
			}

			SetTextAlignment();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == ExtendedButton.HorizontalTextAlignmentProperty.PropertyName)
			{
				SetTextAlignment();
			}
		}

		public void SetTextAlignment()
		{
			Control.Gravity = ToHorizontalGravityFlags(Element.HorizontalTextAlignment);
		}

		public GravityFlags ToHorizontalGravityFlags(Xamarin.Forms.TextAlignment alignment)
		{
			if (alignment == Xamarin.Forms.TextAlignment.Center)
				return GravityFlags.AxisSpecified;
			return alignment == Xamarin.Forms.TextAlignment.End ? GravityFlags.Right : GravityFlags.Left;
		}
	}
}
