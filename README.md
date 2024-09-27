# Zypr FiddlePad

## What is it? 

FiddlePad is a simple scipting environment in C# intended for use by business/data analysts who wish to evaluate Zypr without having to interface directly with Zypr's APIs. 

## What is Zypr?

Zypr is a discrete-event simulator and optimizer for modeling the forward-looking resource requirments primarily of on-premise infrastructure platforms (i.e., hardware, software, power and space resources). You can read more at https://zypr.app.


## Console Apps

FiddlePad project is composed of console apps that provide more simplified interfaces for consuming Zypr's APIs.  

-   **HealthCheck** to check if Zypr is available 
-   **User** to register as a trial user and receive an api key to use Zypr
-	**Zypr Starter Models** variety of online sample models that may be used as input to a Zypr simulation   
-	**Simulations** commands to run and monitor simulations and search and delete completed simulations (referred to as scenarios)   

FiddlePad is the console app in which an analyst can write, organize, debug, and run scripts using static methods of the console apps.  Example scripts are provided in the Fiddler.cs file.

## FiddlePad Settings.json

Settings.json file must include the api key you received upon confirming your registration.  All other settings are optional.

## VS Code Extensions 

-	C# Dev Kit for Visual Studio Code - required
-	Visual NuGet (Package Manager)  - recommended
-	JSON Viewer (tree view of JSON) - optional
-	JSON Crack (graph viewer of JSON) - optional
-	vscode-icons - optional



