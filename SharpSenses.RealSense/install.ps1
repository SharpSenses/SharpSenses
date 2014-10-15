param($installPath, $toolsPath, $package, $project)

$configItem = $project.ProjectItems.Item("libpxccpp2c.dll")

# set 'Copy To Output Directory' to 'Copy if newer'
$copyToOutput = $configItem.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = 1

# set 'Build Action' to 'Content'
$buildAction = $configItem.Properties.Item("BuildAction")
$buildAction.Value = 2

#thanks NLOG for this code :)