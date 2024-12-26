using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDoList;

public class ToDoItemViewModel(string newText, bool newState = false) : INotifyPropertyChanged
{
    private string _text = newText;
    private bool _isComplete = newState;

    public String Text
    {
        get => _text;
        set
        {
            if(_text == value) return;
            _text = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            if(_isComplete == value) return;
            _isComplete = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}