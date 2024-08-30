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





        public void apiGet(string scrySpell)
        {
            using (var client = new HttpClient())
            //creates an HttpClient Object for contacting web end points. creates with using so when done the obj is dropped and connection is closed
            {

                var endPoint = new Uri("https://www.dnd5eapi.co/api/spells/" + scrySpell); //sets 

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



                if (arcaneKnowledge.damage == null)
                {
                    var addToDB = new SqliteCommand("INSERT INTO unprepared (name, desc, higher_level, range, components, material, ritual, duration, concentration, casting_time, level) VALUES('" + arcaneKnowledge.name + "', '" + arcaneKnowledge.descAsString + "', '" + arcaneKnowledge.higher_levelAsString + "', '" + arcaneKnowledge.range + "', '" + arcaneKnowledge.componentsAsString + "', '" + arcaneKnowledge.material + "', '" + arcaneKnowledge.ritual + "', '" + arcaneKnowledge.duration + "', '" + arcaneKnowledge.concentration + "', '" + arcaneKnowledge.casting_time + "', '" + arcaneKnowledge.level + "');", connection);
                    addToDB.ExecuteNonQuery();
                }
                else
                {
                    var addToDB = new SqliteCommand("INSERT INTO unprepared (name, desc, higher_level, range, components, material, ritual, duration, concentration, casting_time, level, attack_type, l1, l2, l3, l4, l5, l6, l7, l8, l9, l10) VALUES('" + arcaneKnowledge.name + "', '" + arcaneKnowledge.descAsString + "', '" + arcaneKnowledge.higher_levelAsString + "', '" + arcaneKnowledge.range + "', '" + arcaneKnowledge.componentsAsString + "', '" + arcaneKnowledge.material + "', '" + arcaneKnowledge.ritual + "', '" + arcaneKnowledge.duration + "', '" + arcaneKnowledge.concentration + "', '" + arcaneKnowledge.casting_time + "', '" + arcaneKnowledge.level + "', '" + arcaneKnowledge.damage.damage_type.name + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l1 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l2 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l3 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l4 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l5 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l6 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l7 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l8 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l9 + "', '" + arcaneKnowledge.damage.damage_at_slot_level.l10 + "');", connection);

                    addToDB.ExecuteNonQuery();
                }
            }


        }


    }
}
