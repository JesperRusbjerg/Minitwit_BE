# Feature-mapping

## Considerations:
### Initial thoughts
The Python minitwit app contains very basic functionality: Create a user, Create/delete operations on twits, flag twits that should not be displayed. We set out to create a new application in a new tech stack which mirrors this functionality meanwhile being easily modifiable, scalable and ready for further development

### Tech stacks & Architecture
### Architecture:
As we are unaware of futrue requirements for the project, we decided to go with a low-coupling architecture and have created a REST-API for the backend in C# .NET and a seperate frontend written in Vue 

This will allow the backend to serve potential other applications if required and to scale seperatly if required. Had we known all the requirements the project is going to have, we could have thoughtout a potentially better solution.

In addition to this, we did not want to bind our frontends to C#, as we would like to explore some of the newer frontend technoligies such as Vue.

Backend:
Writtein in C# .NET and funtions as a REST-API, it is connected to a SQLite database which stores the twits.

Frontend:
Written in Vue and is decoupled from the backend

## Mapping of features:

### Displaying twits
Displaying twits will be available in our frontend and will be fetched from our backend

### Flagging twits
Flagging twits will be available in the frontend, when a twit is flagged a request is sent to our backend which then flags it in the database and it will no longer be displayed when fetching twits

### Fetching twits, Flagging twits, Logging in etc.
The following features are available through the backend which reflects all of the functionality within the python twit app:

**FollowerController:** \
GET list of follows by the user\
POST follow another user\
DELETE unfollow another user\
**TwitController:**\
POST add message\
GET all public msgs\
GET all private msgs\
PUT flag/unflag msg\
**UserController:**\
POST register a user\
GET login/authenticate user\

## Containarization
We have created a containerized environment for our backend by creating a DockerFile. In addition to this, we push our latest image builds to DockerHub in order for the image to be easily deployed from anywhere. At last we have created a docker-compose file in order to ease the proces of running the image and turning it into a container while exposing the correct port(s). Our natural next step will be to deploy the backend on a DO server, and dockerize our frontend's aswell.



