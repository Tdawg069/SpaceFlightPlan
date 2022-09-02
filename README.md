# SpaceFlightPlan
A console app that generates a flight plan for colonising the most amount of weighted planets within a fixed time frame. Planets are randomly generated.

The following assumptions/simplifications have been made due to time constraints and practicality:
- The decimal points in the coordinate system have been removed.
- The surface area of planets are integer values. Surface area required to colonize can thus be calculated as "integer division by 2, + 1".
- Only the physically colonised portion of surface area is counted in the report, not the whole surface area.
- Reduced maximum surface area from 100 million to 4 million, as anything above 4 million will take more than 24 hours to colonise. Thus many times there were no planets that could be colonized within 24 hours. This value can be changed in HabitablePlanet.cs.
- Colonization period increased from 24 hours to 48 hours for the same reason as above. This can be changed in Program.cs.
- Units of measurement are not displayed in most places.
- There is no exception handling for invalid input arguments or file formats. Please only use generated files and correct argument format (if any)
- The file format chosen for storing the universe was a JSON serialization of the list of SpaceObjects. This was done for simplicity.
- My understanding of the travel time concept is as follows:

Key:
M - Monster
H - Habitable Planet
I - Inhabitable Planet
C - Colonized Planet
Y - Your current position

M-Y--H
H is further away from you than M, therefore it will take 20min to travel there (as it is the second-closest). This will hold true irrespective of the direction they are in. In this case, they are on opposite ends.

I-M-Y----H
The same applies for Inhabitable planets. H is the 3rd closest planet, and thus 30min away. I is 20min and M is 10min.

I-M-Y--C-H
The same would apply for previously colonized planets. Like above, they would also add 10min travel time to H (thus H is now 40min away).

Once you move to a new planet, distances (and thus travel time) get recalculated.


## Usage

`Starship.exe -generate "universe_file_path"`
OR
`Starship.exe -plan "universe_file_path" "flight_plan_file_path"`
OR
`Starship.exe`

If any of the file paths are not passed in (or the program is run without arguments), the default file paths will be used:
project_root\Universe.txt and
project_root\FlightPlan.txt

`-generate` option triggers universe generation and saving to file only.
`-plan`     option triggers loading universe from file, creating flight plan and saving flight plan to file only.
No option runs both of these, but with default file paths only.