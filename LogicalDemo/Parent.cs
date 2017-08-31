using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LogicalDemo
{
    public class Parent : Control
    {
        static Parent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Parent), new FrameworkPropertyMetadata(typeof(Parent)));
        }

        private readonly List<Child> _children = new List<Child>();
        private Child _current;

        private ContentPresenter PART_ContentHost =>
            (ContentPresenter) Template.FindName(nameof(PART_ContentHost), this);

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_current != null)
                    yield return _current;
            }
        }

        public void AddChild(Child child)
        {
            _children.Add(child);
            AddLogicalChild(child);
            if (_current == null)
            {
                SwitchCurrentChild();
            }
        }

        public void RemoveChild(Child child)
        {
            if (Equals(child, _current))
            {
                RemoveCurrentChild();
            }
            else if (_current != null && _children.Count > 1)
            {
                _children.Remove(child);
                RemoveLogicalChild(child);
            }
        }

        public void SwitchCurrentChild()
        {
            if (!_children.Any())
                return;
            var index = _current == null ? 0 : _children.IndexOf(_current);
            index = (index + 1) % _children.Count;
            _current = _children[index];
            PART_ContentHost.Content = _current;
        }

        public void RemoveCurrentChild()
        {
            if (_current != null && _children.Count > 1)
            {
                var toRemove = _current;
                SwitchCurrentChild();
                RemoveChild(toRemove);
            }
        }
    }
}
