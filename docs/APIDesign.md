


## API Routes Design 
/api - root of the app for all data access calls


|Resource|GET (read)|POST (create)|PUT(update)|DELETE (remove)|Details|
|---|---|---|---|---|---|
|**/clubs** | gets all clubs | creates a new club| updates a club |deletes a club| |
|**/clubs/{id}** | gets all clubs | creates|updates|deletes| | 
|**/clubs/{id}/courses** | gets all courses for club by id  | creates|updates|deletes| | 
|**/courses** | gets all courses | creates|updates|deletes|  |
|**/courses/{id}**  | gets a course by id| crates|updates|deletes| Should contain a link back to parent club|
|**/courses/{id}/rounds**  | gets rounds for a course by id| ||| Should contain a link back to parent club, should contain links to players by id|
|**/courses/{id}/tees** | gets the Tees for a given course | creates a new Tee |||Only gets and posts are allowed. Since Tee information changes infrequently, Tee data is denormalized into the Course object when stored. |
|**/courses/{id}/tees/{id}** | gets the Tee by Id | nothing ||Deletes a Tee by Id|Only gets and deletes are allowed |

|**/players** | gets | creates|updates|deletes| |
|**/players/{id}** | gets a player by id | creates|updates|deletes| |
|**/players/{id}/rounds** | gets | creates|updates|deletes| |

## Open API Support

One of the goals of the project is to learn more and go a bit deeper on Open API.  For example, the project should be consumable from the Blazor app, but should also be accessible from Flow or Postman as well.

## Caching

Since this is a personal project, we'll layer in caching as we go.  I am curious to see how caching can be done with Blazor WebAssembly and a Web API.  

## Use OData Integration 

The BlazorGolf API should use the OData integration to enable filtering, sorting and selecting. 



/admin. - root of the admin functions that might be needed 
