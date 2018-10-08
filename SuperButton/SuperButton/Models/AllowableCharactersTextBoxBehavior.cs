using SuperButton.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace SuperButton.Models
{
    class AllowableCharactersTextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }
        void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null) e.Handled = !IsValid(e.Text, false, txt.Text);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var txt = sender as TextBox;
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));

                if (txt != null && !IsValid(text, true, txt.Text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsValid(string newText, bool paste, string currentvalue)
        {
            if (newText != "\u001b")
            {
                int integ;
                if (currentvalue == "" && newText == "-")
                    return true;
                if (currentvalue.Contains('.'))
                {
                    if (Int32.TryParse(newText, out integ))
                        return true;
                    return false;
                }
                if (currentvalue.Contains('-') && newText != "-")
                {
                    return true;
                }
                if (Int32.TryParse(newText, out integ))
                    return true;

                if (newText == "." && currentvalue != "")
                    return true;
                return false;
            }
            else
            {
                currentvalue = "";
                return false;
            }
        }
    }
}
