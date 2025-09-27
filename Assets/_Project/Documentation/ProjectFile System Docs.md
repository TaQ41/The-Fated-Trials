Covers the ProjectFile system. This includes:

+ [External access](#external-access).
+ [System details including methods.](#system-details)
+ [QuickPull subsystem](#quickpull-system)
+ [File handling](#file-handling)
+ [Focused objects (Acting on)](#interacting-datastructures)

###  Overview Details

This system is focused on the de/serialization of files that the game uses to persist data across sessions. Essentially the handler for 'game files'. 

Other classes may reference this system to perform actions on it, given requested parameters.

Typically, this will only be accessed at points of saving and moving from and to the `Start Menu`.

> `Start Menu` is the menu that includes creating, loading, and viewing game files. Among various other actions not related to this particular system.

## External Access

The system is primarily static with a few public methods to allow other classes to interact with this.

The structure for external access is made up of a single component that includes an InputHandler to execute methods based on the specified value of an enumerator defined for the different use cases.

So, the 'save btn' will be clicked with the enum value set to 'Save' for example. The method associated with that value will then be executed.

## File handling

> Saved Files Directory: "Assets\\_Project\\ProjectFile System\\Saved Files\\"

> QuickPullDataPath: "Assets\\_Project\\ProjectFile System\\Saved Files\\QuickPullData.json"

Files are saved to the `Saved Files Directory` and have parts of their Identification sections copied in the QuickPullData json file.

On file altering actions, such as, serializing or deleting, the actual file along with its counterpart in the QuickPullData are updated accordingly. No files are modified during the deserialization process.

#### Naming

The QuickPullDataPath is a constant path that is permanent and so, its name will not change.

Other files in the directory, such as, saved project files, are named by their guid and the ".json" extension.

## System Details

_Describes how the system works in conjuction with itself and would be used externally._

This will focus on the file @"ProjectFileUtilities.cs". As, the external methods that call this have already been covered [here](#external-access).

These are the primary methods:

1. [Serialization](#serialization)
2. [Deserialization](#deserialization)
3. [File Data Pulling](#file-data-pulling)
4. [File deletion](#file-deletion)

### Serialization

_awaitable bool method_

This method takes in a `ProjectFileHeader(PFH)` as a parameter. This is due to the PFH acting as a single point of all collective game data.

Effectively, this attempts to parse the passed PFH into json and then write to or create a file that matches its Guid. The success of this operation is read by the return value(bool) of the method. Immediately after the main write, the QuickPullData will also begin its write.

> Note: This awaits the main write, but does not await the quickPullData write.

### Deserialization

_void method_

This method takes in a string 'guid' and a `ProjectFileHeader(PFH)` as parameters. Here the PFH acts as a reference.

Essentially, this simply constructs a file's path by the Saved Files directory headed towards the guid named file with the .json extension. If the file is found, it will attempt to overwrite values of the PFH parameter with the serialized values on file.

> NOTE: This has to happen because ScriptableObjects, which is what the PFH is, must be overwritten when using the UnityEngine.JsonUtility.

### File Data Pulling

_QuickPullObjects[] method_

This method takes no parameters and operates in bulk.

After attempting to get the data at the QuickPullDataPath, this will return the item pool stored in the [wrapper class]. This is used to pull in concise data about all the files when selecting files.

> The Pool is a list, but this is converted to an array before being returned.

### File Deletion

_void method_

This takes only a string 'guid' parameter to locate the desired save file.

Effectively, to prevent warnings and unity generation, this deletes the .json save file and its associated .meta file. After that, an update to the quickPullSystem is issued to remove the quickPullObject instance of that save file.

## QuickPull System

Quintessentially, this is an optimization, made to relieve the irritation of deserializing an entire save file to get 3 or so fields.

So, a subsystem is used to allow serialization and mutations of data for quick references to saved files.

### QuickPullObjects

This is the object used to represent the very few set of fields that are necessary for displaying to the user or for the system to use.

This is a struct type that will always contain the string 'guid'; otherwise known as the file name. 

The `wrapper` uses an array of these to represent every saved file.

### Wrapper

The class name is "QuickPullWrapper", but this will often be shortened to "wrapper" in these docs.

This is a class only containing the collection of `QuickPullObjects`. This is required to de/serialize into json for the QuickPullData file.

This collection is referred to as the `Pool`.

### Updating Data

This works at the same times the main ProjectFile methods operate. As a method, this takes two parameters, the `QuickPullObject` this is acting on, and the mode (isDeletion?).

This first attempts to read the data file and get the index of the specified `QuickPullObject`. Then carries on to the next step based on the selected mode. There are two modes:

> The index starts at the Pool's count and then sets it to the index of an element if it was found. If it wasn't found, it will be the Pool's count for an index.

1. Deleting
2. Updating (adding / modifying)

After either of these actions have been proven successful, the wrapper will be serialized at the QuickPullDataPath.

#### Deleting an object

This modifies the Pool by tring to remove an element at its discovered index.

If the index is out of bounds of the Pool for whatever reason, purposefully due to the `QuickPullObject` not being found. Then, this will simply do nothing.

Otherwise, it will remove the `QuickPullObject` found at the discovered index.

#### Updating an object

This modifies the Pool by either adding a new element or changing the value at the specified index. This behaviour depends on the discovered index being in or out of bounds.

If the index is in range: The `QuickPullObject` will be set to the item at the discovered index.

If the indes is out of range: The `QuickPullObject` will instead be added onto the end of the Pool.

### Regenerating Data

[Wrapper Definition](#wrapper)

When: If the user suspects some error in files not showing up when they should or in cases where an error has happened in updating the QuickPullData.

This essentially, gets all the saved game files and adds them one by one if possible to the `wrapper` class before saving it to the QuickPullData file.

First this must setup the two objects needed for reconstructing the QuickPullData file, a `wrapper` class and a `ProjectFileHeader(PFH)`.

This will find all files in the Saved Files directory in which are json files. Using this, the QuickPullData file will be skipped. If a file fails to be deserialized, it will be skipped. Otherwise, it will add onto the Pool in the `wrapper` class.

Finally, the `wrapper` class is serialized and written to the QuickPullData file.

### Pulling Data

This is considered a main function of the entire system and so, it has been defined here: [File Data Pulling](#file-data-pulling).

## Interacting Datastructures

This system interacts with a few notable datastructures/objects.

The two most important are the `ProjectFileHeader(PFH)` and the `Identification` section of the PFH.

The `PFH` is the centerpiece for connecting saved file data for I/O operations and used as the structure for saved files in json.

The `Interaction` section is required for determining the guid of a project and other vital info to decipher which file it is to the user.

### QuickPull System Objects

However, using the [QuickPull System](#quickpull-system), another two objects come into the scene, being: `QuickPullObject` and the `QuickPullWrapper` (often shortened to just "wrapper").

The `QuickPullObject` is essential for storing only a few fields of data. This is the object the Pool utilizes.

The `QuickPullWrapper`, seeminly needless, actually allows the whole Pool to have I/O operations done legally on it. Without the wrapper class, this whole system wouldn't work.