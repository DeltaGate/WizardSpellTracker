using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Windows.Controls;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Reflection.PortableExecutable;
using System.Data.Common;

namespace WizardSpellTracker
{
    class DBControl
    {
        private static string dbLocation = "data source=C:\\Users\\Connor\\Desktop\\coding 2024\\c# interview work\\WizardSpellTracker\\Spells.db"; //db location;

        private SqliteConnection connection;






        public void connectToDb()
        {
            // connect to database
            connection = new SqliteConnection(dbLocation); // create object with location of db
            connection.Open(); //open connection
        }





        public void loadSpellsFromDB(ListBox unpreparedSpells, ListBox preparedSpells)
        { //method to read database and push spell list names to UI
            

            unpreparedSpells.Items.Clear();
            preparedSpells.Items.Clear();  
            //clear list
            
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

            readDB = new SqliteCommand("SELECT name FROM prepared", connection);

            reader = readDB.ExecuteReader();
            {
                while (reader.Read())
                {
                    string name = reader.GetString(0);
                    Trace.WriteLine(name);

                    preparedSpells.Items.Add(name);

                }
            }

        }



        public void moveSpellToPrepared(string name)
        {
            var moveToPrepared = new SqliteCommand("INSERT INTO prepared SELECT * FROM unprepared WHERE name = " + "'" + name + "'" + ";", connection);
            moveToPrepared.ExecuteNonQuery();
            var deleteCurrent = new SqliteCommand("DELETE FROM unprepared WHERE name =" + "'" + name + "'" + ";", connection);
            deleteCurrent.ExecuteNonQuery();
        }


        public void moveSpellToUnprepared(string name)
        {
            var moveToUnprepared = new SqliteCommand("INSERT INTO unprepared SELECT * FROM prepared WHERE name = " + "'" + name + "'" + ";", connection);
            moveToUnprepared.ExecuteNonQuery();
            var deleteCurrent = new SqliteCommand("DELETE FROM prepared WHERE name =" + "'" + name + "'" + ";", connection);
            deleteCurrent.ExecuteNonQuery();
        }






        public void readDBSpell(string name, ListView housing, string preped) //query unprepared db for spell info
        {
            var readDB = new SqliteCommand("SELECT * FROM " + preped + " WHERE name = '" + name + "';", connection);

            var reader = readDB.ExecuteReader();

            while (reader.Read())  // read requsted db entry
            {
                for (int i = 0; i < reader.FieldCount; i++)  //loop for length of field count
                {
                    if (reader.IsDBNull(i)) { continue; } //ignore null fields
                    else {
                        string columnName = reader.GetName(i);
                        string fieldData = reader.GetString(i);
                        Trace.WriteLine(columnName);
                        Trace.WriteLine(fieldData);
                        var item = new ListViewItem();

                        var stackPanel = new StackPanel {Orientation = Orientation.Horizontal };
                        stackPanel.Children.Add(new TextBlock { Text = columnName, Width = 100});
                        stackPanel.Children.Add(new TextBlock { Text=fieldData});

                        item.Content = stackPanel;

                        housing.Items.Add(item);       //wacky but it works, Need to investigate best ways to display data in wpf and rebuild


                    }

                }


            }
        }



        public bool alreadyKnownSpell(string scrySpell)
        {
            //catch already known spells
            var checkExists = new SqliteCommand("SELECT name FROM unprepared WHERE name = '" + char.ToUpper(scrySpell[0]) + scrySpell.Substring(1) + "' ;", connection);
            var checkReader = checkExists.ExecuteReader();


            while (checkReader.Read())
            {
                Trace.WriteLine("trace3");

                string test = checkReader.GetString(0);
                Trace.WriteLine(test);
                Trace.WriteLine(scrySpell);

                if (test.ToLower() == scrySpell.ToLower()) { MessageBox.Show("You already know that spell young wizard"); return false; }
            }

            checkExists = new SqliteCommand("SELECT name FROM prepared WHERE name = '" + char.ToUpper(scrySpell[0]) + scrySpell.Substring(1) + "' ;", connection);
            checkReader = checkExists.ExecuteReader();

            while (checkReader.Read())
            {
                Trace.WriteLine("trace3");

                string test = checkReader.GetString(0);
                Trace.WriteLine(test);
                Trace.WriteLine(scrySpell);

                if (test.ToLower() == scrySpell.ToLower()) { MessageBox.Show("You already know that spell young wizard"); return false; }
            }

            return true;
            //catch already known spells end - this needs to be refactored at a later date.
        }




