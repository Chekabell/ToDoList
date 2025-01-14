﻿namespace ToDoList;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    private void OnCompleteChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox { BindingContext: ToDoItemViewModel })
        {
            if (BindingContext is ToDoListViewModel viewModel)
            {
                viewModel.SaveToDoList();
            }
        }
    }
}