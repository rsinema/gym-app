# gym-app

Group project repo for CS 428

# Setting up the repo

After cloning the repo make sure to run this cmd:

```
dotnet restore
```

# Running the application

## Run the app using Docker:

Make sure Docker Desktop is installed or the Docker CLI, and that the Docker daemon is running.
Run these cmds:

```
cd GymApp
docker build -t gym-app .
docker run -p 8080:80 gym-app
```

This will build the docker image and start it in a container. The web app will be published on localhost:8080.  
If you want/need to use a different local port just change the `docker run` cmd to `<your-port>:80` instead of `8080:80`.

## Run the app using dotnet:

Install the dotnet8 sdk and to run a live server with hot reload enabled run this cmd:

```
cd GymApp
dotnet watch
```

This will open a web page with the server rendered application. Any changes that are made will be updated live.
