using System.Collections;
using System.Linq;
using NUnit.Framework;


namespace Microsoft.Maui.Controls.Core.UnitTests
{
	using StackLayout = Microsoft.Maui.Controls.Compatibility.StackLayout;

	[TestFixture]
	public class TitleViewUnitTests : BaseTestFixture
	{
		[Test]
		public void BindingContextPropagatesFromParent()
		{
			NavigationPage navigationPage = new NavigationPage();
			var image1 = new Image();
			image1.SetBinding(Image.SourceProperty, "ImageSource");
			var page = new ContentPage()
			{
				Content = new Label()
			};

			var title = new Label() { Text = "Failed" };
			title.SetBinding(Label.TextProperty, "Title");

			var layout = new StackLayout()
			{
				Orientation = StackOrientation.Horizontal,
				Children =
					{
						title,
						image1
					}
			};

			page.SetValue(NavigationPage.TitleViewProperty, layout);
			navigationPage.PushAsync(page);

			var model = new Model();
			navigationPage.BindingContext = new Model();
			Assert.AreEqual(model.Title, title.Text);

			string success = "Success";
			page.BindingContext = new Model() { Title = success };
			Assert.AreEqual(success, title.Text);
			navigationPage.BindingContext = new Model() { Title = "Failed" };
			Assert.AreEqual(success, title.Text);
		}

		public class Model
		{
			public string Title { get; set; } = "Binding Working";
			public string ImageSource { get; } = "coffee.png";
		}
	}
}
