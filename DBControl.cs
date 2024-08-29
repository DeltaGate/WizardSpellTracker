using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Windows.Controls;

namespace WizardSpellTracker
{
    class DBControl
    {
        private static string dbLocation = "Data Source=C:\\Users\\Connor\\Desktop\\coding 2024\\c# interview work\\wizardTrackerBasic\\Spells - Copy.db"; //db location;

        private SqliteConnection connection; 






        public void connectToDb()
            {
            // connect to database
            connection = new SqliteConnection(dbLocation); // create object with location of db
            connection.Open(); //open connection
        }





        public void loadSpellsFromDB(ListBox unpreparedSpells) { //method to read database and push spell list names to UI
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            var readDB = new SqliteCommand("SELECT name FROM unprepared", connection);

            var reader = readDB.ExecuteReader();
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    Trace.WriteLine(name);

                    unpreparedSpells.Items.Add(name);

                }
            }

        }



        public void loadSpellPrepared(string name)
        {
            var moveToPrepared = new SqliteCommand("INSERT INTO prepared SELECT * FROM unprepared WHERE name = " + "'" + name + "'" + ";", connection);
            moveToPrepared.ExecuteNonQuery();
            var deleteCurrent = new SqliteCommand("DELETE FROM unprepared WHERE name =" + "'" + name + "'" + ";", connection);
            deleteCurrent.ExecuteNonQuery();
        }


        public void loadSpellUnprepared(string name)
        {
            var moveToUnprepared = new SqliteCommand("INSERT INTO unprepared SELECT * FROM prepared WHERE name = " + "'" + name + "'" + ";", connection);
            moveToUnprepared.ExecuteNonQuery();
            var deleteCurrent = new SqliteCommand("DELETE FROM prepared WHERE name =" + "'" + name + "'" + ";", connection);
            deleteCurrent.ExecuteNonQuery();
        }



    }



}
