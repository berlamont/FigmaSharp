using FigmaSharp;
using FigmaSharp.Models;
using FigmaSharp.Services;
using LiteForms;
using LiteForms.Forms;
using System.Linq;

namespace BasicRendering.Forms
{
	public class LoginButtonConverter : FigmaViewConverter
	{
		public override bool CanConvert(FigmaNode currentNode) =>
			currentNode.name == "LoginButton";

		public override IView ConvertTo(FigmaNode currentNode, ProcessedNode parent, FigmaRendererService rendererService)
		{
			var entry = new Xamarin.Forms.Button();

			if (currentNode is IFigmaNodeContainer nodeContainer)
			{
				var text = nodeContainer.children
					.OfType<FigmaText>()
					.FirstOrDefault ();

				if (text != null)
					entry.Text = text.characters;


				var backgroundColor = nodeContainer.children.OfType<RectangleVector>().FirstOrDefault();
				if (backgroundColor != null && backgroundColor.HasFills)
					entry.BackgroundColor = backgroundColor.fills[0].color.ToFormsColor();
			}

			entry.TextColor = Xamarin.Forms.Color.White;
			var view = new View(entry);
			return view;
		}
		public override bool ScanChildren(FigmaNode currentNode) => false;
		public override string ConvertToCode(FigmaNode currentNode, FigmaCodeRendererService rendererService) => string.Empty;
	}
}
