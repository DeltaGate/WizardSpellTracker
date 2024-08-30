using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WizardSpellTracker
{

    public class Damage_at_slot_level 
    {
        [JsonPropertyName("1")]
        public string l1 { get; set; }

        [JsonPropertyName("2")]
        public string l2 { get; set; }

        [JsonPropertyName("3")]
        public string l3 { get; set; }

        [JsonPropertyName("4")]
        public string l4 { get; set; }

        [JsonPropertyName("5")]
        public string l5 { get; set; }

        [JsonPropertyName("6")]
        public string l6 { get; set; }

        [JsonPropertyName("7")]
        public string l7 { get; set; }

        [JsonPropertyName("8")]
        public string l8 { get; set; }

        [JsonPropertyName("9")]
        public string l9 { get; set; }

        [JsonPropertyName("10")]
        public string l10 { get; set; }

    }




    public class Damage_Type  // subsubclass, same as below but down a level :)
    {
        public string name { get; set; }

    }




    public class Damage         // subclass accessed and used when required by main class
    {
        public Damage_Type damage_type { get; set; }

        public Damage_at_slot_level damage_at_slot_level { get; set; }
    }




    public class SpellResponse
    {
        // main obj properties
        public string name { get; set; }
        public string[] desc { get; set; }  //class object with sepcific json property linked name
        public string[] higher_level { get; set; }
        public string range { get; set; }
        public string[] components { get; set; }
        public string material { get; set; }
        public bool ritual { get; set; }
        public string duration { get; set; }
        public bool concentration { get; set; }
        public string casting_time { get; set; }
        public int level { get; set; }
        public Damage damage { get; set; } // create damage varible of class damage


        // converted array -> string propeties
        public string descAsString { get; set; }
        public string higher_levelAsString { get; set; }
        public string componentsAsString { get; set; }
        public string damage_at_slot_levelAsString { get; set; }


        }
    }
