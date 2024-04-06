using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using _04Zaporozhets.ViewModel;

namespace _04Zaporozhets.View
{

    public partial class TableView : Window
    {
        private AgeCalculatorView? ageCalculatorView;
        int index;

        public TableView()
        {
            InitializeComponent();
            DataContext = new TableViewModel(this);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            index = dataGrid.SelectedIndex;
            var selectedPerson = (Person)dataGrid.SelectedItem;

            if (ageCalculatorView != null)
            {
                ageCalculatorView.Close();
                ageCalculatorView = null;
            }

            ageCalculatorView = new AgeCalculatorView();
            ageCalculatorView.DataContext = selectedPerson;
            ageCalculatorView.Show();
            ageCalculatorView.ButtonPressed();
            ageCalculatorView.onSavePressed += save;
            ageCalculatorView.onCancelPressed += cancel;
        }
        private void save()
        {
            ageCalculatorView.ButtonPressed();
            List<Person> All = (List<Person>)userGrid.ItemsSource;
            All[index] = ageCalculatorView.ageCalculatorViewModel.GetPerson();
            userGrid.Items.Refresh();
            ageCalculatorView.Close();
            ageCalculatorView = null;

        }
        private void cancel()
        {
            ageCalculatorView.Close();
            ageCalculatorView = null;
        }

        private void addPersonButton_Click(object sender, RoutedEventArgs e)
        {
            if (ageCalculatorView != null)
            {
                ageCalculatorView.Close();
                ageCalculatorView = null;
            }

            ageCalculatorView = new AgeCalculatorView();
            ageCalculatorView.Show();
            ageCalculatorView.onCancelPressed += cancel;
            ageCalculatorView.onSavePressed += add;

        }

        private void add()
        {
            ageCalculatorView.ButtonPressed();

            List<Person> All = (List<Person>)userGrid.ItemsSource;

            All.Add(ageCalculatorView.ageCalculatorViewModel.GetPerson());

            userGrid.Items.Refresh();
            ageCalculatorView.Close();
            ageCalculatorView = null;
        }

        private void removePersonButton_Click(object sender, RoutedEventArgs e)
        {
            List<Person> All = (List<Person>)userGrid.ItemsSource;
            All.Remove(All[userGrid.SelectedIndex]);
            userGrid.Items.Refresh();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TableViewModel tableViewModel = new TableViewModel(this);

            tableViewModel.SaveDataToFile("data.json");
        }
    }
}