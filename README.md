# WizardSpellTracker

Application: Wizard Spell Tracker

Software Function: To track spells & spell infomation for the table top game Dungeons & Dragons 5E

Technology Used: C#, SQLite, WPF, JSON.



Design Plan / Requirements (at basic level)- 

Create a command line requesting spell and displaying spell infomation via api get request. - COMPLETE

Save infomation from command line to database. - COMPLETE

Create WPF GUI spell table, add existing features to GUI. - COMPLETE

Add limits to prepared spells. - COMPLETE

Add limits based on player level. - COMPLETE

Create readable page to access stored spell infomation. - COMPLETE




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
1 of 2 SQL statements, determined based on if a subclass of the SpellResponse Object is in use. Begin work on WPF front end, testbed functionality now avalible
through GUI. User can now search for spells not know as well as update which Database table a spell should be in via graphical tables.

Day 3 - 

Updated DB location to local files and added to GIT repo. Added check for error responses from GET request so the application no longer crashes out due to
invalid spells being requested, insted an error box will appear informing the user this spell is not found. Found an issue when testing spells with unhandled Null 
recieved, added a 3rd SQL insert varient to cover case. created a second window to display spell infomation, initally attempted a few implumentations for this but landed
on using a list view for now, this will be something to improve in future. Added player level selector and locked prepared spell list from taking more spells based on
current player level. Added database checks to see if spells already exist.








Debug Techniques - 

Break's in code via Visual Studio Debug.

Console / Trace Outputs.



Improvments for future:

Spell infomation needs to be displayed in a better format. will need to look into what other ways WPF offers to display data.
Add spell slots based on game rules. Will need to improve database to accomidate. 
Change level allowed slots to match game rules, its currently just based on player level which isn't exactly right.#

Known Issues:
The database check for already known spells fails to account for strings with capitilsed second words.



If you want to run this yourself from my repo, you'll need to make sure that you assign your "dbLocation" to whatever folder you've downloaded the repo to, "dbLocation" 
can be found in the DBControl class at the top. 