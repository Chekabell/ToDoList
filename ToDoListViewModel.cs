using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ToDoList;

public class ToDoListViewModel : INotifyPropertyChanged
{
    private string _newText;

    public ObservableCollection<ToDoItemViewModel> ItemList { get; }

    public string NewText
    {
        get => _newText;
        set
        {
            if(_newText == value) return;
            _newText = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand EditItemCommand { get; }

    public ToDoListViewModel()
    {
        ItemList = LoadToDoList();

        AddItemCommand = new Command(() =>
        {
            if (string.IsNullOrWhiteSpace(NewText))
                return;
            ItemList.Add(new ToDoItemViewModel(NewText));
            SaveToDoList();
        });
        
        DeleteItemCommand = new Command<ToDoItemViewModel>(item =>
        {
            if(item == null) return;
            ItemList.Remove(item);
            SaveToDoList();
        });

        EditItemCommand = new Command<ToDoItemViewModel>(async item =>
        {
            if (item == null || item.IsComplete)
                return;

            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Edit Todo",
                "Enter new text:",
                initialValue: item.Text);

            if (!string.IsNullOrWhiteSpace(result))
            {
                item.Text = result;
                SaveToDoList();
            }
        });
    }

    private ObservableCollection<ToDoItemViewModel> LoadToDoList()
    {
        var toDoListJson = Preferences.Get("ToDoListItems", "");

        if (toDoListJson == "")
            return new ObservableCollection<ToDoItemViewModel>();
        
        return JsonConvert.DeserializeObject<ObservableCollection<ToDoItemViewModel>>(toDoListJson) ??
                  new ObservableCollection<ToDoItemViewModel>();
    }

    public void SaveToDoList()
    {
        var json = JsonConvert.SerializeObject(ItemList);
        Preferences.Set("ToDoListItems", json);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}