using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace LogicalDemo
{
    public class Child : Control
    {
        static Child()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Child), new FrameworkPropertyMetadata(typeof(Child)));
        }

        public Child()
        {
            _problemChild.Unloaded += (sender, args) =>
            {
                Debug.WriteLine("Content Unloaded");
            };
            Unloaded += (sender, args) =>
            {
                Debug.WriteLine("Self Unloaded");
            };
        }

        private readonly Border _problemChild = new Border();

        private ContentPresenter PART_ContentHost =>
            (ContentPresenter)Template.FindName(nameof(PART_ContentHost), this);

        protected override IEnumerator LogicalChildren
        {
            get { yield return _problemChild; }
        }

        public override void OnApplyTemplate()
        {
            AddLogicalChild(_problemChild);
            PART_ContentHost.Content = _problemChild;
            PART_ContentHost.Unloaded += (sender, args) =>
            {
                Debug.WriteLine("Host Unloaded");
            };
        }
    }
}
