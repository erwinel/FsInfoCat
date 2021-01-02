#
# Module manifest for module 'FsInfoCat'
#
# Generated by: lerwi
#
# Generated on: 12/28/2020
#

@{

# Script module or binary module file associated with this manifest.
RootModule = 'FsInfoCat.psm1'

# Version number of this module.
ModuleVersion = '0.1'

# Supported PSEditions
CompatiblePSEditions = @('Core')

# ID used to uniquely identify this module
GUID = 'fc9c3e2e-257f-45e8-b233-49115f58c375'

# Author of this module
Author = 'Leonard T. Erwine'

# Company or vendor of this module
CompanyName = 'Leonard T. Erwine'

# Copyright statement for this module
Copyright = '(c) 2021 Leonard T. Erwine. All rights reserved.'

# Description of the functionality provided by this module
Description = 'Provides file system crawling for FsInfoCat.Web.'

# Minimum version of the Windows PowerShell engine required by this module
PowerShellVersion = '7.0'

# Name of the Windows PowerShell host required by this module
# PowerShellHostName = ''

# Minimum version of the Windows PowerShell host required by this module
# PowerShellHostVersion = ''

# Processor architecture (None, X86, Amd64) required by this module
# ProcessorArchitecture = ''

# Modules that must be imported into the global environment prior to importing this module
# RequiredModules = @()

# Assemblies that must be loaded prior to importing this module
RequiredAssemblies = @('FsInfoCat.PS.dll')

# Script files (.ps1) that are run in the caller's environment prior to importing this module.
# ScriptsToProcess = @()

# Type files (.ps1xml) to be loaded when importing this module
# TypesToProcess = @()

# Format files (.ps1xml) to be loaded when importing this module
# FormatsToProcess = @()

# Modules to import as nested modules of the module specified in RootModule/ModuleToProcess
NestedModules = @('FsInfoCat.PS.dll')

# Functions to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no functions to export.
FunctionsToExport = @('Test-CsClassName', 'Test-FileInfo', 'Read-CsClassName', 'Read-Choice', 'Read-YesOrNo', 'Convert-ToCommentLines',
    'New-WpfWindowScaffold', 'New-MvcScaffold', 'Read-CsTypeModel', 'New-DependencyProperty', 'Read-DependencyProperty',
    'ConvertTo-PasswordHash', 'Get-SaltBytes', 'Test-PasswordHash', 'Get-InitializationQueries')

# Cmdlets to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no cmdlets to export.
CmdletsToExport = '*'

# Variables to export from this module
VariablesToExport = '*'

# Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
AliasesToExport = @()

# DSC resources to export from this module
# DscResourcesToExport = @()

# List of all modules packaged with this module
ModuleList = @()

# List of all files packaged with this module
FileList = @()

# Private data to pass to the module specified in RootModule/ModuleToProcess. This may also contain a PSData hashtable with additional module metadata used by PowerShell.
PrivateData = @{

    PSData = @{

        # Tags applied to this module. These help with module discovery in online galleries.
        # Tags = @()

        # A URL to the license for this module.
        LicenseUri = 'https://github.com/lerwine/FsInfoCat/blob/main/LICENSE'

        # A URL to the main website for this project.
        ProjectUri = 'https://github.com/lerwine/FsInfoCat'

        # A URL to an icon representing this module.
        # IconUri = ''

        # ReleaseNotes of this module
        # ReleaseNotes = ''

    } # End of PSData hashtable

} # End of PrivateData hashtable

# HelpInfo URI of this module
# HelpInfoURI = ''

# Default prefix for commands exported from this module. Override the default prefix using Import-Module -Prefix.
# DefaultCommandPrefix = ''

}

