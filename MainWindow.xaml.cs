using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace WizardSpellTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBControl dbControl = new DBControl(); //create DB control obj for constant use.
       
        public MainWindow()
        {
            InitializeComponent();

            dbControl.connectToDb(); // Inital Connection to DB

            dbControl.loadSpellsFromDB(unpreparedSpells); // Inital Load of spell names

            Trace.WriteLine("testing123");
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void prepareBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = unpreparedSpells.SelectedItem as string;
            preparedSpells.Items.Add(unpreparedSpells.SelectedItem);
            dbControl.loadSpellPrepared(name);
            unpreparedSpells.Items.Remove(unpreparedSpells.SelectedItem);

        }

        private void forgetBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = preparedSpells.SelectedItem as string;
            unpreparedSpells.Items.Add(preparedSpells.SelectedItem);
            dbControl.loadSpellUnprepared(name);
            preparedSpells.Items.Remove(preparedSpells.SelectedItem);
        }
    }
}