        public void apiGet(string scrySpell)
        {
            using (var client = new HttpClient())
            //creates an HttpClient Object for contacting web end points. creates with using so when done the obj is dropped and connection is closed
            {

                var endPoint = new Uri("https://www.dnd5eapi.co/api/spells/" + scrySpell); //sets 


                HttpResponseMessage response = client.GetAsync(endPoint).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Trace.WriteLine(response.StatusCode.ToString());
                    MessageBox.Show("Unable to find requested spell young wizard, try again.");
                    return;
                }
                // checking API response to user request

                var result = client.GetAsync(endPoint).Result.Content;
                //telling httpClient obj to send a get call to spell page, and send back content as a readable string, the .result tells it to wait for the result as by default async is all at once so won't wait otherwise.


                string held = result.ReadAsStringAsync().Result;


                SpellResponse arcaneKnowledge = JsonSerializer.Deserialize<SpellResponse>(held); //deserialise or extract json data by catagory to object

                //Sanitise Responses
                arcaneKnowledge.descAsString = string.Join(" ", arcaneKnowledge.desc);
                arcaneKnowledge.descAsString = arcaneKnowledge.descAsString.Replace("'", "");

                arcaneKnowledge.higher_levelAsString = string.Join(" ", arcaneKnowledge.higher_level);
                arcaneKnowledge.higher_levelAsString = arcaneKnowledge.higher_levelAsString.Replace("'", "");

                arcaneKnowledge.componentsAsString = string.Join(" ", arcaneKnowledge.components);
                arcaneKnowledge.componentsAsString = arcaneKnowledge.componentsAsString.Replace("'", "");

                if (arcaneKnowledge.material != null) { arcaneKnowledge.material = arcaneKnowledge.material.Replace("'", ""); }
                //end sanitise


                //3 SQL insert querys for use dependent on if parts of the arcaneKnowledge object are in use
                if (arcaneKnowledge.damage == null)
                {
                    var addToDB = new SqliteCommand("INSERT INTO unprepared (name, desc, higher_level, range, components, material, ritual, duration, concentration, casting_time, level) VALUES('" + arcaneKnowledge.name + "', '" + arcaneKnowledge.descAsString + "', '" + arcaneKnowledge.higher_levelAsString + "', '" + arcaneKnowledge.range + "', '" + arcaneKnowledge.componentsAsString + "', '" + arcaneKnowledge.material + "', '" + arcaneKnowledge.ritual + "', '" + arcaneKnowledge.duration + "', '" + arcaneKnowledge.concentration + "', '" + arcaneKnowledge.casting_time + "', '" + arcaneKnowledge.level + "');", connection);
                    addToDB.ExecuteNonQuery();
                }
                else if (arcaneKnowledge.damage.damage_at_slot_level == null)
                {
                    var addToDB = new SqliteCommand("INSERT INTO unprepared (name, desc, higher_level, range, components, material, ritual, duration, concentration, casting_time, level, attack_type) VALUES('" + arcaneKnowledge.name + "', '" + arcaneKnowledge.descAsString + "', '" + arcaneKnowledge.higher_levelAsString + "', '" + arcaneKnowledge.range + "', '" + arcaneKnowledge.componentsAsString + "', '" + arcaneKnowledge.material + "', '" + arcaneKnowledge.ritual + "', '" + arcaneKnowledge.duration + "', '" + arcaneKnowledge.concentration + "', '" + arcaneKnowledge.casting_time + "', '" + arcaneKnowledge.level + "', '" + arcaneKnowledge.damage.damage_type.name + "');", connection);

                    addToDB.ExecuteNonQuery();

                }
                else { 
                    var addToDB = new SqliteCommand("INSERT INTO unprepared (name, desc, higher_level, range, components, material, ritual, duration, concentration, casting_time, level, attack_type, l1, l2, l3, l4, l5, l6, l7, l8, l9, l10) VALUES('" + arcaneKnowledge.name + "', '" + arcaneKnowledge.descAsString + "', '" + arcaneKnowledge.higher_levelAsString + "', '" + arcaneKnowledge.range + "', '" + arcaneKnowledge.componentsAsString + "', '" + arcaneKnowledge.material + "', '" + arcaneKnowledge.ritual + "', '" + arcaneKnowledge.duration + "', '" + arcaneKnowledge.concentration + "', '" + arcaneKnowledge.casting_time + "', '" + arcaneKnowledge.level + "', '" + arcaneKnowledge.damage.damage_type.name + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l1 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l2 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l3 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l4 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l5 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l6 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l7 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l8 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l9 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l10 + "');", connection);

                    addToDB.ExecuteNonQuery();
                }
            }


        }


    }
}
