# Table Of Contents

[Current Version: 1.0](#version-10) at the [ChangeLog](#changelog)

Cutscene Introduction - 

1. [What Are Cutscenes?](#what-are-cutscenes)
2. [Why are Cutscenes Played?](#why-are-cutscenes-played)

Cutscene System - 

1. [Where are cutscenes stored?](#where-are-cutscenes-stored)
2. [How do cutscenes get loaded or transformed into a state to be called?](#cutscene-references)
3. [How do cutscenes get played?](#cutscene-player)

# Cutscene Introduction

## What are Cutscenes?

Cutscenes in this project are scenes that usually only have a single set of standard input when activated and otherwise execute scripts on loading the scene to play a loaded video and provide a means to dispose of the scene when the video has ended. The test cutscenes simply use a time limit. This plays into why cutscenes are played.

The "single set of input" here, will likely just be a common script that leads to the same action map in the inputSettings. This will mostly just be actions that are common to video controls, with the extra capability of skipping the cutscene if allowed by the scene.

Another common script between cutscenes will be the script that disposes of the scene. This should let the Scene handle its disposal and simply call it. Although, there will have to be slightly more varied scripts to handle the various ways a cutscene may conduct itself. (IE. video or manual scene)

> Note: Some cutscenes may even include a complete object set that is interacted with in the duration of the cutscene.

### Why are Cutscenes Played?

Cutscenes are played to show something new of importance or visualize a lead moment in a story. This project uses cutscenes to introduce the player to locales, critical story events for each outcome triggered by the player, and sometimes just to exemplify an opponent's capabilities.

These cutscenes can be played for any moment when the setting should be introduced in a more "entrance-y" manner. Whether the setting be a halcyon to a maelstrom. 

# Cutscene Operations and Mechanics

## Where are cutscenes stored?

Cutscenes will actually have their own designated folder that as [_V1.0_](#version-10) is currently located at "Assets\\_Project\\Cutscenes". The cutscene scenes themselves will be stored as a folder containing the scene and any specific resources it uses. However, when cutscenes are being needed to be played, but cannot due to the cutscene that shouldn't be played within the circumstances, the cutscene should be passed onto the `InitialCutscene` list at the common `ProjectFileHeader.PendingProgressEvents` path. These are played when the locale bootup, boots up.

## Cutscene References

Reference does not mean, a change to this object will affect the scene itself, but rather that this stores the necessary information to play a cutscene given ideal conditions. This stores the name of the cutscene that should be loaded and any other details that affect whether the scene should be loaded at all.

## Cutscene Player

// Cover methods and inner workings of the cutscene queue player and how to generally use for different use cases within the project.

The cutscene player is internally based on a queue and hence, utilizes the `System.Collections.generic.Queue` object to store the cutscenes. These stored cutscenes have the lifespan of the current scene being loaded. Upon unloading the containing scene, they too will unload.

1. [Variables](#variables)
2. [Setting the queue](#setting-the-queue)
3. [Should a cutscene play logic](#validating-if-a-cutscene-should-play)
4. [Constraint validator](#constraint-validator)
5. [Cutscene Playing effects](#system-effects-when-playing-a-cutscene)
6. [Loading Cutscene Process](#loading-cutscene-process)
7. [Determining when a cutscene has ended](#determining-when-a-cutscene-has-ended)
8. [Cutscene Player End](#cutscene-player-ending)

### Variables

- projectFile = The projectFileHeader is set as is throughout the entire project and the object's lifetime.

- m_cutscenes = The queue used to store cutscenes is initialized as a new queue and never set back to default or null.

- m_loadedCutscenes = The list of cutscenes that successfully loaded to be accessed at the end and removed from the `PendingProgressEventsData.InitialCutscenes` collection if possible.

- m_currentSceneHandle = The cutscene player also stores a temporary int that represents the current active cutscene's scene handle used to compare unloading scenes with the active cutscene scene.

- m_isCutscenePlaying = The cutscene player protects against playing multiple scenes which is determined by a bool that is set to true when cutscenes are loading and reset when unloaded. Default state is false.

- completed = Action accessible from all user classes that is invoked when the player determines all available cutscenes have been played through.

### Setting the queue

The internal queue is accessed in two ways from outside the player, as of [_V1.0_](#version-10).

1. Adding cutscenes manually (one by one)

When adding cutscenes, it's important that there aren't any duplicate cutscenes (who wants to watch it twice immediately?). So, this first checks that the queue doesn't contain any equal cutscenes. However, cutscenes may be the same, but have different constraints, they still shouldn't be played again. So, instead the value that controls what cutscene is to play should be checked. The value that controls what cutscene asset is played is the `CutsceneName` string in the `CutsceneReference` object. Then, on the queue not containing the same cutscene type, this will add it to the queue.

2. Using all the cutscenes in `PendingProgressEventsData.InitialCutscenes`

This simply gets the collection from the location listed above and for each value, calls the manual add method on them. (to prevent duplicates)

**Added bonus details**

Comparing the name field rather than the entire object has a consistent decrease in time taken. The exceptions to this are when the queue currently has 0 items and when it has 1 item.

At 0 items, the name comparer takes more than double the amount of time to finish compared to comparing the entire object. This is likely due to the name comparer being wrapped in a foreach loop, while the Contains method on the queue is used for the comparing of the whole object. Where the Contains method may just return false if the collection has no items.

At 1 item, the complete opposite happens, and the object comparer takes surprisingly longer to complete then the name comparer.

Here are some logs looking into the situation

    Items: 0

    Comparing name time: 00:00:00.0001948
    Comparing object time: 00:00:00.0000799

    Items: 1

    Comparing name time: 00:00:00.0000381
    Comparing object time: 00:00:00.0002236

    Items: 2

    Comparing name time: 00:00:00.0000005
    Comparing object time: 00:00:00.0000015

    Items: 998

    Comparing name time: 00:00:00.0000804
    Comparing object time: 00:00:00.0001030

### Validating if a cutscene should play

Cutscenes shouldn't play everywhere and this project will usually add cutscenes to the `InitialCutscenes` collection on an event which will require a cutscene about it later. (IE. key story moment)

Rather than checking at the moment of the event and holding it until it can be played, it is directly added to the `InitialCutscenes` collection to be interpreted there. Then, when calling the cutscene from the cutscenePlayer, its constraints are checked and if it passes, the cutscene is played. However, before any of that happens, the [m_isCutscenePlaying field](#variables) should be false, then the [m_cutscenes](#variables) should not have a count of zero.

#### Constraint Validator

This is the method responsible for carrying out this action. Its name is actually "AreCutsceneConstraintsPassed" which takes a `CutsceneReference` and outputs a bool. This as of [_V1.0_](#version-10), only checks if the player's village locale is included in the selected locales the cutscene is allowed to be played in.

### System effects when playing a cutscene

When a cutscene is able to play (see [above](#validating-if-a-cutscene-should-play)), it will set the [m_isCutscenePlaying](#variables) to true and dequeue the cutscene. The [m_loadedCutscenes](#variables) then is appended with the fetched cutscene. At the end of the cutscene loading, the cutscene handle is set to the [m_currentSceneHandle](#variables).

### Loading Cutscene Process

After a cutscene has been decided to be eligible to play, the cutscene is loaded via the `SceneManager.LoadSceneAsync` method and is awaited to allow the [m_currentSceneHandle](#variables) to be set on time. This also doesn't remove the cutscene from the queue until after it has been determined to be able to be played.

> The `LoadSceneMode` is additive.

### Determining when a cutscene has ended

This utilizes the `SceneManager.sceneUnloaded` UnityAction member. It is subscribed to on the OnEnable and OnDisable calls of the Monobehaviour. The method being subscribed is the "EndOfCutscene(Scene scene)" method.

On the event being called, the unloaded scene handle is compared to the [m_currentSceneHandle](#variables) to determine scene identicality. This will of course, determine that the [m_isCutscenePlaying](#variables) is false and play the next cutscene. On a mismatch, this promptly does nothing.

### Cutscene Player Ending

This is triggered when the cutscene player determines that the [m_cutscenes](#variables) count is 0. This then removes all cutscenes from the `PendingProgressEventsData.InitialCutscenes` collection and clears the [m_loadedCutscenes](#variables) collection. After all of this, the [completed](#variables) Action is invoked.

# ChangeLog

Store and remember changes to the cutscene system since its first working implementation.

## Version 1.0

The Cutscene system works as intended at a rudimentary level in order for the rest of the project to be worked on without having to omit cutscene loading logic.