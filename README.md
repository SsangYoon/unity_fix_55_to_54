# unity-fix-55-to-54

![Englsh](https://img.shields.io/badge/Language-English-lightgrey.svg) 
[![Korean](https://img.shields.io/badge/Language-Korean-blue.svg)](README_KR.md)

- Unity version downgrade tool

## Features
- Fix Unity 5.5 Downgrading to 5.4 missing GameObject name, activeSelf.

## How to use
- In your project's editor settings, make sure your asset serialization mode is set to "Force Text". This is absolutely crucial.
- For each scene you have create a prefab containing all game objects in that scene. The reason for that is that scenes cannot be forced to use a text format. So instead the converter goes through all prefabs and fixes them.
- Throw your project's assets folder into the "Put your assets folder here" folder.
- Run the tool and press enter to start converting.
- Your assets folder inside "Put your assets folder here" is now converted. So go ahead and delete your project's old assets and library folders and then copy your new assets folder over there.
- Run your project in Unity 5.4 and tell it to continue even though the project versions mismatch.

## Known issues
- Particle systems that use the Sub module will have their Sub module reset.
- If the project was originally created before 5.5, then any textures\sprites added to the project after it was upgraded to 5.5 will need to have their settings reset. It's as simple as changing the texture type to something else and then back to what it's meant to be. Just make sure the mipmaps\compression settings are set to what you expect them to be.
- Any image\button components added while the project version was 5.5 will be corrupted. You'll either need to remove and add those components manually or upgrade the source code to fix the missing GUIDs on those components(read MykolaDenysenko's comment on this)


## Copyright
- Source from [here](https://forum.unity.com/threads/behold-the-legendary-unity-5-5-to-5-4-downgrader.457905/)
