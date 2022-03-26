


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


#### Get all courses

```http
  GET /api/courses
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Required**. Your API key |


#### Get course

```http
  GET /api/courses/{id:guid}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `id` | `string (guid)` | **Required**. Course ID |


## Open API Support

One of the goals of the project is to learn more and go a bit deeper on Open API.  For example, the project should be consumable from the Blazor app, but should also be accessible from Flow or Postman as well.

## Caching

Since this is a personal project, we'll layer in caching as we go.  I am curious to see how caching can be done with Blazor WebAssembly and a Web API.  

## Use OData Integration 

The BlazorGolf API should use the OData integration to enable filtering, sorting and selecting. 



/admin. - root of the admin functions that might be needed 

## Web API annd Not Azure Functions

This was a choice early on based on my existing knowledge of Functions and wanting to learn more about Web API.


## MediatR : To use or not to use

Initally, I am building this without the overhead of MediatR, but even early on I am seeing that this may be a valuable addition. I will add an issue if this becomes something that might be valuable. 

## Links and References 

[Building a RESTful API with ASP.NET Core 3](https://app.pluralsight.com/library/courses/asp-dot-net-core-3-restful-api-building/table-of-contents)