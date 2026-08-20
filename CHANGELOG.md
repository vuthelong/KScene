# Changelog

All notable changes to K-Scene are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- A Scene View bookmarks overlay - save the current Scene View camera as a
  named bookmark and jump straight back to it later.
- Save the current Scene View camera - pivot, rotation, size and
  orthographic/perspective mode - as a new bookmark with one click.
- Click a bookmark to jump the active Scene View straight back to it.
- Rename a bookmark inline, or delete it when you no longer need it.
- Per-tool settings window for when K-Setting is not installed; with it, the
  settings fold into **Tools > KTools Setting** instead.
- Installs as a UPM package from its git URL, or as a plain folder under
  `Assets/`.
- Editor-only: the assembly is `Editor`-platform only, so nothing is compiled
  into player builds.
