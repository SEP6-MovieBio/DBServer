# DBServer
Server dedicated to database
# API Documentation
Base path : https://moviebiodb.azurewebsites.net/

Actor
| Type | Path | Parameter | Returns |
|---|---|---|---|
| GET  |  Actor/rating?actorID={STRING} | Actor ID | Double |
| GET | Actor/top20Actors | N/A | [ { "id": 0, "name": "string", "rating": 0, "age": 0, "knownFor": "string", "picture": "string" } ] |
| GET | Actor/getActor?id={INTEGER} | Actor ID | { "id": 0, "name": "string", "rating": 0, "age": 0, "knownFor": "string", "picture": "string" } |
| GET | Actor/searchTop10Actors?searchText={STRING} | search text | [ { "id": 0, "name": "string", "rating": 0, "age": 0, "knownFor": "string", "picture": "string" } ] |

Director
| Type | Path | Parameter | Returns |
|---|---|---|---|
| GET | Director/top20Directors | N/A | [ { "id": 0, "name": "string", "rating": 0, "age": 0, "topDirectedMovie": "string", "picture": "string" } ] |
| GET | Director/getDirector?id={INTEGER} | Director ID | { "id": 0, "name": "string", "rating": 0, "age": 0, "topDirectedMovie": "string", "picture": "string" } |
| GET | Director/searchTop10Directors?searchText={STRING} | search text | [ {  "id": 0, "name": "string", "rating": 0, "age": 0, "topDirectedMovie": "string",  "picture": "string" } ] |
| GET | Director/rating?directorID={STRING} | Director ID | Double |

MovieInfo
| Type | Path | Parameter | Body | Returns |
|---|---|---|---|---|
| GET | MovieInfo/top200Movies | N/A | N/A | [ { "movieId": 0, "movieTitle": "string", "Year": 0, "rating": 0, "votes": 0, "director": [ "string" ], "ratingBasedOnActors": 0,  "reviews": [ {   "ReviewID": 0, "MovieID": 0, "ReviewUsername": "string", "ReviewDescription": "string", "ReviewRating": 0 } ] } ] |
| GET | MovieInfo/searchTop10Movies?searchText={STRING} | Search text | N/A | [ { "movieId": 0, "movieTitle": "string", "Year": 0, "rating": 0, "votes": 0, "director": [ "string" ], "ratingBasedOnActors": 0, "reviews": [  {  "ReviewID": 0, "MovieID": 0, "ReviewUsername": "string", "ReviewDescription": "string", "ReviewRating": 0 } ] } ] |
| GET | MovieInfo/RandomChar?randChar={STRING} | Random character to search by | N/A | { "movieId": 0, "movieTitle": "string", "Year": 0,"rating": 0, "votes": 0, "director": [  "string" ], "ratingBasedOnActors": 0, "reviews": [ {  "ReviewID": 0,  "MovieID": 0,  "ReviewUsername": "string", "ReviewDescription": "string",    "ReviewRating": 0 } ] } |
| GET | MovieInfo/ratingByActor?movieid={STRING} | Movie ID | N/A | Double |
| GET | MovieInfo/MovieID?id={INTEGER} | Movie ID | N/A | { "ReviewID": 0, "MovieID": 0, "ReviewUsername": "string", "ReviewDescription": "string","ReviewRating": 0 } |
| POST | MovieInfo/MovieReview | Review object | {  "ReviewID": int, "MovieID": int, "ReviewUsername": "string", "ReviewDescription": "string", "ReviewRating": int } |  |
| PATCH | MovieInfo/UpdateMovieReview | Review object | {  "ReviewID": int, "MovieID": int, "ReviewUsername": "string", "ReviewDescription": "string", "ReviewRating": int } | { "ReviewID": 0, "MovieID": 0,  "ReviewUsername": "string", "ReviewDescription": "string",  "ReviewRating": 0 } |
| GET | MovieInfo/MovieRatingsByDecade | N/A | N/A | Json serialized C# dictionary |

User
| Type | Path | Parameter | Body | Returns |
|---|---|---|---|---|
| GET | User/favoriteMovies?username={STRING} | username for user | N/A | [ { "movieId": 0, "movieTitle": "string", "Year": 0, "rating": 0, "votes": 0, "director": [ "string" ], "ratingBasedOnActors": 0, "reviews": [  {  "ReviewID": 0, "MovieID": 0, "ReviewUsername": "string", "ReviewDescription": "string", "ReviewRating": 0 } ] } ] |
| GET | userInfo?username={STRING} | username for user | N/A | { "Username": "string", "Password": "string", "SecurityLevel": 0, "PhoneNumber": "string", "PhoneIsHidden": true, "Email": "string", "EmailIsHidden": true, "Biography": "string" } |
| GET | searchTop10Users?searchText={STRING} | Search text | N/A | [ { "Username": "string", "Password": "string", "SecurityLevel": 0, "PhoneNumber": "string", "PhoneIsHidden": true, "Email": "string", "EmailIsHidden": true, "Biography": "string" } ] |
| POST | User/postinfo | Userinfo object | { "PhoneNumber":"string","PhoneIsHidden": bool ,"Email":"string", "EmailIsHidden": false , "Biography":"string", "Username": "string", "Password": "string, "SecurityLevel": int} | BOOLEAN |
| POST | User/postHash | userinfo object |  { "PhoneNumber":"string","PhoneIsHidden": bool ,"Email":"string", "EmailIsHidden": false , "Biography":"string", "Username": "string", "Password": "string, "SecurityLevel": int} | { "Username": "string", "Password": "string", "SecurityLevel": 0, "PhoneNumber": "string", "PhoneIsHidden": true, "Email": "string", "EmailIsHidden": true, "Biography": "string"} |
| GET | User/Niceness?username={STRING} | username for user | N/A | Double |
| GET | User/LoginCheck?username={STRING}&password={STRING} | username and password for user | N/A | { "Username": "string", "Password": "string", "SecurityLevel": 0 } |
| POST | User/postUser | userinfo object | { "Username": "string", "Password": "string", "SecurityLevel": 0, "PhoneNumber": "string", "PhoneIsHidden": true, "Email": "string", "EmailIsHidden": true, "Biography": "string" } | { "Username": "string", "Password": "string", "SecurityLevel": 0, "PhoneNumber": "string", "PhoneIsHidden": true,  "Email": "string", "EmailIsHidden": true, "Biography": "string"} |
| POST | User/postFavouriteMovie?username={STRING}&movieID={INTEGER} | username for user and movie ID for movie to be added as favorite for user | N/A | STRING |
