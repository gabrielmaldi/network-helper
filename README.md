# NetworkHelper [![Build status](https://ci.appveyor.com/api/projects/status/592jibqiy3q1ge4m)](https://ci.appveyor.com/project/gabrielmaldi/network-helper)

NetworkHelper is a Windows utility that configures your (Windows) VPNs to use split tunneling and sits on the tray and waits for you to connect to or disconnect from them; when this happens, it automatically adds and removes pre-configured routes (using `route add` and `route delete` just like you would using the Command Prompt).

## Getting started

Download a `.zip` file with the latest binaries from the [releases page](https://github.com/gabrielmaldi/network-helper/releases).

### Usage

- [ ] Add an in-depth guide.

- You need to edit `NetworkHelperConfiguration.xml` with something like this:
  ```xml 
  <Vpns>
    <Vpn>
      <Name>My VPN name</Name>
      <DnsSuffix>some.suffix.com</DnsSuffix>
      <Routes>
        <Route>
          <Description>A database server</Description>
          <DestinationIpAddress>192.168.44.33</DestinationIpAddress>
        </Route>
        <Route>
          <Description>A range of workstations</Description>
          <DestinationIpAddress>192.168.45.0</DestinationIpAddress>
          <Mask>255.255.255.0</Mask>
        </Route>
      </Routes>
    </Vpn>
  </Vpns>
  ```
  __It's essential that the VPN name in this configuration exactly matches the name of the VPN created in Windows.__
- When you connect to the VPN named `My VPN name` NetworkHelper will automatically add those routes; and when you disconnect from it, it will remove them.
- You can also add and remove routes at any time by right clicking on the tray icon.
- When stared, NetworkHelper will search Windows for matching VPNs and configure them to:
  - Use split tunneling.
  - Don't use RAS credentials (to allow some stuff like SQL Server Windows Authentication).
  - Use the provided DNS suffix (if any).
- You can configure NetworkHelper to start with Windows.
- There are more configuration options available, look at [Vpn.cs](https://github.com/gabrielmaldi/network-helper/blob/master/NetworkHelper/Classes/Vpn.cs) and [Route.cs](https://github.com/gabrielmaldi/network-helper/blob/master/NetworkHelper/Classes/Route.cs).

## Supported environments

NetworkHelper runs on Windows XP and up, both x86 and x64. However, it has only been tested on:

- Windows XP x86
- Windows Server 2008 R2

If you successfully run NetworkHelper on a different Windows version, open an issue to let me know and I'll list it here (or better yet: send a PR!).

Requirements:
- [.NET Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851)
- NetworkHelper needs administrator privileges in order to add and remove routes.

## Third party libraries that make it possible

- [DotRas](http://dotras.codeplex.com)
- [Task Scheduler Managed Wrapper](http://taskscheduler.codeplex.com)
- [log4net](http://logging.apache.org/log4net)
