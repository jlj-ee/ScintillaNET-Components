﻿#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives

namespace CodeEditor_Components
{
    /// <summary>
    /// Component to 
    /// </summary>
    public class Suggestions : ComponentManager
    {

        #region Fields

        private List<SuggestionItem> _items;
        private List<SuggestionItem> _visibleItems;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructs a new <see cref="Suggestions"/> instance with associated <see cref="SuggestionDropDown"/> instance.
        /// </summary>
        /// <param name="editor">>The <see cref="IEditor"/> editor to which the <see cref="Suggestions"/> is attached.</param>
        public Suggestions(IEditor editor) {
            MaxVisibleItems = 5;

            if (editor != null) {
                Editor = editor;

            }

            DropDown = CreateDropDownInstance();
            DropDown.List.SuggestionChosen += ListBox_Selected;
            MaximumWidth = 500;
        }

        /// <summary>
        /// Constructs a new <see cref="Suggestions"/> instance.
        /// </summary>
        public Suggestions() : this(null) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the associated <see cref="IEditor"/> control that <see cref="Suggestions"/> can act upon.
        /// </summary>
        public override IEditor Editor {
            get {
                return base.Editor;
            }
            set {
                base.Editor = value;
                Editor.LostFocus += Editor_LostFocus;
                Editor.MouseDown += Editor_MouseDown;
                Editor.KeyDown += Editor_KeyDown;
                Editor.Scroll += Editor_Scroll;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of items that will be displayed in the list.
        /// </summary>
        public int MaxVisibleItems { get; set; }

        /// <summary>
        /// Gets or sets the maximum width in pixels of the list.
        /// </summary>
        public int MaximumWidth {
            get {
                return DropDown.MaximumSize.Width;
            }
            set {
                DropDown.MaximumSize = new Size(value, DropDown.MaximumSize.Height);
            }
        }

        /// <summary>
        /// Gets or sets the color theme for the suggestion menu.
        /// </summary>
        public ListTheme Theme {
            get {
                return DropDown.Theme;
            }
            set {
                DropDown.Theme = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SuggestionDropDown"/>.
        /// </summary>
        public SuggestionDropDown DropDown { get; private set; }

        #endregion Properties

        #region Events & Handlers

        // Called when the editor loses focus. 
        private void Editor_LostFocus(object sender, EventArgs e) {
            if (!DropDown.Focused /*_dropDown.List.SelectedIndex != -1*/) {
                HideSuggestions();
        }
    }

        // Called when the mouse is clicked in the editor.
        private void Editor_MouseDown(object sender, MouseEventArgs e) {
            HideSuggestions();
        }

        // Called when the editor is scrolled.
        private void Editor_Scroll(object sender, ScrollEventArgs e) {
            HideSuggestions();
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e) {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.Space) {
                ShowSuggestions(true);
                e.SuppressKeyPress = true;
                return;
            }
        }

        // Called when an item is double-clicked in the listbox.
        private void ListBox_Selected(object sender, EventArgs e) {
            Editor.Target.Focus();
        }

        #endregion Events & Handlers

        #region Methods

        /// <summary>
        /// Displays the list of suggestions.
        /// </summary>
        public void ShowSuggestions(bool forceOpened) {
            Point point = Editor.GetPointFromPosition(Editor.CurrentPosition);
            point.Offset(0, Editor.LineHeight);
            DropDown.ShowSuggestionBox(point);
        }

        public void HideSuggestions() {
            DropDown.Close();
        }

        /// <summary>
        /// Sets the list of <see cref="SuggestionItem"/> objects that can be displayed if a matching query is found.
        /// </summary>
        /// <param name="suggestions"><see cref="List{Suggestion}"/> to be stored.</param>
        public void SetSuggestions(List<SuggestionItem> suggestions) {
            _items = suggestions;
            DropDown.AddItems(suggestions);
        }

        /// <summary>
        /// Creates and returns a new <see cref="SuggestionDropDown"/> object.
        /// </summary>
        /// <returns>A new <see cref="SuggestionDropDown"/> object.</returns>
        private SuggestionDropDown CreateDropDownInstance() {
            return new SuggestionDropDown(this);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                DropDown?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Methods
    }

    //public class SelectingEventArgs : EventArgs
    //{
    //    public Suggestion Selection { get; internal set; }
    //    public bool Cancel { get; set; }
    //    public int SelectionIndex { get; set; }
    //    public bool Handled { get; set; }
    //}

    //public class SelectedEventArgs : EventArgs
    //{
    //    public Suggestion Selection { get; internal set; }
    //    public Control Control { get; set; }
    //}

    //public class HoveredEventArgs : EventArgs
    //{
    //    public Suggestion Item { get; internal set; }
    //}


    //public class PaintItemEventArgs : PaintEventArgs
    //{
    //    public RectangleF TextRect { get; internal set; }
    //    public StringFormat Format { get; internal set; }
    //    public Font Font { get; internal set; }
    //    public bool IsSelected { get; internal set; }
    //    public bool IsHovered { get; internal set; }
    //    public Theme ThemeColors { get; internal set; }

    //    public PaintItemEventArgs(Graphics graphics, Rectangle clipRect) : base(graphics, clipRect) {
    //    }
    //}

}
