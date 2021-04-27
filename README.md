This plugin simply allows you to generate bookmarks over any fixed interval. To use simply click the new button "Generate Bookmarks" on the right pop out panel in editor.

### Building
To build this project you need to add a `ChroMapperAutoBookmarker.csproj.user` file with the following contents.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <ChroMapperDir>C:\Path\To\ChroMapper</ChroMapperDir>
  </PropertyGroup>
</Project>
```

This will resolve dependencies in the project and copy the plugin to the plugins folder in this directory on build.