# osu! Multi Analyser
#### Video Demo:  <URL HERE>
#### Description:
osu! Multi Analyser (shorthand OMA) is a project I made to get stats from tournament lobbies in the game osu.
It will work on any lobby as long as it is played in the team vs. gamemode, however.
This tool allows you quickly get stats from the lobby, including each participant's:
Average Score, Average Accuracy, Team Match Cost and Game Match Cost.
It will also display who had the highest Avg. Score and Accuracy.

The match cost is using a simple formula to get the slightly weighted averages in a standardised way for each player:

cost = (2 / (n(t) + 2)) * sum(s(i) / m(i)
where n(t) is the number of games played by the player
where s(i) is the score for player on game i
where m(i) is the average score for the game by all players (or just thier team).

(https://imgur.com/BJPOKDY)

This lets you quickly see how players performed in the game, and gives you a nice little overview of the lobby.
It also shows you each of the beatmaps played, along with the links to the mapset if you wish to download it.

I used C# because it is a language I have come to enjoy, and I do believe that it can really do anything you want it to.
Including slightly lower level stuff with it's interop and unsafe capabiliy, and with how many bindings
it has to popular libraries written in C and the like.
There really isn't anything you can't do in the language outside of hardware and very low level stuff,
but in the modern day and for modern software, it's not something you would need everyday anyways.

It's very performant for a runtime-managed language, and honestly the syntax and ecosystem is just really nice
and easy to work with.
It's now a great cross-platform solution with many resources online, so what's not to like!

## Implementation

There are a few key parts that make this project work:

### HTTP Calls to the osu! multiplayer links to get data

This is done in the Lobby.cs file, which is the highest element in the object heirarchy.
As everything else comes under this, I used this object to carry out all the actions that take place on the lobby.
Here we have a static Parse function which returns a lobby should it be a valid id presented,
and all the Caulculation functions in order to get the stats that are desired.

ASP.net makes it easy with it's built in HTTP types to make these calls and handle the data in terms of web,
making our lives easier when it comes to making and sending requests, then handling the responses.

The HomeController.cs file is quite self explanatory, but it just hosts all the routes and relevant functions for
sending back the desired response, essentially the same way that we did it in the Finance project with a
different syntax.

A nice thing in this case though was dependancy injection, which lets you then pass through the services you
configure and easily interact with them, which I can definitely see the value of should you have a larger project
with multiple services and dependancies.

- For this to work as I needed it, I needed to make a way to call it multiple times with their query paramters.
  This is because it only shows a set amount by default, and expects you to click a 'show more' button to carry on.
  Thus, I made a function that calls it again with the required query, and appends all the information to the existing
  structure from the initial call.

### Storing getting, calculating and adding the relevant data to the objects:

This is mostly handled in the Lobby.cs and User.cs models, as they are what represent the data we are trying to get.
The lobby has list of users that played it, and so we operate the functions through this for every user, calling
their functions to get their specific stats.
I also addes some custom fields (properties) to the Game.cs model, to store some specific information about
each team and the game itself. This was mostly to allow the calculation of match cost to be done on the given values,
as opposed to calculating these every time, with every player (game average score & team specific ones).

Doing it in this way allows us to keep things separated into what they affect, and where the data is stored.
It's one of the benefits of object oriented programming, however I don't make too much use of the OO part of C#,
since I think it's a bit of a slippery slope when you get to over-using inheritance and such.

As I'm not exactly a master at it, I think going into those features would quickly get out of hand.

- From the multi lobby call, you mostly just get the raw data for the lobby in a JSON format,
  consisting of the events of the lobby, the games and the scores set.
  You then need to calculate what team won, how many points they got and so on.
  using this data, I could then get the individual player stats, which using the LINQ capabilities of C# made very easy.
  C# has a built in library for JSON serialisation and de-serialisation, so mapping the objects was not too hard to do.
- This information I would calculate would then be added to custom fields on the data objects so that it only has
  to be calculated once per lobby. I can then just reference these values that I have stored.

### Presenting the information:

This is handled in the Views folder, which contains the cshtml template files used in ASP to generate HTML. 
The Razor syntax is very easy to use, and essentially just allows you to write C# in your html files.
It works seamlessly and is checked at compile time also, making it easy to spot errors in your ways.

Although frontend is probably my least favorite part, I can't say that it wasn't a little fun to see all your changes
happening in front of your eyes, and which hot reloading (dotnet watch), it definitely made it easier to do.

- I decided to go with a web app, just as it would be more relevant in the current landscape of programming,
  and because I could then compare and see how it differed from how we did things in Flask for the finance project.
  The finance project also made me more curious as to how these HTTP server libraries work under the hood and
  appreciate them more for what they do and accomplish with real-time templating and serving of content.
- Using MVC made the most sense as the paradigm is very useful in splitting things out,
  and allows you to integrate different elements of the project very easily, without affecting the views.
- This means you can make a service and use dependency injection to have it change how the app works behind the scenes,
  while to the user and the view side, it all works the same.

I also made a command line version with the CliLobbyManager.cs, but wanted to make something that required a bit
more work and out of my comfort zone. 
You can use this one by creating a new CliLobbyManager in the program.cs file instead of the MVCLobbyManager.

### Authorisation:
This is handled in both the OMASimpleAuth.cs service, and OMAService.cs.
The OMASimpleAuth is the custom middleware I made to understand the pipeline, and how it can be used
to implement something like auth.
It essentially just checks if the user has the required 'hash' cookie, and if not, redirects them to the
login page before they can use the site.
This applies to every page, so only when they have a valid hash, can they enter.

The OMAService is the general handler for all things lobbies, and is static.
I did it this way, so that we can then use it to store everything for the duration of the program for all requests,
and essentially then replicates a database it fetches from without needing to implement that.
This also makes for easy testing and use, as all the data is removed at shutdown.
In regards to Auth, it is then just used to see if the hash key already exists in the dictionary, and if not, then
you do not have a valid hash.
Logging in then adds your hash to the dictionary, and gives you a list of lobbies to work with.
I used a non-cryptographic hash since nothing here needs to be private, and just wanted it to be more like
storage areas as opposed to accounts and security.
Someone could then give you an alias for a set of lobbies for example to see a whole single tournament.
To make that work better, I also implemented 'locking' aliases for editing.
This can't be reversed since everything is anonymous, but the idea being you create an archived list of lobbies for
others to be able to log into and see.
Though I did implement this at first before realising it diminishes the whole idea.

- Somewhat piggy-backing the sessions we used in the Flask project, I wanted to quickly write my own
  form of 'Sessions' just to have a better understanding of how Cookies work, and how you can use them
  in routing and access to the application.
- In reality, it's all alot simpler than it seems, and peeling that layer of 'magic' away really helped me
  understand just what is happening in web applications when it comes to this subject.
  My implementation is of course very naive, however I can now see how you would expand this using for example
  a database and cryptographic hashing, Cookies with expiration, perhaps multiple layers  and so on.
  In my case, just using a dictionary containing the users hash and their respective lobbies serves well for
  the purpose, so why over-engineer it.
  Of course, it's all runtime, but I think it's more important that I know HOW it would be persistent on app close,
  which I do - which is why I was not too concerned about the implementation.
- In my case, it's less about a secure account and more about storing lobbies under a specific user name, so it's
  all public access, just behind a certain hash / alias that you give to the login page.
  You can then change the login you use to see different lobbies that have been added.
- Rather than calculating and storing a different version of the same lobby for every alias,
  if it already exists in another user, you just get that same object added to your list instead.
  This helps with memory cost and performance slightly, since you aren't creating unnessecary objects or
  re-doing those calculations.

## Revelations:

I've come to realise that I much more enjoy the 'backend' of things rather than the 'frontend'.
For me, the most grueling part was designing the page, and having the CSS and html be right and so on.
It got really boring and felt more like a boilerplate of how to present the things, rather than really
doing something.
I really enjoyed doing the http request handling, handling the best of and warmup counts, coming up with ways
to solve the problems I was having in data capture and so on.
Up to the auth and having a more general understanding of just how things work.
I think I will endeavor to learn more about backend development, api's and lower-level concepts,
as they interest me greatly and I found great enjoyment in doing so in the small amount here and throughout CS50.

Before starting CS50, I thought C was this scary language that will blow up my computer if I tried it,
but now I've come to really enjoy using C, Zig, C++ in what they offer and the power they grant you.

CS50 has been invaluabe to me and has really made me realise just how much I enjoy the topic of computer science and
programming. It's become probably the main thing that I engage with in my own time now, and I'm very grateful.

Thank you for the great lectures and resources, I think it may have just changed my life!

I am inspired to look for employment in the programming world, and think my quality of life and enjoyment thereof will
be increased.
As much as I understand the Self-Taught route isn't the greatest in terms of 'Professional respect', I have a plan,
and resources in mind to help me not just know: web, app, backend, frontend, C#, Java, Python or whatever it is -
but know computer science as a field and coding as a tool to achieve the things it allows us to. 

Thus I'll be making sure I go through all the nitty gritty (and probably even not quite enjoyabe) subjects,
and truly understand what it all means and how it works.

Eventually I would like to do some graphics programming using OpenGL or Vulkan, knowing how the GPU works and such.
Super interested and can't wait to go into it.
