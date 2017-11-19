# Rube Goldberg Challenge

**Developed by- Vidhur Voora**

**Rube Goldberg Challenge** is a very interesting High Immersive VR project. 

It can be experienced using the **Oculus Rift with touch controllers**. 

The aim for Rube Goldberg machine game is to challenge players to create contraptions capable of guiding a ball through several collectibles.
It has different levels with varied level of complexity.
It utilizes serveral aspects of VR.
1) Teleportation
2) Object Grabbing and Throwing
3) Object Menu
4) Game Play elements

As soon as you launch the game you will be introducted with a nice UI describing the rules and various controls.

**Things I like about this project**
This was my first project which introduced me to the concepts of High Immersive VR and helped me learn about Occulus SDK and SteamVR SDK.
The entire implementation of the Teleportation , object grabbing and throwing , menu items etc..was a great learning experience.

**Things I found challenging**
1) For a very long time I found it hard to have the teleportation working. 
Most of the times the Raycast hit was not getting detected properly, and I was able to get teleported to up in the air or beneath the ground
2) For now had to hardcode the logic to maintain a constant y, irrespective of the teleportation location. This is not ideal, but got me to a workable solution
3) Had issues with throwing the objects smoothly. Got it working by deleting the entire factory and bringing back to the project.

Overall it took me close to around **40 hours**  to get it to a decent shape. 

**Technology Stack**
This project is built using Unity 2017.2.0f3 , Oculus Unity Utilities 1.19.0, Steam VR Unity plugin v1.2.2

**References**
The following articles were used as a reference while developing this project
- https://developer.oculus.com/documentation/unity/latest/concepts/unity-ovrinput/


