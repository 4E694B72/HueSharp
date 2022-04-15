# HueSharp
This is a small custom SDK I've been hacking in a day. Because of this, it's obivious that this is Lib is not finished yet.
To keep it flexible it's a .NET Standard 2.1 lib.

## Discovery
To automatically find the Hue bridge(s) you can use one of both discovery classes, `SSDPAutoBridgeDiscover` or `MDNSAutoBridgeDiscover`.
After some testing in different networks I decided to use a timeout of 30 seconds. This means when calling the method `GetHuesInNetwork` it will return after that timeout with a List of strings containing the IPs of found bridges in the network.

## Client connection
Depending on your bridge version, you can choose between the V1 and V2 client. For V2, the bridge version must be at least 1948086000. Currently I did not implement anything that will indicate the version. To retrieve the version from the bridge, you have to call the `/api/config` endpoint.

Depending if you have already setup the app on the bridge you can setup the client only with the IP, or together with the application and client key.

```csharp
var client = new HueClientV2(HUE_BRIDGE_IP);
```
 
 ```csharp
var client = new HueClientV2(HUE_BRIDGE_IP, APPLICATION_KEY, CLIENT_KEY);
```

The V2 client also contains the V1 since the newer API is still on early access.

## Handler
The client holds different handlers that are responsible for something specific.

To register your application on the bridge:

```csharp
await client.RegistrationHandler.RegisterApplication(APPLICATION_NAME, DEVICE_NAME);
```

It will return a tuple containing the application key (T1) and client key (T2).

## Command
To send commands to devices, you have to create a command and pass it to the handler.

Before sending a command, you have to prepare it for example like this

```csharp
var command = new LightBulbCommand();
command.TurnOn().SetColor(Color.Red).SendToBulb(LIGHT_BULB_ID);
```

and send it like this ...

```csharp
await client.LightBulbHandler.SendLightBulbCommandAsync(command);
```

## Security
You could install Signifys root CA in your key store but this lib does encapsulate it already on application level and checks the server certificate. If the check fails, the connection will not be estabslished since the lib will not downgrade from https to http.
