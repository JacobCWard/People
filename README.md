People app
==================

Sample address book. Manage contact information and define relations between **people** and **organizations**.

## How to run

1. Make sure you Launcher is up and running.
2. Build the solution
3. Start `People.exe` 

   ```bash
   star bin/Debug/People.exe 
   ```
   You can use `run.bat`, or Visual Studio as well.
4. Go to [http://localhost:8080/](http://localhost:8080/)

## Developer instructions

### How to release a package

This repo comes with a tool that automatically increments the information `package.config` (version number, version date, Starcounter dependency version) and creates a ZIP file containing the packaged app. To use it follow the below steps:

1. Make sure you have installed [Node.js](https://nodejs.org/)
2. Run `npm install` to install the tool (installs grunt, grunt-replace, grunt-bump, grunt-shell)
2. Run `grunt package` to generate a packaged version, (you can use `:minor`, `:major`, etc. as [grunt-bump](https://github.com/vojtajina/grunt-bump) does)
4. The package is created in `packages/<AppName>.zip`. Upload it to the Starcounter App Store