using Microsoft.Maui.Layouts;
using StackLayoutManager = Microsoft.Maui.Layouts.StackLayoutManager;

namespace SimpleSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }
	public abstract class CustomLayout : StackLayout
	{
		internal abstract Size LayoutArrangeChildren(Rect bounds);
		internal abstract Size LayoutMeasure(double widthConstraint, double heightConstraint);
	}


	internal class CustomLayoutManager : StackLayoutManager
	{
		private CustomLayout layout;
		internal CustomLayoutManager(CustomLayout layout)
			: base(layout)
		{
			this.layout = layout;
		}
		public override Size ArrangeChildren(Rect bounds)
		{
			return this.layout.LayoutArrangeChildren(bounds);
		}

		public override Size Measure(double widthConstraint, double heightConstraint)
		{
			var measuredSize = this.layout.LayoutMeasure(widthConstraint, heightConstraint);

			if (measuredSize.Height < 0)
			{
				measuredSize.Height = 0;
			}

			return measuredSize;
		}
	}

	public class CustomStackLayout : CustomLayout
	{
		public Label label;
		public CustomStackLayout()
		{
			label = new Label() { Text = "CustomLabel", Background = Colors.Yellow, HeightRequest = 100 };
			this.Loaded += CustomLayout_Loaded;
		}

		private void CustomLayout_Loaded(object? sender, EventArgs e)
		{
			this.Children.Add(label);
		}

		protected override ILayoutManager CreateLayoutManager()
		{
			return new CustomLayoutManager(this);
		}

		internal override Size LayoutArrangeChildren(Rect bounds)
		{
			(label as IView).Arrange(new Rect(bounds.X, bounds.Y, bounds.Width, 40));
			return new Size(bounds.Width, 40);

		}

		internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
		{
			(label as IView).Measure(widthConstraint, 40);

			return new Size(widthConstraint, heightConstraint);
		}
	}
}
