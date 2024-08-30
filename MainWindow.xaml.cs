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
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WizardSpellTracker
{
    /// <summary>
    /// main login for front end controls.
    /// any Database related tasks are housed in DBControl
    /// any object prep for spell retrieval via 5e API is done via DBControl & SpellResponse OBJ
    /// </summary>
    public partial class MainWindow : Window
    {
        DBControl dbControl = new DBControl(); //create DB control obj for constant use.
       
        public MainWindow()
        {
            InitializeComponent();

            dbControl.connectToDb(); // Inital Connection to DB
            dbControl.loadSpellsFromDB(unpreparedSpells, preparedSpells); // Inital Load of spell names
        }




        private void prepareBtn_Click(object sender, RoutedEventArgs e)
        {
            int level = levelSelect.SelectedIndex + 1;
            
            if(preparedSpells.Items.Count > level) { 
                MessageBox.Show("Whooooo there, too many spells today young wizard, slow down"); 
                return; 
            }

            var name = unpreparedSpells.SelectedItem as string;
            preparedSpells.Items.Add(unpreparedSpells.SelectedItem);
            dbControl.moveSpellToPrepared(name);
            unpreparedSpells.Items.Remove(unpreparedSpells.SelectedItem);

        }




        private void forgetBtn_Click(object sender, RoutedEventArgs e)
        {
            var name = preparedSpells.SelectedItem as string;
            unpreparedSpells.Items.Add(preparedSpells.SelectedItem);
            dbControl.moveSpellToUnprepared(name);
            preparedSpells.Items.Remove(preparedSpells.SelectedItem);
        }




        private void learnSpellBtn_Click(object sender, RoutedEventArgs e)
        {
            string scrySpell = learnSpellTxtBox.Text;
            learnSpellTxtBox.Text = "";

            if (scrySpell == "" || scrySpell.StartsWith(" ")) //check for valid input
            {
                return;
            }

            scrySpell = scrySpell.Replace(" ", "-"); // correct formatting

            if (dbControl.alreadyKnownSpell(scrySpell)) 
            {
                dbControl.apiGet(scrySpell);
                dbControl.loadSpellsFromDB(unpreparedSpells, preparedSpells);
            }

        }



        //Display Spell Infomation
        private void unprepearedSpellInfoBtn_Click(object sender, RoutedEventArgs e) 
        {
            SpellDetails spellDetails = new SpellDetails(unpreparedSpells.SelectedItem as string);
            spellDetails.Show();
            dbControl.readDBSpell(unpreparedSpells.SelectedItem as string, spellDetails.SpellInfo, "unprepared");
        }




        private void preparedSpellInfoBtn_Click(object sender, RoutedEventArgs e) 
        {
            SpellDetails spellDetails = new SpellDetails(preparedSpells.SelectedItem as string);
            spellDetails.Show();
            dbControl.readDBSpell(preparedSpells.SelectedItem as string, spellDetails.SpellInfo, "prepared");
        }
    }
}