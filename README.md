# K-Scene

Scene View camera bookmarks for the Unity editor - save the current Scene View
camera as a named bookmark and jump straight back to it later, without
manually re-framing the view.

One of the Kingfisher K-Tools, built on the shared
[K-Setting](https://github.com/vuthelong/KSetting) backend. This package ships
with its own copy, so installing K-Scene on its own still gives you the
combined settings window.

## Features

- A bookmarks overlay docked in the Scene View
- Save the current Scene View camera - pivot, rotation, size and
  orthographic/perspective mode - as a new bookmark with one click
- Click a bookmark to jump the active Scene View straight back to it
- Rename a bookmark inline, or delete it when you no longer need it

Everything is editor-only - the assembly is `Editor`-platform only, so nothing
here is compiled into player builds.

## Install

Two channels. Take whichever suits - the package is the same either way.

### Package Manager (git URL)

Package Manager > **+** > **Install package from git URL...**, then paste
K-Scene's URL:

```
https://github.com/vuthelong/KScene.git
```

This tracks the default branch, so Package Manager's **Update** button pulls
new commits as they land. Unity keeps packages read-only in
`Library/PackageCache`.

> [!TIP]
> Install [K-Setting](https://github.com/vuthelong/KSetting) alongside it to
> fold K-Scene's settings into **Tools > KTools Setting** instead of opening
> its own window:
>
> ```
> https://github.com/vuthelong/KSetting.git
> ```

#### Pin a version (optional)

Append `#<version>` to either URL above to install a specific tag instead of
tracking the branch:

```
https://github.com/vuthelong/KSetting.git#1.0.4
https://github.com/vuthelong/KScene.git#0.1.0
```

A tag is a fixed point, so Update has nothing new to fetch while pinned to
one - move to a newer tag by repeating this step with the new `#<version>`.

### `.unitypackage`

Download the `.unitypackage` from the
[latest release](https://github.com/vuthelong/KScene/releases/latest) and
import it via **Assets > Import Package > Custom Package**. This is a
point-in-time snapshot, not a tracked install - re-download it to update.

Keep one copy per project, whichever channel you use - Unity rejects a second
with `Assembly with name 'Kingfisher.KScene' already exists`.

## Where your data lives

Your bookmarks and settings are written to a `.KData` folder at the root of
your project, next to `Assets/` and `Packages/` - not into the tool's folder,
so it survives updating or re-cloning the repository.

The folder carries a `.gitignore` of its own that excludes everything inside it,
so it stays out of version control without your project's `.gitignore` needing
an entry. Delete that file to commit the folder instead.

## Settings

**Tools > Kingfisher > KScene > Settings** opens K-Scene's own settings
window.

Install [K-Setting](https://github.com/vuthelong/KSetting) beside it and you get
**Tools > KTools Setting** instead - one window that every installed Kingfisher
tool folds its settings into. It finds the installed tools by reflection at load
time, so there is nothing to wire up.

K-Setting is optional. Without it, each tool keeps its own window.

## License

Proprietary - see [LICENSE.md](LICENSE.md). Licensed per purchase (Unity Asset
Store or a direct agreement with Kingfisher); it is not open source.
