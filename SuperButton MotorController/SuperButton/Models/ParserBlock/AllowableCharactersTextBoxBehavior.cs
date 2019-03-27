using SuperButton.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

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
            string Name = txt.DataContext.ToString();
            string selection = txt.SelectedText;
            if (txt.Background.ToString() == "#FFFF0000" || Name == "SuperButton.ViewModels.NumericTextboxModel") {
                if (txt != null) e.Handled = !IsValid(e.Text, false, txt.Text, selection == "" ? false : true);
            }
            else
            {
                e.Handled = true;
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var txt = sender as TextBox;
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text));
                string selection = txt.SelectedText;
                if (txt != null && !IsValid(text, true, txt.Text, selection == "" ? false : true))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsValid(string newText, bool paste, string currentvalue, bool TextSelected)
        {
            if (newText != "\u001b")
            {
                int integ;
                if (currentvalue == "" && newText == "-") // start a negative number
                    return true;
                if (newText == "." && currentvalue != "" && currentvalue != "-" && !currentvalue.Contains('.') && !TextSelected) // float number - adding dot
                    return true;
                if (newText == "-" && currentvalue != "" && TextSelected) // delete all selected text and start a negative number
                    return true;

                if (currentvalue.Contains('.') && newText != "-") // verify the number is valid (not a char)
                {
                    if (Int32.TryParse(newText, out integ))
                        return true;
                    return false;
                }
                if (currentvalue.Contains('-') && newText != "-") // verify the number is valid for negative value
                {
                    if (newText != ".")
                    {
                        if (Int32.TryParse(newText, out integ))
                            return true;
                        return false;
                    }
                    else if (TextSelected && newText != ".") return true;
                    else return false;
                }
                if (Int32.TryParse(newText, out integ))
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
