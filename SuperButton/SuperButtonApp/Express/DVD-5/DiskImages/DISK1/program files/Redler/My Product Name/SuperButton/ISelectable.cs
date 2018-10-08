using System.Windows.Input;

namespace SuperButton
{
    public interface ISelectable
    {
        ICommand SelectCommand { get; set; }
    }
}
