This Repo is a compiled showcase of various code samples and snippets across my game dev career so far. 
I've tried to get a good variety of projects to show off, so don't expect to see any specialisation here! 
That being said, I've also avoided the temptation to go and touch-up the older projects. 
I've improved as a game developer significantly over the past year or two, and it feels cheap to pretend it was never a journey - especially working in a professional team, I would be even better about commenting and documentation, etc etc. 

I've packaged each project with the context needed to run it, but didn't want specific Unity, Unreal or Houdini versions and know-how to be a requirement to see my coding style;
Thus, I also included a "Quick Reference" folder that gets to the most interesting select files of each project if you want to skip digging through or running each version. 

Listed are the projects showcased, in descending chronological order:



Gloaming Gerstner Waves
Unity, C# & HLSL

An ocean shader built in Unity for my undergrad final year - got 78% total (First)
Despite being most known for implementing Gerstner Waves in HLSL, a fair amount of C# was needed to prop up the scene around the shader. Examples of both have been put in Quick Reference.



DigitisingMiniatures
Unreal 5, Blueprints

My Final Year Project, aiming to expose model-painting to the player in Unreal - got 83% total (First)
I mostly coded it in Unreal Blueprints, because I knew I'd have a lot to do and didn't want to risk scope reduction from C++ glitches, like those I had encountered in the project I'd worked on prior.
This is a room for expansion (in terms of coding practice, the actual project is satisfactory) - I used a little C++, but only for a handful of triflingly small systems.
Since you can't open .uassets in a text editor, it's hard to glean my coding thought process from this project unless you already have Unreal installed and updated, and know where to look.
As such, for the Quick Reference, I included a video of my workstation going through the main blueprint handling the painting mechanic.



NavScape
Unreal 5, C++

This was a "pure" C++ project in Unreal 5 that attempted to both procedurally generate a mesh, to show visually, before converting it to a navigation mesh thereafter - got 78% total, identical to GGW (First) 
Running it showcases a glaring bug: limited by the project's runtime, a memory leak crashes the game relatively quickly for complex navigation, which I wish I'd had time to fix before the deadline. 
However, the mathematics for coding random number generation by hand from a seed is solid, as well as the mesh generation, so it's still a good showcase as to what I consider shippable - even if I'd keep it in the oven for a while longer were it actually going to be shipped.



Cosmos Math
Unity, C#

My oldest project here, from 2nd year, mostly included because I wanted to show off some C# work - got 91% total (First) 
It's a relatively rudimentary orrery emulator (solar system showcase), but the part bringing home that 91% money is that I tried to code a custom maths API underneath the hood of the entire simulation.
"AshVectors" (in the Quick Reference) is this API - it's full of "AshVectors", "AshQuaternions", etc, with every common operator built from the ground up. T
As far as I'm aware, there's no good commercial reason to do this outside of developing an in-house engine - Unity's maths libraries and documentation are more than usable. However, it sure showcases the extents to which I know C# and the Unity Engine! Check it out!



terraintest.hipnc
Houdini

This is a more visual, Technical Artist-esque project, so I haven't prioritised it too much, but I also have Houdini experience! If you have the tool avaliable, you can open this file up and see a mountain generator here, flexible enough to generate three environments.



snowgraverun
LUA

A common worry I have as a game developer is that I feel my peers develop smaller games in their free time more often than I do. In pseudo-interviews, I'm often asked to share the games I make for fun and that's never been the kind of developer I am. I have primarily treated this as a career, not pursuing it as a hobby.
I do worry this might reflect poorly on me to some interviewers, though - and by chance, when I received the request to put this repo together, I was actually learning to mod The Binding of Isaac for the first time just for fun. 
As such, I decided to include that mod in this repo, too. Take this one with a grain of salt, it's not exactly a lot of code, but since it might be used as code reference for other modders, I tried to display my thought process in the comments. 
For a language and API I'd never touched before, I'm pretty proud of that result for only three or so hours' work! But three hours is ... ridiculously small compared to my other projects. 
