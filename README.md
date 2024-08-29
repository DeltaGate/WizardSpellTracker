# WizardSpellTracker

Application: Wizard Spell Tracker

Software Function: To track spells & spell infomation for the table top game Dungeons & Dragons 5E

Technology Used: C#, SQLite, WPF.



Design Plan Requirements- 

Create a command line requesting spell and displaying spell infomation via api get request. - COMPLETE

Save infomation from command line to database. - COMPLETE

Create WPF GUI spell tale, add existing features to GUI.

Add limits to prepared spells.

Add limits based on player level.

Create readable page to access stored spell infomation.




Direct Challanges - 

No prior work done with Json responses in c#

No prior work done with GET / POST responses in c#




Log: 

Day 1 - 

Work on a commandline test bed began, test bed can be found at (https://github.com/DeltaGate/c-GetTest/tree/main/wizardTrackerBasic). Created a GET request to
a free third party D&D API at (https://www.dnd5eapi.co/) based on user input, finding spell infomation requested. API responds to GET request with a JSon Object.
Used System.Text.Json libary to manage Json data, "deserilizing" it into a specificlly created class to house expected data called "SpellResponse". Created
inital test Database to house partical responses during testing. transfer from object to Database was a success.

Day 2 -

Completed work on SpellResponse Object, Database & SQL insert statements. Confirmed functional with spell inputs for "Fireball", "Blur", "Acid-Arrow".
All data is now accounted for from the Json response into the local SpellResponse object called "arcaneKnoweldge", this then is transfered into the DB by
1 of 2 SQL statements, determined based on if a subclass of the SpellResponse Object is in use. Now moving onto WPF front end.








Debug Techniques - 

Break's in code via Visual Studio Debug.

Console / Trace Outputs.