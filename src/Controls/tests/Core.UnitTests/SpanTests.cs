using Microsoft.Maui.Graphics;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	[TestFixture]
	public class SpanTests : BaseTestFixture
	{
		[Test]
		public void StyleApplied()
		{
			var pinkStyle = new Style(typeof(Span))
			{
				Setters = {
					new Setter { Property = Span.TextColorProperty, Value = Colors.Pink },
				},
				Class = "pink",
				ApplyToDerivedTypes = true,
			};

			var span = new Span
			{
				Style = pinkStyle
			};

			var formattedText = new FormattedString();
			formattedText.Spans.Add(span);

			var label = new Label()
			{
				FormattedText = formattedText
			};

			new ContentView
			{
				Resources = new ResourceDictionary { pinkStyle },
				Content = label
			};

			Assert.AreEqual(Colors.Pink, span.TextColor);
		}

		[Test]
		public void BindingApplied()
		{
			var vm = new ViewModel()
			{
				Text = "CheckBindingWorked"
			};

			var formattedText = new FormattedString();

			var label = new Label()
			{
				FormattedText = formattedText
			};

			var span = new Span();
			span.SetBinding(Span.TextProperty, "Text");

			formattedText.Spans.Add(span);

			label.BindingContext = vm;

			Assert.AreEqual(vm.Text, span.Text);
		}

		class ViewModel
		{
			public string Text { get; set; }
		}
	}
}
