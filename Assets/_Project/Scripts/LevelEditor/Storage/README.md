# Level Storage

Storage for levels can take different forms. The storage system has been built
to either only be local (ie levels are saved on the device being played on)
or to be able to sync with a server. The server could be:

 - A remote database
 - A website
 - Steamworks
 - etc

### Configuration

The options for configuring storage are in `EditorLevelStorage.cs` which sets up 
storage in its Awake method.
