Edit Butler
===========

Butler.xml
----------
Edit Butler.xml, so you can change your butler's information and appearances.


### 1. Butler's name and information

```
<Butler 
    Name="Emotional-Jenkins"            ... (1)
    Nickname="Mr. Jenkins"              ... (2)
    DisplayName="Emotional Jenkins"     ... (3)
    License="jenkins.png and..."        ... (4)
    TypicalAppearance="Sad">            ... (5)
```

![Butler (Configuration)](./images/Configuration-07.png?raw=true)

![Butler (Tasktray)](./images/Tasktray-01.png?raw=true)



### 2. Appearances

A butler has appearances that according to feelings (e.g. Normal, Angry, Sad and Happy).

```
<Butler>
  <Butler.Appearances>
    <Appearance x:Key="Angry" />
    <Appearance x:Key="Sad" />
    <Appearance x:Key="Normal" />
    <Appearance x:Key="Happy" />
  </Butler.Appearances>
</Butler>
```

```
<Appearance 
    IconFile="jenkins_icon.ico"                     ... (1)
    BalloonTipText="{}{Subject} {CurrentStatus}"    ... (2)
    BalloonTipTitle="{}{Title}"                     ... (3) 
    BalloonTipTimeout="10"                          ... The time period in seconds that the balloon tip should display.
    ImageFile="jenkins.png"                         ... (4)
    MessageText="{}{Subject} {CurrentStatus}"       ... (5)
    Topmost="False"                                 ... True if the butler's window is topmost.
    x:Key="Normal">                                 ... A feeling that the appearance imply.
```

![Appearance (BallonTip)](./images/Butler-01.png?raw=true)

![Appearance (Batler Window)](./images/Butler-02.png?raw=true)



### 3. Message Style

```
<Appearance>
  <Appearance.MessageStyle>
    <MessageStyle/>
  </Appearance.MessageStyle>
</Appearance>
```

<pre>
&lt;MessageStyle 
    Position="Right"                ... <a href="../JenkinsOnDesktop/Core/MessagePosition.cs">MessagePosition</a>
    Width="200"                     ... (1) double
    Height="200"                    ... (2) double
    Padding="12,12,12,12"           ... (3) <a href="http://msdn.microsoft.com/en-us/library/system.windows.thickness(v=vs.100).aspx">Thickness</a>
    FontSize="14"                   ... (4) double
    FontFamily="Meiryo"             ... (5) <a href="http://msdn.microsoft.com/en-us/library/system.windows.media.fontfamily(v=vs.100).aspx">FontFamily</a>
    Foreground="#FF000000"          ... (6) <a href="http://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.100).aspx">Brush</a>
    BorderBrush="#FF000000"         ... (7) <a href="http://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.100).aspx">Brush</a>
    BorderThickness="6,6,6,6"       ... (8) <a href="http://msdn.microsoft.com/en-us/library/system.windows.thickness(v=vs.100).aspx">Thickness</a>
    CornerRadius="8,8,8,8"          ... (9) <a href="http://msdn.microsoft.com/en-us/library/system.windows.cornerradius(v=vs.100).aspx">CornerRadius</a>
    Background="#FFFFFFFF"          ... (10) <a href="http://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.100).aspx">Brush</a>
    BackgroundFile="msg.png" /&gt;     ... string
</pre>

![MessageStyle](./images/MessageStyle-01.png?raw=true)


### 4. Animation
```
<Appearance>
  <Appearance.EnterAnimation>	... Entering animations of the butler.
    <Operation />
    <SlideIn />
    ...
  </Appearance.EnterAnimation>
  <Appearance.ExitAnimation>	... Exiting animations of the butler.
    <Operation />
    <SlideOut />
    ...
  </Appearance.ExitAnimation>
</Appearance>
```

#### Operation
<pre>
&lt;Operation 
    BeginTime="00:00:01"        ... <a href="http://msdn.microsoft.com/en-us/library/system.timespan(v=vs.100).aspx">TimeSpan</a>
    Command="UpdateWindow"      ... <a href="../JenkinsOnDesktop/Animation/Command.cs">Command</a>
/&gt;
</pre>

#### SlideIn
<pre>
&lt;SlideIn
    BeginTime="00:00:01"        ... <a href="http://msdn.microsoft.com/en-us/library/system.timespan(v=vs.100).aspx">TimeSpan</a>
    Duration="00:00:01"         ... <a href="http://msdn.microsoft.com/en-us/library/system.windows.duration(v=vs.100).aspx">Duration</a>
    Direction="Right"           ... <a href="../JenkinsOnDesktop/Animation/Direction.cs">Direction</a>
    Position="LeftBottom"       ... <a href="../JenkinsOnDesktop/Animation/Position.cs">Position</a>
/&gt;
</pre>


#### SlideOut
<pre>
&lt;SlideOut
    BeginTime="00:00:01"        ... <a href="http://msdn.microsoft.com/en-us/library/system.timespan(v=vs.100).aspx">TimeSpan</a>
    Duration="00:00:01"         ... <a href="http://msdn.microsoft.com/en-us/library/system.windows.duration(v=vs.100).aspx">Duration</a>
    Direction="Right"           ... <a href="../JenkinsOnDesktop/Animation/Direction.cs">Direction</a>
/&gt;
</pre>

#### FadeIn
<pre>
&lt;FadeIn
    BeginTime="00:00:01"        ... <a href="http://msdn.microsoft.com/en-us/library/system.timespan(v=vs.100).aspx">TimeSpan</a>
    Duration="00:00:01"         ... <a href="http://msdn.microsoft.com/en-us/library/system.windows.duration(v=vs.100).aspx">Duration</a>
    Position="LeftBottom"       ... <a href="../JenkinsOnDesktop/Animation/Position.cs">Position</a>
/&gt;
</pre>

#### FadeOut
<pre>
&lt;FadeOut
    BeginTime="00:00:01"        ... <a href="http://msdn.microsoft.com/en-us/library/system.timespan(v=vs.100).aspx">TimeSpan</a>
    Duration="00:00:01"         ... <a href="http://msdn.microsoft.com/en-us/library/system.windows.duration(v=vs.100).aspx">Duration</a>
/&gt;
</pre>



feel.ps1
--------
You can program the butler's feelings arise from reading a [report](./Report.md).

*   The feel.ps1 is a PowerShell script.
*   It must have a Main function as an entry point.
*   The Main function must return a feeling as a string.
*   Automatic variable $report is available as latest [report](./Report.md).

### Examples
*   [Calm-Jenkins](../JenkinsOnDesktop/Resources/Scripts/Butlers/Calm-Jenkins/feel.ps1)
*   [Emotional-Jenkins](../JenkinsOnDesktop/Resources/Scripts/Butlers/Emotional-Jenkins/feel.ps1)
